using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Repositories.Interfaces;
using itapoker.Core.Services.Interfaces;

namespace itapoker.Core.Services;

public class DealerService : IDealerService
{
    private readonly IGameRepo _gameRepo;

    public DealerService(IGameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public void AnteUp(string gameId)
    {
        var game = _gameRepo.GetByGameId(gameId);

        foreach (var player in game.Players)
        {
            player.Cash -= game.Ante;
            game.Pot += game.Ante;
        }

        game.Stage = GameStage.Deal;

        _gameRepo.AddOrUpdate(game);
    }

    public void Deal(string gameId)
    {
        var game = _gameRepo.GetByGameId(gameId);

        for (var i=0; i<5; i++)
        {
            foreach (var player in game.Players.OrderByDescending(x => x.PlayerType))
            {
                player.Cards.Add(game.Deck.Last());
                game.Deck.Remove(game.Deck.Last());
            }
        }

        // sort cards
        
        foreach (var player in game.Players)
        {
            player.Cards = player.Cards.OrderBy(x => x.Rank)
                                       .ThenBy(x => x.Suit)
                                       .ToList();
        }

        game.Stage = GameStage.BetPreDraw;

        _gameRepo.AddOrUpdate(game);
    }

    public void Draw(string gameId, List<Card> cards)
    {
        var game = _gameRepo.GetByGameId(gameId);



        _gameRepo.AddOrUpdate(game);
    }

    public void Shuffle(string gameId)
    {
        var game = _gameRepo.GetByGameId(gameId);

        var random = new Random();

        var cards = Deck.Cards;
        var total = Deck.Cards.Count;

        game.Deck.Clear();

        for (var i = 0; i < total; i++)
        {
            var index = random.Next(0, cards.Count - 1);
            game.Deck.Add(cards[index]);
            cards.RemoveAt(index);
        }

        _gameRepo.AddOrUpdate(game);
    }
}