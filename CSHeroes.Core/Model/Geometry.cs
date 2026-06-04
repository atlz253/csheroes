namespace CSHeroes.Core.Model;

public readonly record struct GridPoint(int X, int Y);

public readonly record struct SpriteRect(int X, int Y, int Width, int Height);

public static class GameConstants
{
    public const int CellSize = 32;
    public const int BattleCellSize = 50;
    public const int CanvasWidth = 802;
    public const int CanvasHeight = 824;
    public const int StatusBarHeight = 22;
    public const int ExploreMapSerializedRows = 25;
    public const int ExploreMapSerializedColumns = 26;
}
