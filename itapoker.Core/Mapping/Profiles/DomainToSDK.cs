using AutoMapper;

using Domain = itapoker.Core.Domain;
using SDK = itapoker.SDK;

public class DomainToSDK : Profile
{
    public DomainToSDK()
    {
        CreateMap<Domain.Requests.AnteUpRequest, SDK.Requests.AnteUpRequest>();
        CreateMap<Domain.Requests.DealRequest, SDK.Requests.DealRequest>();
        CreateMap<Domain.Requests.SinglePlayerRequest, SDK.Requests.SinglePlayerRequest>();

        CreateMap<Domain.Responses.AnteUpResponse, SDK.Responses.AnteUpResponse>();
        CreateMap<Domain.Responses.DealResponse, SDK.Responses.DealResponse>();
        CreateMap<Domain.Responses.SinglePlayerResponse, SDK.Responses.SinglePlayerResponse>();

        CreateMap<Domain.Enums.CardRank, SDK.Enums.CardRank>();
        CreateMap<Domain.Enums.CardSuit, SDK.Enums.CardSuit>();
        CreateMap<Domain.Enums.GameStage, SDK.Enums.GameStage>();
        CreateMap<Domain.Enums.PlayerType, SDK.Enums.PlayerType>();

        CreateMap<Domain.Models.Card, SDK.Models.Card>();
        CreateMap<Domain.Models.Game, SDK.Models.Game>();
        CreateMap<Domain.Models.HighScore, SDK.Models.HighScore>();
        CreateMap<Domain.Models.Player, SDK.Models.Player>();
    }
}
