using itapoker.SDK.Enums;

namespace itapoker.SDK.Requests;

public class BetRequest
{
    public string GameId { get; set; }
    public BetType BetType { get; set; }
    public int Amount { get; set; }
}