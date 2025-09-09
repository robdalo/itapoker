using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Domain.Models;

public class Deck
{
    public static List<Card> Cards => Clubs
        .Concat(Diamonds)
        .Concat(Hearts)
        .Concat(Spades)
        .ToList();

    private static List<Card> Clubs => new() {
        new(CardSuit.Clubs, CardRank.Ace),
        new(CardSuit.Clubs, CardRank.Two),
        new(CardSuit.Clubs, CardRank.Three),
        new(CardSuit.Clubs, CardRank.Four),
        new(CardSuit.Clubs, CardRank.Five),
        new(CardSuit.Clubs, CardRank.Six),
        new(CardSuit.Clubs, CardRank.Seven),
        new(CardSuit.Clubs, CardRank.Eight),
        new(CardSuit.Clubs, CardRank.Nine),
        new(CardSuit.Clubs, CardRank.Ten),
        new(CardSuit.Clubs, CardRank.Jack),
        new(CardSuit.Clubs, CardRank.Queen),
        new(CardSuit.Clubs, CardRank.King)
    };

    private static List<Card> Diamonds => new() {
        new(CardSuit.Diamonds, CardRank.Ace),
        new(CardSuit.Diamonds, CardRank.Two),
        new(CardSuit.Diamonds, CardRank.Three),
        new(CardSuit.Diamonds, CardRank.Four),
        new(CardSuit.Diamonds, CardRank.Five),
        new(CardSuit.Diamonds, CardRank.Six),
        new(CardSuit.Diamonds, CardRank.Seven),
        new(CardSuit.Diamonds, CardRank.Eight),
        new(CardSuit.Diamonds, CardRank.Nine),
        new(CardSuit.Diamonds, CardRank.Ten),
        new(CardSuit.Diamonds, CardRank.Jack),
        new(CardSuit.Diamonds, CardRank.Queen),
        new(CardSuit.Diamonds, CardRank.King)
    };

    private static List<Card> Hearts => new() {
        new(CardSuit.Hearts, CardRank.Ace),
        new(CardSuit.Hearts, CardRank.Two),
        new(CardSuit.Hearts, CardRank.Three),
        new(CardSuit.Hearts, CardRank.Four),
        new(CardSuit.Hearts, CardRank.Five),
        new(CardSuit.Hearts, CardRank.Six),
        new(CardSuit.Hearts, CardRank.Seven),
        new(CardSuit.Hearts, CardRank.Eight),
        new(CardSuit.Hearts, CardRank.Nine),
        new(CardSuit.Hearts, CardRank.Ten),
        new(CardSuit.Hearts, CardRank.Jack),
        new(CardSuit.Hearts, CardRank.Queen),
        new(CardSuit.Hearts, CardRank.King)
    };

    private static List<Card> Spades => new() {
        new(CardSuit.Spades, CardRank.Ace),
        new(CardSuit.Spades, CardRank.Two),
        new(CardSuit.Spades, CardRank.Three),
        new(CardSuit.Spades, CardRank.Four),
        new(CardSuit.Spades, CardRank.Five),
        new(CardSuit.Spades, CardRank.Six),
        new(CardSuit.Spades, CardRank.Seven),
        new(CardSuit.Spades, CardRank.Eight),
        new(CardSuit.Spades, CardRank.Nine),
        new(CardSuit.Spades, CardRank.Ten),
        new(CardSuit.Spades, CardRank.Jack),
        new(CardSuit.Spades, CardRank.Queen),
        new(CardSuit.Spades, CardRank.King)
    };
}
