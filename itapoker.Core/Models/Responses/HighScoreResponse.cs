namespace itapoker.Core.Models.Responses;

public class HighScoreResponse
{
    public List<KeyValuePair<string, int>> HighScores { get; set; } = new();
}
