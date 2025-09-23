using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Domain.Requests;

public class HoldRequest
{
    public string GameId { get; set; }
    public CardRank Rank { get; set; }
    public CardSuit Suit { get; set; }
}