using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;

namespace itapoker.Core.Services.Interfaces;

public interface IDecisionService
{
    (BetType, int) GetBet(Game game);
    List<Card> GetDiscardedCards(Game game);
}