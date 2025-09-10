using itapoker.Core.Domain.Models;
using itapoker.Core.Domain.Requests;

namespace itapoker.Core.Interfaces;

public interface IGameEngine
{
    Game AnteUp(AnteUpRequest request);
    Game Bet(BetRequest request);
    Game Deal(DealRequest request);
    Game SinglePlayer(SinglePlayerRequest request);
}