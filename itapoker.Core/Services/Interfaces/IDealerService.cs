using itapoker.Core.Domain.Models;

namespace itapoker.Core.Interfaces;

public interface IDealerService
{
    void Deal(string gameId);
    void Shuffle(string gameId);
}