using itapoker.Core.Domain.Requests;

namespace itapoker.Core.Services.Interfaces;

public interface IValidationService
{
    void AddChip(AddChipRequest request);
    void AnteUp(AnteUpRequest request);
    void Bet(BetRequest request);
    void Deal(DealRequest request);
    void Draw(DrawRequest request);
    void Hold(HoldRequest request);
    void Next(NextRequest request);
    void RemoveChip(RemoveChipRequest request);
    void Showdown(ShowdownRequest request);
    void SinglePlayer(SinglePlayerRequest request);
}