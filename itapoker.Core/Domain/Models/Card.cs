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

    public static Card Get(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.AceClubs: return Deck.AceClubs;
            case CardType.TwoClubs: return Deck.TwoClubs;
            case CardType.ThreeClubs: return Deck.ThreeClubs;
            case CardType.FourClubs: return Deck.FourClubs;
            case CardType.FiveClubs: return Deck.FiveClubs;
            case CardType.SixClubs: return Deck.SixClubs;
            case CardType.SevenClubs: return Deck.SevenClubs;
            case CardType.EightClubs: return Deck.EightClubs;
            case CardType.NineClubs: return Deck.NineClubs;
            case CardType.TenClubs: return Deck.TenClubs;
            case CardType.JackClubs: return Deck.JackClubs;
            case CardType.QueenClubs: return Deck.QueenClubs;
            case CardType.KingClubs: return Deck.KingClubs;

            case CardType.AceDiamonds: return Deck.AceDiamonds;
            case CardType.TwoDiamonds: return Deck.TwoDiamonds;
            case CardType.ThreeDiamonds: return Deck.ThreeDiamonds;
            case CardType.FourDiamonds: return Deck.FourDiamonds;
            case CardType.FiveDiamonds: return Deck.FiveDiamonds;
            case CardType.SixDiamonds: return Deck.SixDiamonds;
            case CardType.SevenDiamonds: return Deck.SevenDiamonds;
            case CardType.EightDiamonds: return Deck.EightDiamonds;
            case CardType.NineDiamonds: return Deck.NineDiamonds;
            case CardType.TenDiamonds: return Deck.TenDiamonds;
            case CardType.JackDiamonds: return Deck.JackDiamonds;
            case CardType.QueenDiamonds: return Deck.QueenDiamonds;
            case CardType.KingDiamonds: return Deck.KingDiamonds;

            case CardType.AceHearts: return Deck.AceHearts;
            case CardType.TwoHearts: return Deck.TwoHearts;
            case CardType.ThreeHearts: return Deck.ThreeHearts;
            case CardType.FourHearts: return Deck.FourHearts;
            case CardType.FiveHearts: return Deck.FiveHearts;
            case CardType.SixHearts: return Deck.SixHearts;
            case CardType.SevenHearts: return Deck.SevenHearts;
            case CardType.EightHearts: return Deck.EightHearts;
            case CardType.NineHearts: return Deck.NineHearts;
            case CardType.TenHearts: return Deck.TenHearts;
            case CardType.JackHearts: return Deck.JackHearts;
            case CardType.QueenHearts: return Deck.QueenHearts;
            case CardType.KingHearts: return Deck.KingHearts;

            case CardType.AceSpades: return Deck.AceSpades;
            case CardType.TwoSpades: return Deck.TwoSpades;
            case CardType.ThreeSpades: return Deck.ThreeSpades;
            case CardType.FourSpades: return Deck.FourSpades;
            case CardType.FiveSpades: return Deck.FiveSpades;
            case CardType.SixSpades: return Deck.SixSpades;
            case CardType.SevenSpades: return Deck.SevenSpades;
            case CardType.EightSpades: return Deck.EightSpades;
            case CardType.NineSpades: return Deck.NineSpades;
            case CardType.TenSpades: return Deck.TenSpades;
            case CardType.JackSpades: return Deck.JackSpades;
            case CardType.QueenSpades: return Deck.QueenSpades;
            case CardType.KingSpades: return Deck.KingSpades;                     

            default: return null;
        }
    }
}