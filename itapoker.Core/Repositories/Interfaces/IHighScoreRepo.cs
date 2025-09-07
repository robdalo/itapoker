using itapoker.Core.Domain.Models;

namespace itapoker.Core.Repositories.Interfaces;

public interface IHighScoreRepo
{
    HighScore AddOrUpdate(HighScore highScore);
    List<HighScore> Get();
    void Truncate();
}