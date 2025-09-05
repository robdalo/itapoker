using itapoker.Core.Models.Enums;

namespace itapoker.Core.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PlayerId { get; set; }
    public PlayerType PlayerType { get; set; }
    public int Cash { get; set; }
}
