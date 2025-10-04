using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Services.Interfaces;
using itapoker.Shared.Consumers;
using Microsoft.Extensions.Options;

namespace itapoker.Core.Services;

public class BetService : IBetService
{
    private readonly IDecisionService _decisionService;
    private readonly GameSettings _settings;

    public BetService(IDecisionService decisionService, IOptions<GameSettings> settings)
    {
        _decisionService = decisionService;
        _settings = settings.Value;
    }

    public Game ProcessAIPlayerBet(Game game)
    {
        // we use the decision service to compute which type of bet
        // the ai player would like to place

        var (betType, amount) = GetBet(game);

        if (betType == BetType.Fold)
        {
            // if the ai player is folding, we should end the game
            // immediately and pay out the player

            game.AIPlayer.LastBetAmount = 0;
            game.AIPlayer.LastBetType = BetType.Fold;
            game.AIPlayer.Winnings = game.AIPlayer.Cash - game.Cash;

            game.Player.Cash += game.Pot;
            game.Player.Winnings = game.Player.Cash - game.Cash;

            game.Pot = 0;
            game.Stage = GameStage.GameOver;
        }
        else if (betType == BetType.Check)
        {
            // if the ai player is checking, the pot remains
            // unchanged, the betting round ends and we proceed
            // to the draw

            game.AIPlayer.LastBetAmount = 0;
            game.AIPlayer.LastBetType = BetType.Check;

            game.Stage = GetNextGameStage(game.Stage);
        }
        else if (betType == BetType.Call)
        {
            // if the ai player is calling, they are matching
            // the player bet and asking to proceed to the draw

            game.AIPlayer.LastBetAmount = game.Player.LastBetAmount;
            game.AIPlayer.LastBetType = BetType.Call;
            game.AIPlayer.Cash -= game.AIPlayer.LastBetAmount;

            game.Pot += game.AIPlayer.LastBetAmount;

            game.Stage = GetNextGameStage(game.Stage);
        }
        else if (betType == BetType.Raise)
        {
            // if the ai player is raising, they are matching
            // the previous player bet, but then raising the
            // bet by an amount

            game.AIPlayer.LastBetAmount = amount;
            game.AIPlayer.LastBetType = BetType.Raise;
            game.AIPlayer.Cash -= game.Player.LastBetAmount;
            game.AIPlayer.Cash -= amount;

            game.Pot += game.Player.LastBetAmount;
            game.Pot += amount;
        }

        return game;
    }    

    public Game ProcessPlayerBet(Game game, BetType betType, int amount)
    {
        if (betType == BetType.Fold)
        {
            // if the player is folding, we should end the game
            // immediately and pay out the ai player

            game.Player.LastBetAmount = 0;
            game.Player.LastBetType = BetType.Fold;
            game.Player.LastBetChips = new();
            game.Player.Winnings = game.Player.Cash - game.Cash;
            
            game.AIPlayer.Cash += game.Pot;
            game.AIPlayer.Winnings = game.AIPlayer.Cash - game.Cash;            

            game.Pot = 0;
            game.Stage = GameStage.GameOver;
        }
        else if (betType == BetType.Check)
        {
            // if the player is checking, the pot remains
            // unchanged and we pass control to the ai player

            game.Player.LastBetAmount = 0;
            game.Player.LastBetType = BetType.Check;
            game.Player.LastBetChips = new();
        }
        else if (betType == BetType.Call)
        {
            // if the player is calling, they are matching
            // the ai players bet and asking to proceed to the draw

            game.Player.LastBetAmount = game.AIPlayer.LastBetAmount;
            game.Player.LastBetType = BetType.Call;
            game.Player.LastBetChips = new();

            game.Player.Cash -= game.Player.LastBetAmount;

            game.Pot += game.Player.LastBetAmount;

            game.Stage = GetNextGameStage(game.Stage);
        }
        else if (betType == BetType.Raise)
        {
            // if the player is raising, they are matching
            // the previous ai player bet, but then raising the
            // bet by an amount

            game.Player.LastBetAmount = amount;
            game.Player.LastBetType = BetType.Raise;
            game.Player.LastBetChips = Serializer.Clone(game.Player.Chips);

            game.Player.Cash -= game.AIPlayer.LastBetAmount;
            game.Player.Cash -= amount;

            game.Pot += game.AIPlayer.LastBetAmount;
            game.Pot += amount;
        }

        return game;
    }

    internal List<Chip> GetChips(int amount)
    {
        var chips = new List<Chip>();

        foreach (var chip in _settings.Chips)
        {
            if (chip.Value > amount)
                continue;

            chip.Quantity = amount / chip.Value;
            chips.Add(chip);

            var remainder = amount % chip.Value;
            if (remainder == 0)
                break;

            amount = remainder;
        }

        return chips;
    }    

    private (BetType, int) GetBet(Game game)
    {
        if (game.NextBetType.HasValue && game.NextBetAmount.HasValue)
        {
            var betType = game.NextBetType.Value;
            var betAmount = game.NextBetAmount.Value;

            game.NextBetType = null;
            game.NextBetAmount = null;

            return (betType, betAmount);
        }

        return _decisionService.GetBet(game);
    }

    private GameStage GetNextGameStage(GameStage stage)
    {
        // if we are in the 1st round of betting, the next stage
        // is the draw. if we are in the 2nd round of betting, the
        // next stage is the showdown

        return stage == GameStage.BetPreDraw ?
            GameStage.Draw :
            GameStage.Showdown;
    }    
}