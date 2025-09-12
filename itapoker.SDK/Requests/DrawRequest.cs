using itapoker.SDK.Models;

namespace itapoker.SDK.Requests;

public class DrawRequest
{
    public string GameId { get; set; }
    public List<Card> Cards { get; set; }
}