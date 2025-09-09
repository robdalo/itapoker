namespace itapoker.Core.Domain.Requests;

public class SinglePlayerRequest
{
    public string PlayerName { get; set; }
    public int Cash { get; set; }
    public int Ante { get; set; }
    public int Limit { get; set; }
}
