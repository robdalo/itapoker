using itapoker.Core.Domain.Models;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core.Services;

public class DealerService : IDealerService
{
    private readonly IGameRepo _gameRepo;

    public DealerService(IGameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public void Deal(string gameId)
    {
        var game = _gameRepo.GetByGameId(gameId);

        for (var i = 0; i < 5; i++)
        {
            foreach (var player in game.Players.OrderByDescending(x => x.PlayerType))
            {
                player.Cards.Add(game.Deck.Last());
                game.Deck.Remove(game.Deck.Last());
            }
        }

        _gameRepo.AddOrUpdate(game);
    }

    public void Shuffle(string gameId)
    {
        var game = _gameRepo.GetByGameId(gameId);

        var random = new Random();

        var cards = Deck.Cards;
        var total = Deck.Cards.Count;

        for (var i = 0; i < total; i++)
        {
            var index = random.Next(0, cards.Count - 1);
            game.Deck.Add(cards[index]);
            cards.RemoveAt(index);
        }

        _gameRepo.AddOrUpdate(game);
    }
}