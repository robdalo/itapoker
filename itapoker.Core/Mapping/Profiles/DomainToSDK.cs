using AutoMapper;

using Domain = itapoker.Core.Domain;
using SDK = itapoker.SDK;

public class DomainToSDK : Profile
{
    public DomainToSDK()
    {
        CreateMap<Domain.Requests.AnteUpRequest, SDK.Requests.AnteUpRequest>();
        CreateMap<Domain.Requests.BetRequest, SDK.Requests.BetRequest>();
        CreateMap<Domain.Requests.DealRequest, SDK.Requests.DealRequest>();
        CreateMap<Domain.Requests.DrawRequest, SDK.Requests.DrawRequest>();
        CreateMap<Domain.Requests.NextRequest, SDK.Requests.NextRequest>();
        CreateMap<Domain.Requests.ShowdownRequest, SDK.Requests.ShowdownRequest>();
        CreateMap<Domain.Requests.SinglePlayerRequest, SDK.Requests.SinglePlayerRequest>();

        CreateMap<Domain.Enums.BetType, SDK.Enums.BetType>();
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
