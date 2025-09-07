namespace itapoker.Core.Domain.Responses;

public class HighScoreResponse
{
    public List<KeyValuePair<string, int>> HighScores { get; set; } = new();
}
