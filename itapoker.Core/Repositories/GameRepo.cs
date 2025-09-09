using itapoker.Core.Domain.Models;
using itapoker.Core.Repositories.Interfaces;
using LiteDB;

namespace itapoker.Core.Repositories;

public class GameRepo : IGameRepo
{
    private const string DatabaseName = "itapoker.db";

    public Game AddOrUpdate(Game game)
    {
        using var context = new LiteDatabase(DatabaseName);

        var collection = context.GetCollection<Game>();

        var existing = collection.FindOne(x => x.GameId == game.GameId);

        if (existing == null)
            existing = new Game();

        existing.GameId = game.GameId;
        existing.Ante = game.Ante;
        existing.Cash = game.Cash;
        existing.Limit = game.Limit;
        existing.Pot = game.Pot;
        existing.Stage = game.Stage;
        existing.Players = game.Players;
        existing.Deck = game.Deck;

        collection.Upsert(existing);

        return existing;
    }

    public Game Get(int id)
    {
        using var context = new LiteDatabase(DatabaseName);

        var collection = context.GetCollection<Game>();

        return collection.FindById(id);
    }

    public Game GetByGameId(string gameId)
    {
        using var context = new LiteDatabase(DatabaseName);

        var collection = context.GetCollection<Game>();

        return collection.FindOne(x => x.GameId == gameId);
    }

    public void Truncate()
    {
        using var context = new LiteDatabase(DatabaseName);

        var collection = context.GetCollection<Game>();

        collection.DeleteAll();
    }
}