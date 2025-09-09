using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Domain.Requests;

public class BetRequest
{
    public string GameId { get; set; }
    public BetType BetType { get; set; }
    public int Amount { get; set; }
}