using itapoker.Core.Models.Requests;
using itapoker.Core.Models.Responses;

namespace itapoker.Core.Interfaces;

public interface IGameEngine
{
    NewGameResponse NewGame(NewGameRequest request);
}