using AutoMapper;

using Domain = itapoker.Core.Domain;
using SDK = itapoker.SDK;

public class SDKToDomain : Profile
{
    public SDKToDomain()
    {
        CreateMap<SDK.Requests.AnteUpRequest, Domain.Requests.AnteUpRequest>();
        CreateMap<SDK.Requests.BetRequest, Domain.Requests.BetRequest>();
        CreateMap<SDK.Requests.DealRequest, Domain.Requests.DealRequest>();
        CreateMap<SDK.Requests.DrawRequest, Domain.Requests.DrawRequest>();
        CreateMap<SDK.Requests.ShowdownRequest, Domain.Requests.ShowdownRequest>();
        CreateMap<SDK.Requests.SinglePlayerRequest, Domain.Requests.SinglePlayerRequest>();

        CreateMap<SDK.Enums.BetType, Domain.Enums.BetType>();
        CreateMap<SDK.Enums.CardRank, Domain.Enums.CardRank>();
        CreateMap<SDK.Enums.CardSuit, Domain.Enums.CardSuit>();
        CreateMap<SDK.Enums.GameStage, Domain.Enums.GameStage>();
        CreateMap<SDK.Enums.PlayerType, Domain.Enums.PlayerType>();

        CreateMap<SDK.Models.Card, Domain.Models.Card>();
        CreateMap<SDK.Models.Game, Domain.Models.Game>();
        CreateMap<SDK.Models.HighScore, Domain.Models.HighScore>();
        CreateMap<SDK.Models.Player, Domain.Models.Player>();
    }
}
