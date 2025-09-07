using FluentAssertions;
using itapoker.Core.Interfaces;
using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace itapoker.Core.Tests.Integration;

public class GameEngineTests : TestBase
{
    private IGameEngine _gameEngine;

    [SetUp]
    public void Setup()
    {
        var provider = GetServiceProvider();
        _gameEngine = provider.GetRequiredService<IGameEngine>();
    }

    [Test]
    public void NewGame()
    {
        var request = new NewGameRequest
        {
            PlayerName = "Player A",
            Ante = 10,
            Cash = 500,
            Limit = 100
        };

        var response = _gameEngine.NewGame(request);

        response.Ante.Should().Be(request.Ante);
        response.Cash.Should().Be(request.Cash);
        response.Limit.Should().Be(request.Limit);
        response.Players.First(x => x.PlayerType == PlayerType.Human).Name.Should().Be(request.PlayerName);
        response.Players.First(x => x.PlayerType == PlayerType.Computer).Name.Should().Be("AI Player");
    }
    
    [Test]
    public void UpdateHighScores()
    {
        _gameEngine.UpdateHighScores();
    }
}