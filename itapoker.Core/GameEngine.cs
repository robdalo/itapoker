using itapoker.Core.Interfaces;
using itapoker.Core.Domain.Models;
using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Requests;
using itapoker.Core.Domain.Responses;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core;

public class GameEngine : IGameEngine
{
    private readonly IGameRepo _gameRepo;
    private readonly IHighScoreRepo _highScoreRepo;

    public GameEngine(IGameRepo gameRepo, IHighScoreRepo highScoreRepo)
    {
        _gameRepo = gameRepo;
        _highScoreRepo = highScoreRepo;
    }

    public NewGameResponse NewGame(NewGameRequest request)
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