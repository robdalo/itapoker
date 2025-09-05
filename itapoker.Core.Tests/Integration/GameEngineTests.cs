using FluentAssertions;
using itapoker.Core.Interfaces;
using itapoker.Core.Models.Enums;
using itapoker.Core.Models.Requests;
using itapoker.Core.Repositories;

namespace itapoker.Core.Tests.Integration;

public class GameEngineTests
{
    private IGameEngine _gameEngine;

    [SetUp]
    public void Setup()
    {
        _gameEngine = new GameEngine(new GameRepo());
    }

    [Test]
    public void NewGame()
    {
        var request = new NewGameRequest {
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
}
