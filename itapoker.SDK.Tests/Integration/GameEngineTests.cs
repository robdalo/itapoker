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
        var gameId = await VerifySinglePlayerAsync();
        
        await VerifyAnteUpAsync(gameId);
        await VerifyDealAsync(gameId);
    }

    private async Task VerifyAnteUpAsync(string gameId)
    {
        // ante up

        var response = await _apiConsumer.AnteUpAsync(new() { GameId = gameId });

        response.Pot.Should().Be(100);
    }

    private async Task VerifyDealAsync(string gameId)
    {
        // deal

        var response = await _apiConsumer.DealAsync(new() {
            GameId = gameId
        });

        response.Cards.Should().HaveCount(5);
    }

    private async Task<string> VerifySinglePlayerAsync()
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

        return response.GameId;
    }
}