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
    public BetType LastBetType { get; set; }
    public int LastBetAmount { get; set; }
    public List<Card> Cards { get; set; } = new();
}
