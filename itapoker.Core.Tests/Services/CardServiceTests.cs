using AutoFixture;
using FluentAssertions;
using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Services;
using itapoker.Core.Services.Interfaces;

namespace itapoker.Core.Tests.Services;

public class CardServiceTests
{
    private IFixture _autoFixture;

    private ICardService _cardService;

    [SetUp]
    public void Setup()
    {
        _autoFixture = new Fixture();

        _cardService = _autoFixture.Create<CardService>();
    }

    [TestCase(CardType.SevenSpades, CardType.TwoHearts, CardType.ThreeClubs, CardType.FourClubs, CardType.FiveDiamonds, HandType.SevenHigh)] // seven high
    [TestCase(CardType.EightHearts, CardType.FourHearts, CardType.SixDiamonds, CardType.SevenSpades, CardType.TwoDiamonds, HandType.EightHigh)] // eight high
    [TestCase(CardType.NineDiamonds, CardType.FourHearts, CardType.SixDiamonds, CardType.SevenSpades, CardType.EightDiamonds, HandType.NineHigh)] // nine high
    [TestCase(CardType.TenClubs, CardType.FourHearts, CardType.SixDiamonds, CardType.NineSpades, CardType.ThreeDiamonds, HandType.TenHigh)] // ten high
    [TestCase(CardType.JackHearts, CardType.FourHearts, CardType.SixDiamonds, CardType.NineSpades, CardType.TenDiamonds, HandType.JackHigh)] // jack high
    [TestCase(CardType.QueenSpades, CardType.FourHearts, CardType.SixDiamonds, CardType.JackSpades, CardType.TenDiamonds, HandType.QueenHigh)] // queen high
    [TestCase(CardType.KingDiamonds, CardType.FourHearts, CardType.SixDiamonds, CardType.JackSpades, CardType.QueenDiamonds, HandType.KingHigh)] // king high
    [TestCase(CardType.AceClubs, CardType.FourHearts, CardType.SixDiamonds, CardType.JackSpades, CardType.KingDiamonds, HandType.AceHigh)] // ace high
    [TestCase(CardType.FourSpades, CardType.FourHearts, CardType.SixDiamonds, CardType.JackSpades, CardType.KingDiamonds, HandType.OnePair)] // one pair
    [TestCase(CardType.FourSpades, CardType.FourHearts, CardType.SixDiamonds, CardType.SixClubs, CardType.KingDiamonds, HandType.TwoPair)] // two pair
    [TestCase(CardType.JackClubs, CardType.JackDiamonds, CardType.JackHearts, CardType.SixClubs, CardType.KingDiamonds, HandType.ThreeOfAKind)] // three of a kind
    [TestCase(CardType.TwoClubs, CardType.ThreeDiamonds, CardType.FourHearts, CardType.FiveHearts, CardType.SixHearts, HandType.Straight)] // straight
    [TestCase(CardType.AceClubs, CardType.TwoDiamonds, CardType.ThreeHearts, CardType.FourHearts, CardType.FiveSpades, HandType.Straight)] // straight - ace low
    [TestCase(CardType.TenHearts, CardType.JackClubs, CardType.QueenDiamonds, CardType.KingDiamonds, CardType.AceSpades, HandType.Straight)] // straight - ace high
    [TestCase(CardType.ThreeDiamonds, CardType.FiveDiamonds, CardType.SixDiamonds, CardType.JackDiamonds, CardType.AceDiamonds, HandType.Flush)] // flush
    [TestCase(CardType.FiveSpades, CardType.FiveDiamonds, CardType.FiveClubs, CardType.JackHearts, CardType.JackClubs, HandType.FullHouse)] // full house
    [TestCase(CardType.NineClubs, CardType.NineDiamonds, CardType.NineHearts, CardType.NineSpades, CardType.KingDiamonds, HandType.FourOfAKind)] // four of a kind
    [TestCase(CardType.FourSpades, CardType.FiveSpades, CardType.SixSpades, CardType.SevenSpades, CardType.EightSpades, HandType.StraightFlush)] // straight flush
    [TestCase(CardType.AceDiamonds, CardType.TwoDiamonds, CardType.ThreeDiamonds, CardType.FourDiamonds, CardType.FiveDiamonds, HandType.StraightFlush)] // straight flush - ace low
    [TestCase(CardType.TenHearts, CardType.JackHearts, CardType.QueenHearts, CardType.KingHearts, CardType.AceHearts, HandType.StraightFlush)] // royal flush
    public void GetHandType(
        CardType c1,
        CardType c2,
        CardType c3,
        CardType c4,
        CardType c5,
        HandType expected)
    {
        _cardService
            .GetHandType(GetHand(c1, c2, c3, c4, c5))
            .Should().Be(expected);
    }

    private List<Card> GetHand(
        CardType c1,
        CardType c2,
        CardType c3,
        CardType c4,
        CardType c5)
    {
        return new() {
            Card.Get(c1),
            Card.Get(c2),
            Card.Get(c3),
            Card.Get(c4),
            Card.Get(c5)
        };
    }
}