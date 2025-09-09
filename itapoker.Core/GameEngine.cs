using itapoker.Core.Interfaces;
using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Domain.Requests;
using itapoker.Core.Domain.Responses;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core;

public class GameEngine : IGameEngine
{
    private readonly IGameRepo _gameRepo;
    private readonly IHighScoreRepo _highScoreRepo;
    private readonly IAIPlayerService _aiPlayerService;
    private readonly IDealerService _dealerService;
    private readonly IPlayerService _playerService;

    public GameEngine(
        IAIPlayerService aiPlayerService,
        IDealerService dealerService,
        IGameRepo gameRepo,
        IHighScoreRepo highScoreRepo,
        IPlayerService playerService)
    {
        _aiPlayerService = aiPlayerService;
        _dealerService = dealerService;
        _gameRepo = gameRepo;
        _highScoreRepo = highScoreRepo;
        _playerService = playerService;
    }

    public AnteUpResponse AnteUp(AnteUpRequest request)
    {
        _playerService.AnteUp(request.GameId);
        _aiPlayerService.AnteUp(request.GameId);

        var game = _gameRepo.GetByGameId(request.GameId);

        return new() {
            Pot = game.Pot
        };
    }

    public DealResponse Deal(DealRequest request)
    {
        _dealerService.Shuffle(request.GameId);
        _dealerService.Deal(request.GameId);

        var game = _gameRepo.GetByGameId(request.GameId);

        return new() {
            Cards = game.Players.First(x => x.PlayerType == PlayerType.Human).Cards
        };
    }

    public SinglePlayerResponse SinglePlayer(SinglePlayerRequest request)
    {
        _gameRepo.Truncate();

        var game = new Game
        {
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

        game = _gameRepo.AddOrUpdate(game);

        return new()
        {
            GameId = game.GameId,
            Ante = game.Ante,
            Cash = game.Cash,
            Limit = game.Limit,
            Players = game.Players
        };
    }

    public void UpdateHighScores()
    {
        _highScoreRepo.AddOrUpdate(new HighScore
        {
            PlayerName = "Player A",
            Score = 25000
        });

        _highScoreRepo.AddOrUpdate(new HighScore
        {
            PlayerName = "Player B",
            Score = 35000
        });

        _highScoreRepo.AddOrUpdate(new HighScore
        {
            PlayerName = "Player C",
            Score = 75000
        });
        
        _highScoreRepo.AddOrUpdate(new HighScore {
            PlayerName = "Player D",
            Score = 5000
        });        
    }
}