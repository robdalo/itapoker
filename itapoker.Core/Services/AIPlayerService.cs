using itapoker.Core.Domain.Enums;
using itapoker.Core.Interfaces;
using itapoker.Core.Repositories.Interfaces;

namespace itapoker.Core.Services;

public class AIPlayerService : IAIPlayerService
{
    private readonly IGameRepo _gameRepo;

    public AIPlayerService(IGameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public void AnteUp(string gameId)
    {
        var game = _gameRepo.GetByGameId(gameId);
        var player = game.Players.First(x => x.PlayerType == PlayerType.Computer);

        player.Cash -= game.Ante;
        game.Pot += game.Ante;

        _gameRepo.AddOrUpdate(game);
    }

    public void Bet(string gameId, BetType betType, int amount)
    {
        if (betType == BetType.Fold)
            return;

        var game = _gameRepo.GetByGameId(gameId);
        var player = game.Players.First(x => x.PlayerType == PlayerType.Human);
        var aiPlayer = game.Players.First(x => x.PlayerType == PlayerType.Computer);

        var random = new Random();

        var aiBetType = (BetType)(random.Next(1, 4));

        if (aiBetType == BetType.Fold)
        {
            player.Cash += game.Pot;
            game.Pot = 0;
            game.Stage = GameStage.GameOver;
        }
        else if (aiBetType == BetType.Check)
        {
            game.Stage = GameStage.Draw;
        }
        else if (aiBetType == BetType.Call)
        {
            aiPlayer.Cash -= amount;
            game.Pot += amount;
            game.Stage = GameStage.Draw;
        }
        else if (aiBetType == BetType.Raise)
        {
            aiPlayer.Cash -= amount * 2;
            game.Pot += amount * 2;
        }
    }
}