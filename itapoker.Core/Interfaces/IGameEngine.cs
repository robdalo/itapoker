using itapoker.Core.Domain.Requests;
using itapoker.Core.Domain.Responses;

namespace itapoker.Core.Interfaces;

public interface IGameEngine
{
    AnteUpResponse AnteUp(AnteUpRequest request);
    DealResponse Deal(DealRequest request);
    SinglePlayerResponse SinglePlayer(SinglePlayerRequest request);
    void UpdateHighScores();
}