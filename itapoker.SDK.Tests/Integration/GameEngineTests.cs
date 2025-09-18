using FluentAssertions;
using itapoker.SDK.Consumers;
using itapoker.SDK.Enums;
using itapoker.SDK.Models;

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
        var game = await VerifySinglePlayerAsync();

        game = await VerifyAnteUpAsync(game);
        game = await VerifyDealAsync(game);
        game = await VerifyBetPreDrawAsync(game);
        game = await VerifyDrawAsync(game);
        game = await VerifyBetPostDrawAsync(game);
        game = await VerifyShowdownAsync(game);
        game = await VerifyNextAsync(game);
    }

    private async Task<Game> VerifyAnteUpAsync(Game game)
    {
        // ante up

        game = await _apiConsumer.AnteUpAsync(new() { GameId = game.GameId });

        game.Stage.Should().Be(GameStage.Deal);
        game.Pot.Should().Be(100);

        return game;
    }

    private async Task<Game> VerifyBetPreDrawAsync(Game game)
    {
        // configure the AI player bet

        await _apiConsumer.SetDecisionAsync(new() {
            GameId = game.GameId,
            BetType = BetType.Raise,
            Amount = 20
        });

        // bet - check, AI player will raise by 20

        game = await _apiConsumer.BetAsync(new() {
            GameId = game.GameId,
            BetType = BetType.Check
        });

        // configure the AI player bet
        
        await _apiConsumer.SetDecisionAsync(new() {
            GameId = game.GameId,
            BetType = BetType.Raise,
            Amount = 40
        });

        // bet - raise by 20, ai player will raise by 40

        game = await _apiConsumer.BetAsync(new() {
            GameId = game.GameId,
            BetType = BetType.Raise,
            Amount = 20
        });

        // bet - call

         game = await _apiConsumer.BetAsync(new() {
            GameId = game.GameId,
            BetType = BetType.Call
        });

        game.Stage.Should().Be(GameStage.Draw);
        game.Pot.Should().Be(260);

        return game;
    }

    private async Task<Game> VerifyBetPostDrawAsync(Game game)
    {
        // configure the AI player bet
        
        await _apiConsumer.SetDecisionAsync(new() {
            GameId = game.GameId,
            BetType = BetType.Check
        });

        // bet - check, ai player will also check

        game = await _apiConsumer.BetAsync(new() {
            GameId = game.GameId,
            BetType = BetType.Check
        });

        game.Stage.Should().Be(GameStage.Showdown);
        game.Pot.Should().Be(260);

        return game;
    }

    private async Task<Game> VerifyDealAsync(Game game)
    {
        // deal

        game = await _apiConsumer.DealAsync(new()
        {
            GameId = game.GameId
        });

        game.Stage.Should().Be(GameStage.BetPreDraw);

        var player = game.Players.First(x => x.PlayerType == PlayerType.Human);

        player.Cards.Should().HaveCount(5);

        return game;
    }

    private async Task<Game> VerifyDrawAsync(Game game)
    {
        // draw

        game = await _apiConsumer.DrawAsync(new()
        {
            GameId = game.GameId,
            Cards = game.Player.Cards.Take(3).ToList()
        });

        game.Stage.Should().Be(GameStage.BetPostDraw);

        game.Player.Cards.Should().HaveCount(5);

        return game;
    }
    
    private async Task<Game> VerifyNextAsync(Game game)
    {
        // showdown

        game = await _apiConsumer.NextAsync(new() {
            GameId = game.GameId
        });

        game.Stage.Should().Be(GameStage.Ante);
        game.Pot.Should().Be(0);
        game.Hand.Should().Be(2);

        return game;
    }
    
    private async Task<Game> VerifyShowdownAsync(Game game)
    {
        // showdown

        game = await _apiConsumer.ShowdownAsync(new()
        {
            GameId = game.GameId
        });

        game.Stage.Should().Be(GameStage.GameOver);
        game.Pot.Should().Be(0);

        return game;
    }

    private async Task<Game> VerifySinglePlayerAsync()
    {
        // create new single player game

        var playerName = "itaboi";
        var ante = 50;
        var cash = 1000;
        var limit = 100;

        var game = await _apiConsumer.SinglePlayerAsync(new()
        {
            PlayerName = playerName,
            Ante = ante,
            Cash = cash,
            Limit = limit
        });

        // verify new single player game

        game.Ante.Should().Be(ante);
        game.Cash.Should().Be(cash);
        game.Limit.Should().Be(limit);
        game.Players.First(x => x.PlayerType == PlayerType.Human).Name.Should().Be(playerName);
        game.Players.First(x => x.PlayerType == PlayerType.Computer).Name.Should().Be("AI Player");

        return game;
    }
}