using itapoker.Core.Domain.Models;

namespace itapoker.Core.Domain.Responses;

public class DealResponse
{
    public List<Card> Cards { get; set; } = new();
}