using itapoker.Core.Domain.Models;

namespace itapoker.Core.Repositories.Interfaces;

public interface IGameRepo
{
    Game AddOrUpdate(Game game);
    Game Get(int id);
    Game GetByGameId(string gameId);
    void Truncate();
}