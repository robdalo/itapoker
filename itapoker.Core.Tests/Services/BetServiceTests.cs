using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using itapoker.Core.Services;

namespace itapoker.Core.Tests.Services;

public class BetServiceTests
{
    private IFixture _autoFixture;

    private BetService _betService;

    [SetUp]
    public void Setup()
    {
        _autoFixture = new Fixture();
        _autoFixture.Customize(new AutoMoqCustomization());

        _betService = _autoFixture.Create<BetService>();
    }

    [TestCase(0, 0, 0, 0, 0)]
    [TestCase(5, 1, 0, 0, 0)]
    [TestCase(10, 0, 1, 0, 0)]
    [TestCase(15, 1, 1, 0, 0)]
    [TestCase(20, 0, 2, 0, 0)]
    [TestCase(25, 0, 0, 1, 0)]
    [TestCase(30, 1, 0, 1, 0)]
    [TestCase(35, 0, 1, 1, 0)]
    [TestCase(40, 1, 1, 1, 0)]
    [TestCase(45, 0, 2, 1, 0)]
    [TestCase(50, 0, 0, 0, 1)]
    [TestCase(55, 1, 0, 0, 1)]
    [TestCase(60, 0, 1, 0, 1)]
    [TestCase(65, 1, 1, 0, 1)]
    [TestCase(70, 0, 2, 0, 1)]
    [TestCase(75, 0, 0, 1, 1)]
    [TestCase(80, 1, 0, 1, 1)]
    [TestCase(85, 0, 1, 1, 1)]
    [TestCase(90, 1, 1, 1, 1)]
    [TestCase(95, 0, 2, 1, 1)]
    [TestCase(100, 0, 0, 0, 2)]
    public void GetChips(
        int amount,
        int exp5,
        int exp10,
        int exp25,
        int exp50)
    {
        var chips = _betService.GetChips(amount);

        chips.Where(x => x.Value == 5).Sum(x => x.Quantity).Should().Be(exp5);
        chips.Where(x => x.Value == 10).Sum(x => x.Quantity).Should().Be(exp10);
        chips.Where(x => x.Value == 25).Sum(x => x.Quantity).Should().Be(exp25);
        chips.Where(x => x.Value == 50).Sum(x => x.Quantity).Should().Be(exp50);
    }
}