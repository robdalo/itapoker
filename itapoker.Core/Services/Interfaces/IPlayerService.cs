using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Interfaces;

public interface IPlayerService
{
    void AnteUp(string gameId);
    void Bet(string gameId, BetType betType, int amount);
}