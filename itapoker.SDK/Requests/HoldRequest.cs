using itapoker.SDK.Enums;

namespace itapoker.SDK.Requests;

public class HoldRequest
{
    public string GameId { get; set; }
    public CardRank Rank { get; set; }
    public CardSuit Suit { get; set; }
}