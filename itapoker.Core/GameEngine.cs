using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Domain.Requests;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core;

public class GameEngine : IGameEngine
{
    private readonly IDealerService _dealerService;
    private readonly IDecisionService _decisionService;    
    private readonly IGameRepo _gameRepo;
    private readonly IHighScoreRepo _highScoreRepo;

    public GameEngine(
        IDealerService dealerService,
        IDecisionService decisionService,        
        IGameRepo gameRepo,
        IHighScoreRepo highScoreRepo)
    {
        _dealerService = dealerService;
        _decisionService = decisionService;        
        _gameRepo = gameRepo;
        _highScoreRepo = highScoreRepo;
    }

    public Game AnteUp(AnteUpRequest request)
    {
        // the ante is currently automatically added for each
        // player in the game based on the ante specified
        // when creating the game

        _dealerService.AnteUp(request.GameId);

        return _gameRepo.GetByGameId(request.GameId);
    }

    public Game Bet(BetRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        // let's handle pre draw betting separately from
        // post draw betting

        if (game.Stage == GameStage.BetPreDraw)
        {
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
                game.Stage = GameStage.Draw;
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

            var betType = _decisionService.GetBetType(game);

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

                game.Stage = GameStage.Draw;
            }
            else if (request.BetType == BetType.Call)
            {
                // if the ai player is calling, they are matching
                // the player bet and asking to proceed to the draw

                game.AIPlayer.LastBetAmount = game.Player.LastBetAmount;
                game.AIPlayer.LastBetType = BetType.Call;
                game.AIPlayer.Cash -= game.AIPlayer.LastBetAmount;

                game.Pot += game.AIPlayer.LastBetAmount;
                game.Stage = GameStage.Draw;
            }
            else if (request.BetType == BetType.Raise)
            {
                // if the ai player is raising, they are matching
                // the previous player bet, but then raising the
                // bet by an amount

                game.AIPlayer.LastBetAmount = request.Amount;
                game.AIPlayer.LastBetType = BetType.Raise;
                game.AIPlayer.Cash -= game.Player.LastBetAmount;
                game.AIPlayer.Cash -= request.Amount;

                game.Pot += game.Player.LastBetAmount;
                game.Pot += request.Amount;
            }
        }

        return _gameRepo.AddOrUpdate(game);
    }

    public Game Deal(DealRequest request)
    {
        // dealer shuffles the deck and then distributes
        // five cards to each player, one card to each player
        // at a time

        _dealerService.Shuffle(request.GameId);
        _dealerService.Deal(request.GameId);

        return _gameRepo.GetByGameId(request.GameId);
    }

    public Game SinglePlayer(SinglePlayerRequest request)
    {
        _gameRepo.Truncate();

        var game = new Game {
            GameId = Guid.NewGuid().ToString(),
            Stage = GameStage.NewGame,
            Ante = request.Ante,
            Cash = request.Cash,
            Limit = request.Limit,
            Players = new() {
                new() {
                    Name = request.PlayerName,
                    PlayerId = Guid.NewGuid().ToString(),
                    PlayerType = PlayerType.Human,
                    Cash = request.Cash,
                },
                new() {
                    Name = "AI Player",
                    PlayerId = Guid.NewGuid().ToString(),
                    PlayerType = PlayerType.Computer,
                    Cash = request.Cash
                }
            }
        };

        return _gameRepo.AddOrUpdate(game);
    }
}