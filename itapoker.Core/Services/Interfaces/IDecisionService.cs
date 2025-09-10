using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;

namespace itapoker.Core.Interfaces;

public interface IDecisionService
{
    BetType GetBetType(Game game);
}