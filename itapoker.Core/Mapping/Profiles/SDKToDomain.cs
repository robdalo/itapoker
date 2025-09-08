using AutoMapper;

using Domain = itapoker.Core.Domain;
using SDK = itapoker.SDK;

public class SDKToDomain : Profile
{
    public SDKToDomain()
    {
        CreateMap<SDK.Requests.NewGameRequest, Domain.Requests.NewGameRequest>();

        CreateMap<SDK.Responses.HighScoreResponse, Domain.Responses.HighScoreResponse>();
        CreateMap<SDK.Responses.MainMenuResponse, Domain.Responses.MainMenuResponse>();
        CreateMap<SDK.Responses.NewGameResponse, Domain.Responses.NewGameResponse>();

        CreateMap<SDK.Enums.GameStage, Domain.Enums.GameStage>();
        CreateMap<SDK.Enums.PlayerType, Domain.Enums.PlayerType>();

        CreateMap<SDK.Models.Game, Domain.Models.Game>();
        CreateMap<SDK.Models.HighScore, Domain.Models.HighScore>();
        CreateMap<SDK.Models.Player, Domain.Models.Player>();
    }
}
