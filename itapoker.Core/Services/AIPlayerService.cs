using itapoker.Core.Domain.Enums;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core.Services;

public class AIPlayerService : IAIPlayerService
{
    private readonly IGameRepo _gameRepo;

    public AIPlayerService(IGameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public void AnteUp(string gameId)
    {
        var game = _gameRepo.GetByGameId(gameId);
        var player = game.Players.First(x => x.PlayerType == PlayerType.Computer);

        player.Cash -= game.Ante;
        game.Pot += game.Ante;

        _gameRepo.AddOrUpdate(game);
    }
}