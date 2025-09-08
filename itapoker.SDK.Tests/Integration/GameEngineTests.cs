using FluentAssertions;
using itapoker.SDK.Consumers;
using itapoker.SDK.Enums;

namespace itapoker.SDK.Tests.Integration;

public class GameEngineTests
{
    private ApiConsumer _apiConsumer;

    private const string BaseUrl = "http://localhost:5174";

    [SetUp]
    public void Setup()
    {
        _apiConsumer = new ApiConsumer(BaseUrl);
    }

    [Test]
    public async Task SinglePlayerAsync()
    {
        // create new single player game

        var playerName = "itaboi";
        var ante = 50;
        var cash = 1000;
        var limit = 100;

        var response = await _apiConsumer.SinglePlayerAsync(new()
        {
            PlayerName = playerName,
            Ante = ante,
            Cash = cash,
            Limit = limit
        });

        // verify new single player game

        response.Ante.Should().Be(ante);
        response.Cash.Should().Be(cash);
        response.Limit.Should().Be(limit);
        response.Players.First(x => x.PlayerType == PlayerType.Human).Name.Should().Be(playerName);
        response.Players.First(x => x.PlayerType == PlayerType.Computer).Name.Should().Be("AI Player");
        
        
    }
}