namespace itapoker.Core.Domain.Models;

public class GameSettings
{
    public List<Chip> Chips => new() { new(5), new(10), new(25), new(50) };
}