using itapoker.SDK.Enums;

namespace itapoker.SDK.Models;

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
    public string Alert { get; set; }
    public List<Player> Players { get; set; } = new();
    public Player Player { get; set; }
    public Player AIPlayer { get; set; }
    public List<Chip> BetChips { get; set; }
}