using System.Security.Cryptography.X509Certificates;
using itapoker.Core.Domain.Enums;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core.Services;

public class PlayerService : IPlayerService
{
    private readonly IGameRepo _gameRepo;

    public PlayerService(IGameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public void AnteUp(string gameId)
    {
        var game = _gameRepo.GetByGameId(gameId);
        var player = game.Players.First(x => x.PlayerType == PlayerType.Human);

        player.Cash -= game.Ante;
        game.Pot += game.Ante;

        _gameRepo.AddOrUpdate(game);
    }

    public void Bet(string gameId, BetType betType, int amount)
    {
        var game = _gameRepo.GetByGameId(gameId);
        var player = game.Players.First(x => x.PlayerType == PlayerType.Human);
        var aiPlayer = game.Players.First(x => x.PlayerType == PlayerType.Computer);

        if (betType == BetType.Fold)
        {
            aiPlayer.Cash += game.Pot;
            game.Pot = 0;
            game.Stage = GameStage.GameOver;
        }
        else if (betType == BetType.Check)
        {
            player.LastBetAmount = 0;
        }
        else if (betType == BetType.Call)
        {
            player.Cash -= aiPlayer.LastBetAmount;
            player.LastBetAmount = aiPlayer.LastBetAmount;
            game.Pot += aiPlayer.LastBetAmount;
            game.Stage = GameStage.Draw;
        }
        else if (betType == BetType.Raise)
        {
            player.Cash -= amount;
            player.LastBetAmount = amount;
            game.Pot += amount;
        }

        player.LastBetType = betType;

        _gameRepo.AddOrUpdate(game);
    }
}