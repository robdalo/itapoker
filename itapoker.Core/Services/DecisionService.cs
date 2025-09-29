using System.ComponentModel;
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

        var maxBet = game.AIPlayer.Cash < game.Limit ?
            game.AIPlayer.Cash :
            game.Limit;

        if (game.Player.LastBetType == BetType.Check)
        {
            switch (game.AIPlayer.HandType)
            {
                case HandType.RoyalFlush: return (BetType.Raise, GetBetAmount(maxBet));
                case HandType.StraightFlush: return (BetType.Raise, GetBetAmount(maxBet * 0.9));
                case HandType.FourOfAKind: return (BetType.Raise, GetBetAmount(maxBet * 0.8));
                case HandType.FullHouse: return (BetType.Raise, GetBetAmount(maxBet * 0.7));
                case HandType.Flush: return (BetType.Raise, GetBetAmount(maxBet * 0.6));
                case HandType.Straight: return (BetType.Raise, GetBetAmount(maxBet * 0.5));
                case HandType.ThreeOfAKind: return (BetType.Raise, GetBetAmount(maxBet * 0.4));
                case HandType.TwoPair: return (BetType.Raise, GetBetAmount(maxBet * 0.3));
                case HandType.OnePair: return (BetType.Raise, GetBetAmount(maxBet * 0.2));
                case HandType.AceHigh: return (BetType.Raise, GetBetAmount(maxBet * 0.18));
                case HandType.KingHigh: return (BetType.Raise, GetBetAmount(maxBet * 0.16));
                case HandType.QueenHigh: return (BetType.Raise, GetBetAmount(maxBet * 0.14));
                case HandType.JackHigh: return (BetType.Raise, GetBetAmount(maxBet * 0.12));
                case HandType.TenHigh: return (BetType.Raise, GetBetAmount(maxBet * 0.10));
                case HandType.NineHigh: return (BetType.Raise, GetBetAmount(maxBet * 0.08));
                case HandType.EightHigh: return (BetType.Raise, GetBetAmount(maxBet * 0.06));
                case HandType.SevenHigh: return (BetType.Raise, GetBetAmount(maxBet * 0.04));

                default: return (BetType.Check, 0);
            }
        }
        else if (game.Player.LastBetType == BetType.Raise)
        {
            return (BetType.Call, 0);
        }
        else
        {
            return (BetType.Call, 0);
        }
    }

    private int GetBetAmount(double value)
    {
        return (int)(value);
    }
}