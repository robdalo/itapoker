using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Domain.Models;

public class Card
{
    public CardSuit Suit { get; set; }
    public CardRank Rank { get; set; }

    public Card()
    {
    }

    public Card(CardSuit suit, CardRank rank)
    {
        this.Suit = suit;
        this.Rank = rank;
    }
}