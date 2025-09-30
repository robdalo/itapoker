using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Domain.Requests;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;
using itapoker.Core.Services.Interfaces;

namespace itapoker.Core;

public class GameEngine : IGameEngine
{
    private readonly ICardService _cardService;
    private readonly IDecisionService _decisionService;
    private readonly IGameRepo _gameRepo;

    public GameEngine(
        ICardService cardService,
        IDecisionService decisionService,
        IGameRepo gameRepo)
    {
        _cardService = cardService;
        _decisionService = decisionService;
        _gameRepo = gameRepo;
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

        foreach (var player in game.Players)
        {
            player.Cash -= game.Ante;
            game.Pot += game.Ante;
        }

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

        if (request.BetType == BetType.Fold)
        {
            // if the player is folding, we should end the game
            // immediately and pay out the ai player

            game.Player.LastBetAmount = 0;
            game.Player.LastBetType = BetType.Fold;

            game.AIPlayer.Cash += game.Pot;

            game.Pot = 0;
            game.Stage = GameStage.GameOver;
        }
        else if (request.BetType == BetType.Check)
        {
            // if the player is checking, the pot remains
            // unchanged and we pass control to the ai player

            game.Player.LastBetAmount = 0;
            game.Player.LastBetType = BetType.Check;
        }
        else if (request.BetType == BetType.Call)
        {
            // if the player is calling, they are matching
            // the ai players bet and asking to proceed to the draw

            game.Player.LastBetAmount = game.AIPlayer.LastBetAmount;
            game.Player.LastBetType = BetType.Call;
            game.Player.Cash -= game.Player.LastBetAmount;

            game.Pot += game.Player.LastBetAmount;

            game.Stage = GetNextGameStage(game.Stage);
        }
        else if (request.BetType == BetType.Raise)
        {
            // if the player is raising, they are matching
            // the previous ai player bet, but then raising the
            // bet by an amount

            game.Player.LastBetAmount = request.Amount;
            game.Player.LastBetType = BetType.Raise;
            game.Player.Cash -= game.AIPlayer.LastBetAmount;
            game.Player.Cash -= request.Amount;

            game.Pot += game.AIPlayer.LastBetAmount;
            game.Pot += request.Amount;
        }

        // if the player is folding or calling, we don't need
        // to process the ai player bet

        if (request.BetType == BetType.Fold || request.BetType == BetType.Call)
            return _gameRepo.AddOrUpdate(game);

        // process the ai player bet

        // we need to implement some ai logic that will
        // determine the ai players decision regarding which
        // type of bet to make - this will reside in decisionService

        var (betType, amount) = GetBet(game);

        if (betType == BetType.Fold)
        {
            // if the ai player is folding, we should end the game
            // immediately and pay out the player

            game.AIPlayer.LastBetAmount = 0;
            game.AIPlayer.LastBetType = BetType.Fold;

            game.Player.Cash += game.Pot;

            game.Pot = 0;
            game.Stage = GameStage.GameOver;
        }
        else if (betType == BetType.Check)
        {
            // if the ai player is checking, the pot remains
            // unchanged, the betting round ends and we proceed
            // to the draw

            game.AIPlayer.LastBetAmount = 0;
            game.AIPlayer.LastBetType = BetType.Check;

            game.Stage = GetNextGameStage(game.Stage);
        }
        else if (betType == BetType.Call)
        {
            // if the ai player is calling, they are matching
            // the player bet and asking to proceed to the draw

            game.AIPlayer.LastBetAmount = game.Player.LastBetAmount;
            game.AIPlayer.LastBetType = BetType.Call;
            game.AIPlayer.Cash -= game.AIPlayer.LastBetAmount;

            game.Pot += game.AIPlayer.LastBetAmount;

            game.Stage = GetNextGameStage(game.Stage);
        }
        else if (betType == BetType.Raise)
        {
            // if the ai player is raising, they are matching
            // the previous player bet, but then raising the
            // bet by an amount

            game.AIPlayer.LastBetAmount = amount;
            game.AIPlayer.LastBetType = BetType.Raise;
            game.AIPlayer.Cash -= game.Player.LastBetAmount;
            game.AIPlayer.Cash -= amount;

            game.Pot += game.Player.LastBetAmount;
            game.Pot += amount;
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
        }

        game.Stage = GameStage.BetPreDraw;
        
        game.Title = GetTitle(game.Stage);
        game.SubTitle = GetSubTitle(game.Stage);
        game.Alert = $"You have {_cardService.GetHandTitle(game.Player.HandType)}";

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
        game.Alert = $"You have {_cardService.GetHandTitle(game.Player.HandType)}";

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
            game.Player.Winnings += game.Pot / 2;
            game.AIPlayer.Winnings -= game.Pot / 2;
            game.Alert = $"{_cardService.GetHandTitle(playerHandType)} beats {_cardService.GetHandTitle(aiHandType)} - pay {game.Player.Name}";
        }
        else if (aiHandType > playerHandType)
        {
            game.AIPlayer.Cash += game.Pot;
            game.AIPlayer.Winnings += game.Pot / 2;
            game.Player.Winnings -= game.Pot / 2;
            game.Alert = $"{_cardService.GetHandTitle(aiHandType)} beats {_cardService.GetHandTitle(playerHandType)} - pay AI player";
        }
        else
        {
            game.Player.Cash += game.Pot / 2;
            game.AIPlayer.Cash += game.Pot / 2;
            game.Alert = $"It's a tie - pay all";
        }

        game.Pot = 0;
        game.Stage = GameStage.GameOver;
        game.Title = GetTitle(game.Stage);
        game.SubTitle = GetSubTitle(game.Stage);

        return _gameRepo.AddOrUpdate(game);
    }

    public Game SinglePlayer(SinglePlayerRequest request)
    {
        _gameRepo.Truncate();

        var game = new Game
        {
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
            }
        };

        return _gameRepo.AddOrUpdate(game);
    }

    private (BetType, int) GetBet(Game game)
    {
        if (game.NextBetType.HasValue && game.NextBetAmount.HasValue)
        {
            var betType = game.NextBetType.Value;
            var betAmount = game.NextBetAmount.Value;

            game.NextBetType = null;
            game.NextBetAmount = null;

            return (betType, betAmount);
        }

        return _decisionService.GetBet(game);
    }

    private GameStage GetNextGameStage(GameStage stage)
    {
        // if we are in the 1st round of betting, the next stage
        // is the draw. if we are in the 2nd round of betting, the
        // next stage is the showdown

        return stage == GameStage.BetPreDraw ?
            GameStage.Draw :
            GameStage.Showdown;
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
            case GameStage.Showdown: return "Highest ranking hand wins pot";

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