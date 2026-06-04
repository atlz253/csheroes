namespace csheroes.src;

internal static class Global
{
    private static readonly Random Random = new();

    public static int CellSize => CSHeroes.Core.Model.GameConstants.CellSize;

    public static int BattleCellSize => CSHeroes.Core.Model.GameConstants.BattleCellSize;

    public static Random Rand => Random;
}
