using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Requests;
using itapoker.Core.Repositories.Interfaces;
using itapoker.Core.Services.Interfaces;

namespace itapoker.Core.Services;

public class ValidationService : IValidationService
{
    private readonly IGameRepo _gameRepo;

    public ValidationService(IGameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public void AddChip(AddChipRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        if (game == null)
            throw new InvalidOperationException($"Game with gameId {request.GameId} not found");

        if (game.Stage != GameStage.BetPreDraw && game.Stage != GameStage.BetPostDraw)
            throw new InvalidOperationException($"Add chip request during game stage {game.Stage} is not allowed");

        if (request.Value > game.Player.Cash)
            throw new InvalidOperationException($"Bet amount ${request.Value} exceeds cash available {game.Player.Cash}");

        var total = game.Player.Chips.Sum(x => x.Total) + request.Value;

        if (total > game.Limit)
            throw new InvalidOperationException($"Bet total ${total} exceeds bet limit {game.Limit}");
    }

    public void AnteUp(AnteUpRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        if (game == null)
            throw new InvalidOperationException($"Game with gameId {request.GameId} not found");

        if (game.Stage != GameStage.Ante)
            throw new InvalidOperationException($"Ante up request during game stage {game.Stage} is not allowed");
    }

    public void Bet(BetRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        if (game == null)
            throw new InvalidOperationException($"Game with gameId {request.GameId} not found");

        if (game.Stage != GameStage.BetPreDraw && game.Stage != GameStage.BetPostDraw)
            throw new InvalidOperationException($"Bet request during game stage {game.Stage} is not allowed");
    }

    public void Deal(DealRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        if (game == null)
            throw new InvalidOperationException($"Game with gameId {request.GameId} not found");

        if (game.Stage != GameStage.Deal)
            throw new InvalidOperationException($"Deal request during game stage {game.Stage} is not allowed");
    }

    public void Draw(DrawRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        if (game == null)
            throw new InvalidOperationException($"Game with gameId {request.GameId} not found");

        if (game.Stage != GameStage.Draw)
            throw new InvalidOperationException($"Draw request during game stage {game.Stage} is not allowed");
    }

    public void Hold(HoldRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        if (game == null)
            throw new InvalidOperationException($"Game with gameId {request.GameId} not found");

        if (game.Stage != GameStage.Draw)
            throw new InvalidOperationException($"Hold request during game stage {game.Stage} is not allowed");
    }

    public void Next(NextRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        if (game == null)
            throw new InvalidOperationException($"Game with gameId {request.GameId} not found");

        if (game.Stage != GameStage.GameOver)
            throw new InvalidOperationException($"Next request during game stage {game.Stage} is not allowed");
    }

    public void RemoveChip(RemoveChipRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        if (game == null)
            throw new InvalidOperationException($"Game with gameId {request.GameId} not found");

        if (game.Stage != GameStage.BetPreDraw && game.Stage != GameStage.BetPostDraw)
            throw new InvalidOperationException($"Remove chip request during game stage {game.Stage} is not allowed");
    }

    public void Showdown(ShowdownRequest request)
    {
        var game = _gameRepo.GetByGameId(request.GameId);

        if (game == null)
            throw new InvalidOperationException($"Game with gameId {request.GameId} not found");

        if (game.Stage != GameStage.Showdown)
            throw new InvalidOperationException($"Showdown request during game stage {game.Stage} is not allowed");
    }

    public void SinglePlayer(SinglePlayerRequest request)
    {
        if (string.IsNullOrEmpty(request.PlayerName) || request.PlayerName.Length > 20)
            throw new InvalidOperationException($"Player name cannot be empty or more than 20 characters");       
    }
}