using itapoker.Core.Domain.Models;

namespace itapoker.Core.Domain.Requests;

public class DrawRequest
{
    public string GameId { get; set; }
    public List<Card> Cards { get; set; }
}