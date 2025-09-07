using AutoMapper;

using Domain = itapoker.Core.Domain;
using SDK = itapoker.SDK;

public class DomainToSDK : Profile
{
    public DomainToSDK()
    {
        CreateMap<Domain.Responses.HighScoreResponse, SDK.Responses.HighScoreResponse>();
        CreateMap<Domain.Responses.MainMenuResponse, SDK.Responses.MainMenuResponse>();
        CreateMap<Domain.Responses.NewGameResponse, SDK.Responses.NewGameResponse>();

        CreateMap<Domain.Enums.GameStage, SDK.Enums.GameStage>();
        CreateMap<Domain.Enums.PlayerType, SDK.Enums.PlayerType>();

        CreateMap<Domain.Models.Game, SDK.Models.Game>();
        CreateMap<Domain.Models.HighScore, SDK.Models.HighScore>();
        CreateMap<Domain.Models.Player, SDK.Models.Player>();
    }
}
