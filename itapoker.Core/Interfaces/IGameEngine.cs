using itapoker.Core.Domain.Requests;
using itapoker.Core.Domain.Responses;

namespace itapoker.Core.Interfaces;

public interface IGameEngine
{
    NewGameResponse NewGame(NewGameRequest request);
    void UpdateHighScores();
}