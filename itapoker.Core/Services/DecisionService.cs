using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Services.Interfaces;

namespace itapoker.Core.Services;

public class DecisionService : IDecisionService
{
    public List<Card> GetDiscardedCards(Game game)
    {
        var random = new Random();

        var total = random.Next(0, 3);

        random = new Random();

        var cards = new List<Card>();

        for (var i = 0; i < total; i++)
            cards.Add(game.AIPlayer.Cards[random.Next(0, 4)]);

        return cards;
    }

    public (BetType, int) GetBet(Game game)
    {
        var random = new Random();

        var betType = (BetType)(random.Next(1, 4));
        var amount = 0;

        if (betType == BetType.Raise)
            amount = game.Player.LastBetAmount;

        return (betType, amount);
    }
}