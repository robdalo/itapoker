using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Domain.Models;

public class Game
{
    public int Id { get; set; }
    public string GameId { get; set; }
    public int Hand { get; set; }
    public GameStage Stage { get; set; }
    public int Ante { get; set; }
    public int Cash { get; set; }
    public int Limit { get; set; }
    public int Pot { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public BetType? NextBetType { get; set; }
    public int? NextBetAmount { get; set; }
    public List<Player> Players { get; set; } = new();
    public List<Card> Deck { get; set; } = new();
    public Player Player => Players.First(x => x.PlayerType == PlayerType.Human);
    public Player AIPlayer => Players.First(x => x.PlayerType == PlayerType.Computer);
}
