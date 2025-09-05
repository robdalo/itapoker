namespace itapoker.Core.Models.Responses;

public class NewGameResponse
{
    public string GameId { get; set; }
    public int Cash { get; set; }
    public int Ante { get; set; }
    public int Limit { get; set; }
    public List<Player> Players { get; set; } = new();
}
