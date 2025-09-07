using itapoker.Core.Domain.Enums;

namespace itapoker.Core.Domain.Models;

public class Game
{
    public int Id { get; set; }
    public string GameId { get; set; }
    public GameStage Stage { get; set; }
    public int Ante { get; set; }
    public int Cash { get; set; }
    public int Limit { get; set; }
    public List<Player> Players { get; set; } = new();
}
