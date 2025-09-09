using itapoker.SDK.Models;

namespace itapoker.SDK.Responses;

public class DealResponse
{
    public List<Card> Cards { get; set; } = new();
}