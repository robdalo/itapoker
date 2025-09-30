using System.Runtime.CompilerServices;
using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Services.Interfaces;

namespace itapoker.Core.Services;

public class DecisionService : IDecisionService
{
    private const double RandRangeMin = -0.25;
    private const double RandRangeMax = 0.25;

    private Dictionary<HandType, double> HandMultipliers => new()
    {
        { HandType.RoyalFlush, 1.0 },
        { HandType.StraightFlush, 0.9 },
        { HandType.FourOfAKind, 0.8 },
        { HandType.FullHouse, 0.7 },
        { HandType.Flush, 0.6 },
        { HandType.Straight, 0.5 },
        { HandType.ThreeOfAKind, 0.4 },
        { HandType.TwoPair, 0.3 },
        { HandType.OnePair, 0.2 },
        { HandType.AceHigh, 0.18 },
        { HandType.KingHigh, 0.16 },
        { HandType.QueenHigh, 0.14 },
        { HandType.JackHigh, 0.12 },
        { HandType.TenHigh, 0.10 },
        { HandType.NineHigh, 0.08 },
        { HandType.EightHigh, 0.06 },
        { HandType.SevenHigh, 0.04 }
    };

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
        // if player checks, the ai player may also check, or
        // alternatively they may raise

        if (game.Player.LastBetType == BetType.Check)
        {
            var maxBet = game.AIPlayer.Cash < game.Limit ?
                game.AIPlayer.Cash :
                game.Limit;

            var multiplier = RandomiseMultiplier(this.HandMultipliers[game.AIPlayer.HandType]);

            if (multiplier == 0)
                return (BetType.Check, 0);

            return (BetType.Raise, GetBetAmount((int)(maxBet * multiplier)));
        }

        // if player raises, the ai player may also raise, or they
        // may call the bet, or they may fold

        else
        {
            var playerCash = game.Player.Cash + game.Player.LastBetAmount;

            var maxBet = playerCash < game.Limit ?
                playerCash :
                game.Limit;

            var multiplier =
                RandomiseMultiplier(this.HandMultipliers[game.AIPlayer.HandType]) -
                RandomiseMultiplier(GetPlayerMultiplier(game));

            if (multiplier < -0.20)
                return (BetType.Fold, 0);
            else if (multiplier >= -0.20 && multiplier <= 0.20)
                return (BetType.Call, 0);
            else
                return (BetType.Raise, GetBetAmount((int)(maxBet * multiplier)));
        }
    }

    private int GetBetAmount(int value)
    {
        var total = 0;

        while (total <= value)
            total += 50;

        if (total == value)
            return total;

        total -= 50;

        while (total <= value)
            total += 25;

        if (total == value)
            return total;

        total -= 25;

        while (total <= value)
            total += 10;

        if (total == value)
            return total;

        total -= 10;

        while (total <= value)
            total += 5;

        if (total == value)
            return total;

        total -= 5;

        return Math.Max(0, total);
    }

    private double GetNearest(List<double> values, double value)
    {
        var result = 0.0;
        var difference = (double?)(null);

        foreach (var v in values.OrderBy(x => x))
        {
            var diff = Math.Abs(v - value);

            if (difference.HasValue && difference < diff)
                return result;

            difference = diff;
            result = v;
        }

        return result;
    }

    private double GetPlayerMultiplier(Game game)
    {
        var playerCash = game.Player.Cash + game.Player.LastBetAmount;

        var maxBet = playerCash < game.Limit ?
            playerCash :
            game.Limit;

        return GetNearest(
            values: this.HandMultipliers.Select(x => x.Value).ToList(),
            value: game.Player.LastBetAmount / maxBet);
    }

    private double RandomiseMultiplier(double multiplier)
    {
        var random = new Random();
        var randomness = Math.Round((double)(random.Next((int)(RandRangeMin * 100), (int)(RandRangeMax * 100)) / 100), 2);

        return Math.Max(Math.Min(multiplier + randomness, 1.0), 0.0);
    }
}