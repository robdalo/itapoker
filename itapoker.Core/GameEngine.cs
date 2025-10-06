using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Domain.Requests;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;
using itapoker.Core.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace itapoker.Core;

public class GameEngine : IGameEngine
{
    private readonly IBetService _betService;
    private readonly ICardService _cardService;
    private readonly IDecisionService _decisionService;
    private readonly IGameRepo _gameRepo;
    private readonly GameSettings _settings;

    public GameEngine(
        IBetService betService,
        ICardService cardService,
        IDecisionService decisionService,
        IGameRepo gameRepo,
        IOptions<GameSettings> settings)
    {
        _betService = betService;
        _cardService = cardService;
        _decisionService = decisionService;
        _gameRepo = gameRepo;
        _settings = settings.Value;
    }

    public Game AddChip(AddChipRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        var existing = game.Player.Chips.FirstOrDefault(x => x.Value == request.Value);

        if (existing == null)
        {
            existing = new Chip(request.Value);
            game.Player.Chips.Add(existing);
        }

        existing.Quantity++;

        game.Player.Chips = game.Player.Chips.OrderBy(x => x.Value).ToList();

        game.Alert = null;

        return _gameRepo.AddOrUpdate(game);
    }

    public Game AnteUp(AnteUpRequest request)
    {
        // the ante is currently automatically added for each
        // player in the game based on the ante specified
        // when creating the game

        var game = _gameRepo.GetByGameId(request.GameId);

        game = _betService.ProcessAnteUp(game);

        game.Stage = GameStage.Deal;

        game.Title = GetTitle(game.Stage);
        game.SubTitle = GetSubTitle(game.Stage);
        game.Alert = null;

        return _gameRepo.AddOrUpdate(game);
    }

    public Game Bet(BetRequest request)
    {
        // this betting logic is used for both 1st round
        // and 2nd round betting

        var game = _gameRepo.GetByGameId(request.GameId);

        // process the player bet first, then the ai player bet

        game = _betService.ProcessPlayerBet(
            game: game,
            betType: request.BetType,
            amount: request.Amount);

        // if the player is folding or calling, we don't need
        // to process the ai player bet

        if (request.BetType != BetType.Fold && request.BetType != BetType.Call)
            game = _betService.ProcessAIPlayerBet(game);

        if (game.Player.LastBetType == BetType.Fold || game.AIPlayer.LastBetType == BetType.Fold)
        {
            game.Player.Cards.Clear();
            game.AIPlayer.Cards.Clear();
            game.Player.Hand = "";
            game.AIPlayer.Hand = "";
        }

        game.Player.Chips.Clear();
        game.AIPlayer.Chips.Clear();

        game.Title = GetTitle(game.Stage);
        game.SubTitle = GetSubTitle(game.Stage);
        game.Alert = null;

        return _gameRepo.AddOrUpdate(game);
    }

    public Game Deal(DealRequest request)
    {
        // dealer shuffles the deck and then distributes
        // five cards to each player, one card to each player
        // at a time

        var game = _gameRepo.GetByGameId(request.GameId);

        // shuffle

        var random = new Random();

        var cards = Deck.Cards;
        var total = Deck.Cards.Count;

        game.Deck.Clear();

        for (var i = 0; i < total; i++)
        {
            var index = random.Next(0, cards.Count - 1);
            game.Deck.Add(cards[index]);
            cards.RemoveAt(index);
        }

        // deal

        for (var i=0; i<5; i++)
        {
            foreach (var player in game.Players.OrderByDescending(x => x.PlayerType))
            {
                player.Cards.Add(game.Deck.Last());
                game.Deck.Remove(game.Deck.Last());
            }
        }

        foreach (var player in game.Players)
        {
            // sort cards

            player.Cards = player.Cards.OrderBy(x => x.Rank)
                                       .ThenBy(x => x.Suit)
                                       .ToList();

            // determine hand

            player.HandType = _cardService.GetHandType(player.Cards);

            // clear previous bets

            player.LastBetAmount = 0;
            player.LastBetType = BetType.None;
            player.LastBetChips = new();
        }

        game.Stage = GameStage.BetPreDraw;
        
        game.Title = GetTitle(game.Stage);
        game.SubTitle = GetSubTitle(game.Stage);
        game.Alert = $"{_cardService.GetHandTitle(game.Player.HandType)}";

        return _gameRepo.AddOrUpdate(game);
    }

    public Game Draw(DrawRequest request)
    {
        // player discards up to 3 cards and the dealer issues
        // replacements from the top of the deck

        var game = _gameRepo.GetByGameId(request.GameId);

        // process human player first

        // remove discarded cards from players hand

        game.Player.Cards.RemoveAll(x => request.Cards.Any(y => y.Suit == x.Suit &&
                                                                y.Rank == x.Rank));

        // deal new cards from top of deck

        for (var i = 0; i < request.Cards.Count; i++)
        {
            game.Player.Cards.Add(game.Deck.Last());
            game.Deck.Remove(game.Deck.Last());
        }

        // process ai player

        var cards = _decisionService.GetDiscardedCards(game);

        // remove discarded cards from ai players hand

        game.AIPlayer.Cards.RemoveAll(x => cards.Any(y => y.Suit == x.Suit &&
                                                          y.Rank == x.Rank));

        // deal new cards from top of deck

        for (var i = 0; i < cards.Count; i++)
        {
            game.AIPlayer.Cards.Add(game.Deck.Last());
            game.Deck.Remove(game.Deck.Last());
        }

        // clear holds

        foreach (var card in game.Players.SelectMany(x => x.Cards))
            card.Hold = false;

        foreach (var player in game.Players)
        {
            // sort cards

            player.Cards = player.Cards.OrderBy(x => x.Rank)
                                       .ThenBy(x => x.Suit)
                                       .ToList();

            // determine hand

            player.HandType = _cardService.GetHandType(player.Cards);

            // clear previous bets

            player.LastBetAmount = 0;
            player.LastBetType = BetType.None;
        }

        game.Stage = GameStage.BetPostDraw;
        game.Title = GetTitle(game.Stage);
        game.SubTitle = GetSubTitle(game.Stage);
        game.Alert = $"{_cardService.GetHandTitle(game.Player.HandType)}";

        return _gameRepo.AddOrUpdate(game);
    }

