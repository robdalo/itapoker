using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;
using itapoker.Core.Services.Interfaces;

namespace itapoker.Core.Services;

public class SecurityService : ISecurityService
{
    public void Redact(Game game)
    {
        game.AIPlayer.PlayerId = "";

        // following the showdown, the ai player hand should be revealed

        if (game.Stage == GameStage.GameOver)
            return;

        game.AIPlayer.Hand = "";
        game.AIPlayer.HandType = HandType.None;

        foreach (var card in game.AIPlayer.Cards)
        {
            card.Rank = CardRank.None;
            card.Suit = CardSuit.None;
        }
    }
}