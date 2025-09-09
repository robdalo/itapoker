using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Interfaces;

public interface IAIPlayerService
{
    void AnteUp(string gameId);
    void Bet(string gameId, BetType betType, int amount);
}