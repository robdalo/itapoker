using itapoker.SDK.Enums;

namespace itapoker.SDK.Models;

public class Card
{
    public CardSuit Suit { get; set; }
    public CardRank Rank { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
}
