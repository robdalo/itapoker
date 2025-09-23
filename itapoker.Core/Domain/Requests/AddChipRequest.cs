namespace itapoker.Core.Domain.Requests;

public class AddChipRequest
{
    public string GameId { get; set; }
    public int Value { get; set; }
}