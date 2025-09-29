using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Services.Interfaces;

namespace itapoker.Core.Services;

public class CardService : ICardService
{
    public CardService()
    {
    }

    public string GetHandTitle(HandType handType)
    {
        switch (handType)
        {
            case HandType.TwoHigh: return "Two High";
            case HandType.ThreeHigh: return "Three High";
            case HandType.FourHigh: return "Four High";
            case HandType.FiveHigh: return "Five High";
            case HandType.SixHigh: return "Six High";
            case HandType.SevenHigh: return "Seven High";
            case HandType.EightHigh: return "Eight High";
            case HandType.NineHigh: return "Nine High";
            case HandType.TenHigh: return "Ten High";
            case HandType.JackHigh: return "Jack High";
            case HandType.QueenHigh: return "Queen High";
            case HandType.KingHigh: return "King High";
            case HandType.AceHigh: return "Ace High";
            case HandType.OnePair: return "One Pair";
            case HandType.TwoPair: return "Two Pair";
            case HandType.ThreeOfAKind: return "Three Of A Kind";
            case HandType.Straight: return "Straight";
            case HandType.Flush: return "Flush";
            case HandType.FullHouse: return "Full House";
            case HandType.FourOfAKind: return "Four Of A Kind";
            case HandType.StraightFlush: return "Straight Flush";
            case HandType.RoyalFlush: return "Royal Flush";

            default: return "";
        }
    }

    public HandType GetHandType(List<Card> cards)
    {
        if (IsRoyalFlush(cards))
            return HandType.RoyalFlush;
        else if (IsStraightFlush(cards))
            return HandType.StraightFlush;
        else if (IsFourOfAKind(cards))
            return HandType.FourOfAKind;
        else if (IsFullHouse(cards))
            return HandType.FullHouse;
        else if (IsFlush(cards))
            return HandType.Flush;
        else if (IsStraight(cards))
            return HandType.Straight;
        else if (IsThreeOfAKind(cards))
            return HandType.ThreeOfAKind;
        else if (IsTwoPair(cards))
            return HandType.TwoPair;
        else if (IsOnePair(cards))
            return HandType.OnePair;
        else if (IsAceHigh(cards))
            return HandType.AceHigh;
        else if (IsKingHigh(cards))
            return HandType.KingHigh;
        else if (IsQueenHigh(cards))
            return HandType.QueenHigh;
        else if (IsJackHigh(cards))
            return HandType.JackHigh;
        else if (IsTenHigh(cards))
            return HandType.TenHigh;
        else if (IsNineHigh(cards))
            return HandType.NineHigh;
        else if (IsEightHigh(cards))
            return HandType.EightHigh;
        else if (IsSevenHigh(cards))
            return HandType.SevenHigh;
        else if (IsSixHigh(cards))
            return HandType.SixHigh;
        else if (IsFiveHigh(cards))
            return HandType.FiveHigh;
        else if (IsFourHigh(cards))
            return HandType.FourHigh;
        else if (IsThreeHigh(cards))
            return HandType.ThreeHigh;
        else if (IsTwoHigh(cards))
            return HandType.TwoHigh;

        return HandType.None;
    }

    private bool IsAceHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Ace);
    }

    private bool IsKingHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.King);
    }

    private bool IsQueenHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Queen);
    }

    private bool IsJackHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Jack);
    }

    private bool IsTenHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Ten);
    }

    private bool IsNineHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Nine);
    }

    private bool IsEightHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Eight);
    }

    private bool IsSevenHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Seven);
    }

    private bool IsSixHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Six);
    }

    private bool IsFiveHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Five);
    }

    private bool IsFourHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Four);
    }

    private bool IsThreeHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Three);
    }

    private bool IsTwoHigh(List<Card> cards)
    {
        return
            cards.Any(x => x.Rank == CardRank.Two);
    }

    private bool IsFourOfAKind(List<Card> cards)
    {
        // four of a kind
        // four cards of the same rank

        return
            cards.GroupBy(x => x.Rank).Any(x => x.Count() == 4);
    }

    private bool IsFlush(List<Card> cards)
    {
        // flush
        // five cards of the same suit

        return
            cards.GroupBy(x => x.Suit).Any(x => x.Count() == 5);
    }

    private bool IsFullHouse(List<Card> cards)
    {
        // full house
        // three of a kind and one pair

        return
            IsThreeOfAKind(cards) && IsOnePair(cards);
    }

    private bool IsOnePair(List<Card> cards)
    {
        // one pair
        // two cards of the same rank

        return
            cards.GroupBy(x => x.Rank).Where(x => x.Count() == 2).Count() == 1;
    }

    private bool IsRoyalFlush(List<Card> cards)
    {
        // royal flush
        // ace, king, queen, jack, 10 of the same suit

        return
            cards.Select(x => x.Suit).Distinct().Count() == 1 &&
            cards.Any(x =>
                x.Rank == CardRank.Ace &&
                x.Rank == CardRank.King &&
                x.Rank == CardRank.Queen &&
                x.Rank == CardRank.Jack &&
                x.Rank == CardRank.Ten);
    }

    private bool IsStraight(List<Card> cards)
    {
        // straight
        // five consecutive cards

        var temp = cards.Where(x => x.Rank != CardRank.Ace)
                        .Select(x => (int)(x.Rank))
                        .ToList();

        var aceRank = temp.Contains(2) ? 1 : 14;

        foreach (var ace in cards.Where(x => x.Rank == CardRank.Ace))
            temp.Add(aceRank);

        temp = temp.OrderBy(x => x).ToList();

        var differences = new List<int>();

        for (var i = 0; i < temp.Count() - 1; i++)
            differences.Add(temp[i + 1] - temp[i]);

        return
            differences.All(x => x == 1);
    }

    private bool IsStraightFlush(List<Card> cards)
    {
        // straight flush
        // five consecutive cards of the same suit

        return
            IsFlush(cards) && IsStraight(cards);
    }

    private bool IsThreeOfAKind(List<Card> cards)
    {
        // three of a kind
        // three cards of the same rank

        return
            cards.GroupBy(x => x.Rank).Any(x => x.Count() == 3);
    }

    private bool IsTwoPair(List<Card> cards)
    {
        // two pair
        // two cards of the same rank

        return
            cards.GroupBy(x => x.Rank).Where(x => x.Count() == 2).Count() == 2;
    }
}