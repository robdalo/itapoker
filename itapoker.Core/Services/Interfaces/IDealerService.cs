namespace itapoker.Core.Services.Interfaces;

public interface IDealerService
{
    void AnteUp(string gameId);
    void Deal(string gameId);
    void Shuffle(string gameId);
}