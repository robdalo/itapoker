namespace itapoker.Core.Domain.Requests;

public class RemoveChipRequest
{
    public string GameId { get; set; }
    public int Value { get; set; }
}