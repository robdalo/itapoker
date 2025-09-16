using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Domain.Models;

public class Deck
{
    public static List<Card> Cards => Clubs
        .Concat(Diamonds)
        .Concat(Hearts)
        .Concat(Spades)
        .ToList();

    public static Card AceClubs => new(CardSuit.Clubs, CardRank.Ace);
    public static Card TwoClubs => new(CardSuit.Clubs, CardRank.Two);
    public static Card ThreeClubs => new(CardSuit.Clubs, CardRank.Three);
    public static Card FourClubs => new(CardSuit.Clubs, CardRank.Four);
    public static Card FiveClubs => new(CardSuit.Clubs, CardRank.Five);
    public static Card SixClubs => new(CardSuit.Clubs, CardRank.Six);
    public static Card SevenClubs => new(CardSuit.Clubs, CardRank.Seven);
    public static Card EightClubs => new(CardSuit.Clubs, CardRank.Eight);
    public static Card NineClubs => new(CardSuit.Clubs, CardRank.Nine);
    public static Card TenClubs => new(CardSuit.Clubs, CardRank.Ten);
    public static Card JackClubs => new(CardSuit.Clubs, CardRank.Jack);
    public static Card QueenClubs => new(CardSuit.Clubs, CardRank.Queen);
    public static Card KingClubs => new(CardSuit.Clubs, CardRank.King);

    public static Card AceDiamonds => new(CardSuit.Diamonds, CardRank.Ace);
    public static Card TwoDiamonds => new(CardSuit.Diamonds, CardRank.Two);
    public static Card ThreeDiamonds => new(CardSuit.Diamonds, CardRank.Three);
    public static Card FourDiamonds => new(CardSuit.Diamonds, CardRank.Four);
    public static Card FiveDiamonds => new(CardSuit.Diamonds, CardRank.Five);
    public static Card SixDiamonds => new(CardSuit.Diamonds, CardRank.Six);
    public static Card SevenDiamonds => new(CardSuit.Diamonds, CardRank.Seven);
    public static Card EightDiamonds => new(CardSuit.Diamonds, CardRank.Eight);
    public static Card NineDiamonds => new(CardSuit.Diamonds, CardRank.Nine);
    public static Card TenDiamonds => new(CardSuit.Diamonds, CardRank.Ten);
    public static Card JackDiamonds => new(CardSuit.Diamonds, CardRank.Jack);
    public static Card QueenDiamonds => new(CardSuit.Diamonds, CardRank.Queen);
    public static Card KingDiamonds => new(CardSuit.Diamonds, CardRank.King);

    public static Card AceHearts => new(CardSuit.Hearts, CardRank.Ace);
    public static Card TwoHearts => new(CardSuit.Hearts, CardRank.Two);
    public static Card ThreeHearts => new(CardSuit.Hearts, CardRank.Three);
    public static Card FourHearts => new(CardSuit.Hearts, CardRank.Four);
    public static Card FiveHearts => new(CardSuit.Hearts, CardRank.Five);
    public static Card SixHearts => new(CardSuit.Hearts, CardRank.Six);
    public static Card SevenHearts => new(CardSuit.Hearts, CardRank.Seven);
    public static Card EightHearts => new(CardSuit.Hearts, CardRank.Eight);
    public static Card NineHearts => new(CardSuit.Hearts, CardRank.Nine);
    public static Card TenHearts => new(CardSuit.Hearts, CardRank.Ten);
    public static Card JackHearts => new(CardSuit.Hearts, CardRank.Jack);
    public static Card QueenHearts => new(CardSuit.Hearts, CardRank.Queen);
    public static Card KingHearts => new(CardSuit.Hearts, CardRank.King);

    public static Card AceSpades => new(CardSuit.Spades, CardRank.Ace);
    public static Card TwoSpades => new(CardSuit.Spades, CardRank.Two);
    public static Card ThreeSpades => new(CardSuit.Spades, CardRank.Three);
    public static Card FourSpades => new(CardSuit.Spades, CardRank.Four);
    public static Card FiveSpades => new(CardSuit.Spades, CardRank.Five);
    public static Card SixSpades => new(CardSuit.Spades, CardRank.Six);
    public static Card SevenSpades => new(CardSuit.Spades, CardRank.Seven);
    public static Card EightSpades => new(CardSuit.Spades, CardRank.Eight);
    public static Card NineSpades => new(CardSuit.Spades, CardRank.Nine);
    public static Card TenSpades => new(CardSuit.Spades, CardRank.Ten);
    public static Card JackSpades => new(CardSuit.Spades, CardRank.Jack);
    public static Card QueenSpades => new(CardSuit.Spades, CardRank.Queen);
    public static Card KingSpades => new(CardSuit.Spades, CardRank.King);         

    private static List<Card> Clubs => new() {
        Deck.AceClubs,
        Deck.TwoClubs,
        Deck.ThreeClubs,
        Deck.FourClubs,
        Deck.FiveClubs,
        Deck.SixClubs,
        Deck.SevenClubs,
        Deck.EightClubs,
        Deck.NineClubs,
        Deck.TenClubs,
        Deck.JackClubs,
        Deck.QueenClubs,
        Deck.KingClubs
    };

    private static List<Card> Diamonds => new() {
        Deck.AceDiamonds,
        Deck.TwoDiamonds,
        Deck.ThreeDiamonds,
        Deck.FourDiamonds,
        Deck.FiveDiamonds,
        Deck.SixDiamonds,
        Deck.SevenDiamonds,
        Deck.EightDiamonds,
        Deck.NineDiamonds,
        Deck.TenDiamonds,
        Deck.JackDiamonds,
        Deck.QueenDiamonds,
        Deck.KingDiamonds
    };

    private static List<Card> Hearts => new() {
        Deck.AceHearts,
        Deck.TwoHearts,
        Deck.ThreeHearts,
        Deck.FourHearts,
        Deck.FiveHearts,
        Deck.SixHearts,
        Deck.SevenHearts,
        Deck.EightHearts,
        Deck.NineHearts,
        Deck.TenHearts,
        Deck.JackHearts,
        Deck.QueenHearts,
        Deck.KingHearts
    };    

    private static List<Card> Spades => new() {
        Deck.AceSpades,
        Deck.TwoSpades,
        Deck.ThreeSpades,
        Deck.FourSpades,
        Deck.FiveSpades,
        Deck.SixSpades,
        Deck.SevenSpades,
        Deck.EightSpades,
        Deck.NineSpades,
        Deck.TenSpades,
        Deck.JackSpades,
        Deck.QueenSpades,
        Deck.KingSpades
    };
}