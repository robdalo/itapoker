namespace itapoker.Core.Domain.Enums;

public enum GameStage
{
    None = 0,
    NewGame = 1,
    Ante = 2,
    Deal = 3,
    BetPreDraw = 4,
    Draw = 5,
    BetPostDraw = 6,
    Showdown = 7,
    GameOver = 8
}
