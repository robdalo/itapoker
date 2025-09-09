using itapoker.SDK.Enums;

namespace itapoker.SDK.Requests;

public class BetRequest
{
    public BetType BetType { get; set; }
    public int Amount { get; set; }
}