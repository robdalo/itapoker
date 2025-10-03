using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Domain.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PlayerId { get; set; }
    public PlayerType PlayerType { get; set; }
    public int Cash { get; set; }
    public int Winnings { get; set; }
    public string Hand { get; set; }
    public HandType HandType { get; set; }
    public BetType LastBetType { get; set; }
    public int LastBetAmount { get; set; }
    public List<Card> Cards { get; set; } = new();
    public List<Chip> Chips { get; set; } = new();
    public string LastBetTypeTitle => GetBetTypeTitle(this.LastBetType);

    public static string GetBetTypeTitle(BetType betType)
    {
        switch (betType)
        {
            case BetType.Call: return "Call";
            case BetType.Check: return "Check";
            case BetType.Fold: return "Fold";
            case BetType.Raise: return "Raise";

            default: return "";
        }
    }
}