using itapoker.Core.Domain.Enums;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core.Services;

public class PlayerService : IPlayerService
{
    private readonly IGameRepo _gameRepo;

    public PlayerService(IGameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public void AnteUp(string gameId)
    {
        var game = _gameRepo.GetByGameId(gameId);
        var player = game.Players.First(x => x.PlayerType == PlayerType.Human);

        player.Cash -= game.Ante;
        game.Pot += game.Ante;

        _gameRepo.AddOrUpdate(game);
    }
}