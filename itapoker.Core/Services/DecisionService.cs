using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Interfaces;

namespace itapoker.Core.Services;

public class DecisionService : IDecisionService
{
    public BetType GetBetType(Game game)
    {
        var random = new Random();

        var next = random.Next(1, 4);

        return (BetType)(next);
    }
}