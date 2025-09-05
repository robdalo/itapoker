using FluentAssertions;
using itapoker.Core.Models;
using itapoker.Core.Models.Enums;
using itapoker.Core.Repositories;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core.Tests.Repositories;

public class GameRepoTests
{
    private IGameRepo _gameRepo;

    [SetUp]
    public void Setup()
    {
        _gameRepo = new GameRepo();
    }

    [Test]
    public void AddOrUpdate()
    {
        var game = _gameRepo.AddOrUpdate(new Game {
            GameId = Guid.NewGuid().ToString(),
            Ante = 10,
            Limit = 100,
            Cash = 500,
            Stage = GameStage.NewGame,
            Players = new() {
                new() { Name = "Player 1" },
                new() { Name = "Player 2" }
            }
        });

        _gameRepo.Get(game.Id).Should().BeEquivalentTo(game);
    }
}
