using AutoFixture;
using AutoFixture.AutoMoq;
using itapoker.Core.Domain.Models;
using itapoker.Core.Services;
using Moq;

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

    [Test]
    public void GetChips()
    {
        _betService.GetChips(100);
    }
}