using itapoker.Core.Domain.Models;
using itapoker.Core.Repositories.Interfaces;
using LiteDB;

namespace itapoker.Core.Repositories;

public class HighScoreRepo : IHighScoreRepo
{
    private const string DatabaseName = "itapoker.db";

    public HighScore AddOrUpdate(HighScore highScore)
    {
        using var context = new LiteDatabase(DatabaseName);

        var collection = context.GetCollection<HighScore>();

        var existing = collection.FindOne(x => x.PlayerName == highScore.PlayerName);

        if (existing == null)
            existing = new HighScore();

        existing.PlayerName = highScore.PlayerName;
        existing.Score = highScore.Score;

        collection.Upsert(existing);

        return existing;
    }

    public List<HighScore> Get()
    {
        using var context = new LiteDatabase(DatabaseName);

        var collection = context.GetCollection<HighScore>();

        return collection.FindAll()
            .OrderByDescending(x => x.Score)
            .ToList();
    }

    public void Truncate()
    {
        using var context = new LiteDatabase(DatabaseName);

        var collection = context.GetCollection<HighScore>();

        collection.DeleteAll();
    }
}