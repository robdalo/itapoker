using itapoker.Core.Interfaces;
using itapoker.Core.Models;
using itapoker.Core.Models.Enums;
using itapoker.Core.Models.Requests;
using itapoker.Core.Models.Responses;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core;

public class GameEngine : IGameEngine
{
    private readonly IGameRepo _gameRepo;

    public GameEngine(IGameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public NewGameResponse NewGame(NewGameRequest request)
    {
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

        return new() {
            GameId = game.GameId,
            Ante = game.Ante,
            Cash = game.Cash,
            Limit = game.Limit,
            Players = game.Players
        };
    }
}