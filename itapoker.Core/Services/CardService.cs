using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Services.Interfaces;

namespace itapoker.Core.Services;

public class CardService : ICardService
{
    public CardService()
    {
    }

    public HandType GetHandType(List<Card> cards)
    {
        if (IsRoyalFlush(cards))
            return HandType.RoyalFlush;
        else if (IsStraightFlush(cards))
            return HandType.StraightFlush;
        else if (IsFourOfAKind(cards))
            return HandType.FourOfAKind;

        return HandType.None;
    }

    private bool IsFourOfAKind(List<Card> cards)
    {
        // four of a kind
        // four cards of the same rank

        return
            cards.GroupBy(x => x.Rank).Any(x => x.Count() == 4);
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

    private bool IsStraightFlush(List<Card> cards)
    {
        // straight flush
        // five consecutive cards of the same suit

        return
            cards.Select(x => x.Suit).Distinct().Count() == 1;
    }
}