    public Game Hold(HoldRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        var card = game.Player.Cards.First(x => x.Rank == request.Rank &&
                                                x.Suit == request.Suit);

        card.Hold = !card.Hold;

        game.Alert = null;

        return _gameRepo.AddOrUpdate(game);
    }

    public Game Next(NextRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        foreach (var player in game.Players)
        {
            // return player cards to deck

            player.Cards.Clear();

            // clear last bets

            player.LastBetAmount = 0;
            player.LastBetType = BetType.None;
        }

        game.Hand++;
        game.Stage = GameStage.Ante;

        game.Title = GetTitle(game.Stage);
        game.SubTitle = GetSubTitle(game.Stage);
        game.Alert = null;

        return _gameRepo.AddOrUpdate(game);
    }

    public Game RemoveChip(RemoveChipRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        var existing = game.Player.Chips.FirstOrDefault(x => x.Value == request.Value);

        if (existing != null)
        {
            if (--existing.Quantity < 1)
                game.Player.Chips.Remove(existing);
        }

        game.Player.Chips = game.Player.Chips.OrderBy(x => x.Value).ToList();

        game.Alert = null;

        return _gameRepo.AddOrUpdate(game);
    }

    public Game SetDecision(SetDecisionRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        game.NextBetType = request.BetType;
        game.NextBetAmount = request.Amount;

        game.Alert = null;

        return _gameRepo.AddOrUpdate(game);
    }

    public Game Showdown(ShowdownRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        var playerHandType = _cardService.GetHandType(game.Player.Cards);
        var aiHandType = _cardService.GetHandType(game.AIPlayer.Cards);

        if (playerHandType > aiHandType)
        {
            game.Player.Cash += game.Pot;
            game.Alert = $"Pay {game.Player.Name}";
        }
        else if (aiHandType > playerHandType)
        {
            game.AIPlayer.Cash += game.Pot;
            game.Alert = $"Pay AI player";
        }
        else
        {
            game.Player.Cash += game.Pot / 2;
            game.AIPlayer.Cash += game.Pot / 2;
            game.Alert = $"Return all bets";
        }

        game.Player.Winnings = game.Player.Cash - game.Cash;
        game.AIPlayer.Winnings =  game.AIPlayer.Cash - game.Cash;
        game.Player.Hand = _cardService.GetHandTitle(playerHandType);
        game.AIPlayer.Hand = _cardService.GetHandTitle(aiHandType);
        game.Pot = 0;
        game.Stage = GameStage.GameOver;
        game.Title = GetTitle(game.Stage);
        game.SubTitle = GetSubTitle(game.Stage);

        return _gameRepo.AddOrUpdate(game);
    }

    public Game SinglePlayer(SinglePlayerRequest request)
    {
        _gameRepo.Truncate();

        var game = new Game {
            GameId = Guid.NewGuid().ToString(),
            Hand = 1,
            Stage = GameStage.Ante,
            Ante = Math.Max(request.Ante, 5),
            Cash = Math.Max(request.Cash, 500),
            Limit = Math.Max(request.Limit, 100),
            Title = GetTitle(GameStage.Ante),
            SubTitle = GetSubTitle(GameStage.Ante),
            Players = new() {
                new() {
                    Name = request.PlayerName,
                    PlayerId = Guid.NewGuid().ToString(),
                    PlayerType = PlayerType.Human,
                    Cash = Math.Max(request.Cash, 500)
                },
                new() {
                    Name = "AI Player",
                    PlayerId = Guid.NewGuid().ToString(),
                    PlayerType = PlayerType.Computer,
                    Cash = Math.Max(request.Cash, 500)
                }
            },
            BetChips = _settings.Chips
        };

        return _gameRepo.AddOrUpdate(game);
    }

    private string GetSubTitle(GameStage stage)
    {
        switch (stage)
        {
            case GameStage.Ante: return "Pay the ante to play";
            case GameStage.BetPostDraw: return "Place bets before showdown";
            case GameStage.BetPreDraw: return "Place bets before draw";
            case GameStage.Deal: return "Shuffle and deal cards";
            case GameStage.Draw: return "Hold cards before draw";
            case GameStage.GameOver: return "Play the next hand";
            case GameStage.Showdown: return "Highest rank hand wins pot";

            default: return "";
        }
    }

    private string GetTitle(GameStage stage)
    {
        switch (stage)
        {
            case GameStage.Ante: return "Ante Up";
            case GameStage.BetPostDraw: return "Bet";
            case GameStage.BetPreDraw: return "Bet";
            case GameStage.Deal: return "Deal";
            case GameStage.Draw: return "Draw";
            case GameStage.GameOver: return "Game Over";
            case GameStage.Showdown: return "Showdown";

            default: return "";
        }
    }
}