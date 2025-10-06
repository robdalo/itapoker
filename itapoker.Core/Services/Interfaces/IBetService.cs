using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;

namespace itapoker.Core.Services.Interfaces;

public interface IBetService
{
    Game ProcessAnteUp(Game game);
    Game ProcessAIPlayerBet(Game game);
    Game ProcessPlayerBet(Game game, BetType betType, int amount);
}