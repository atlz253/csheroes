using CSHeroes.Core.Model;
using OriginalArmy = csheroes.src.Army;
using OriginalExploreMap = csheroes.src.ExploreMap;
using OriginalHero = csheroes.src.Hero;
using OriginalIGameObj = csheroes.src.IGameObj;
using OriginalObstacle = csheroes.src.Obstacle;
using OriginalAttackType = csheroes.src.Units.AttackType;
using OriginalUnit = csheroes.src.Units.Unit;

namespace CSHeroes.Core.Services;

public sealed class GameSession
{
    private OriginalExploreMap? exploreMap;
    private BattleSession? battle;
    private GridPoint? battleDestination;
    private GameScreen screen = GameScreen.MainMenu;
    private string? message;
    private int? score;
    private bool aiEnabled = true;

    public GameSnapshot Snapshot => BuildSnapshot();

    public void ShowNewGameMenu()
    {
        screen = GameScreen.NewGame;
        message = null;
    }

    public void ExitToMenu()
    {
        battle = null;
        battleDestination = null;
        screen = GameScreen.MainMenu;
        message = null;
        score = null;
    }

    public void StartNewGame(byte[] mapBytes)
    {
        exploreMap = CreateExploreMap();
        using var stream = new MemoryStream(mapBytes);
        using var reader = new BinaryReader(stream);
        exploreMap.ReadSave(reader);
        battle = null;
        screen = GameScreen.Exploring;
        message = null;
        score = null;
    }

    public void Load(byte[] saveBytes)
    {
        StartNewGame(saveBytes);
    }

    public byte[] Save()
    {
        if (exploreMap is null)
        {
            return Array.Empty<byte>();
        }

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        exploreMap.WriteSave(writer);
        writer.Flush();
        return stream.ToArray();
    }

    public void ClickExploreCell(int x, int y)
    {
        if (screen != GameScreen.Exploring || exploreMap is null)
        {
            return;
        }

        if (x < 0 || y < 0 || x >= MapColumns || y >= MapRows)
        {
            return;
        }

        var target = exploreMap.action[y, x];
        var destination = new GridPoint(x, y);

        if (IsBlockingCell(x, y) && target is not OriginalArmy)
        {
            return;
        }

        var reachableCells = GetReachableCells();

        if (target is OriginalArmy enemy && HasReachableAdjacentCell(destination, reachableCells))
        {
            battle = new BattleSession(exploreMap.hero, enemy, ToSpriteRect(exploreMap.battleMapBackgroundTile), aiEnabled);
            battleDestination = destination;
            screen = GameScreen.Battle;
            message = null;

            return;
        }

        if (target is null && reachableCells.Contains(destination))
        {
            MoveHero(destination);
        }
    }

    public void ClickBattleCell(int x, int y)
    {
        if (screen != GameScreen.Battle || battle is null || exploreMap is null)
        {
            return;
        }

        battle.ClickCell(x, y);
        FinishBattleIfNeeded();
    }

    public void WaitTurn()
    {
        if (screen == GameScreen.Battle && battle is not null)
        {
            battle.WaitTurn();
            FinishBattleIfNeeded();
        }
    }

    public void ToggleAi()
    {
        aiEnabled = !aiEnabled;
        battle?.SetAi(aiEnabled);
    }

    public void EnterCamp()
    {
        if (screen == GameScreen.Exploring && exploreMap is not null)
        {
            screen = GameScreen.Camp;
        }
    }

    public void ExitCamp()
    {
        if (screen == GameScreen.Camp)
        {
            screen = GameScreen.Exploring;
        }
    }

    public void HireUnit(int index)
    {
        if (screen != GameScreen.Camp || exploreMap is null || index < 0 || index >= 7)
        {
            return;
        }

        if (exploreMap.hero.Army.Units[index] is null && exploreMap.hero.Respect >= 100)
        {
            exploreMap.hero.Respect -= 100;
            exploreMap.hero.Army.Units[index] = new OriginalUnit(true);
        }
    }

    public void HealUnit(int index)
    {
        if (screen != GameScreen.Camp || exploreMap is null || index < 0 || index >= 7)
        {
            return;
        }

        var unit = exploreMap.hero.Army.Units[index];
        if (unit is null || unit.Hp == unit.MaxHp)
        {
            return;
        }

        var cost = 0;
        for (var i = 0; i < unit.MaxHp - unit.Hp; i++)
        {
            cost += (int)(10 + cost * 0.05);
        }

        if (exploreMap.hero.Respect >= cost)
        {
            exploreMap.hero.Respect -= cost;
            unit.Hp = unit.MaxHp;
        }
    }

    public void UpgradeUnit(int index, CampUpgradeOption option)
    {
        if (screen != GameScreen.Camp || exploreMap is null || index < 0 || index >= 7)
        {
            return;
        }

        var unit = exploreMap.hero.Army.Units[index];
        if (unit is null || unit.Exp < unit.NextLevel)
        {
            return;
        }

        if (unit.Range == 5)
        {
            UpgradeAttackType(unit, option);
            return;
        }

        UpgradeStats(unit, option);
    }

    private static OriginalExploreMap CreateExploreMap()
    {
        return new OriginalExploreMap(GameConstants.ExploreMapSerializedRows, GameConstants.ExploreMapSerializedColumns);
    }

    private void MoveHero(GridPoint point)
    {
        if (exploreMap is null)
        {
            return;
        }

        if (IsBlockingCell(point.X, point.Y) && exploreMap.action[point.Y, point.X] is not OriginalArmy)
        {
            return;
        }

        exploreMap.action[exploreMap.heroCords.Y, exploreMap.heroCords.X] = null;
        exploreMap.heroCords.X = point.X;
        exploreMap.heroCords.Y = point.Y;
        exploreMap.action[exploreMap.heroCords.Y, exploreMap.heroCords.X] = exploreMap.hero;

        if (point.X == exploreMap.winCell.X && point.Y == exploreMap.winCell.Y)
        {
            score = exploreMap.hero.Army.Units
                .Where(unit => unit is not null)
                .Sum(unit => unit!.Damage + unit.Hp + unit.Exp + unit.Range) + exploreMap.hero.Respect;
            screen = GameScreen.Win;
        }
    }

    private HashSet<GridPoint> GetReachableCells()
    {
        var reachable = new HashSet<GridPoint>();
        if (exploreMap is null)
        {
            return reachable;
        }

        var used = new bool[MapRows, MapColumns];
        var queue = new Queue<GridPoint>();
        var heroPosition = new GridPoint(exploreMap.heroCords.X, exploreMap.heroCords.Y);
        queue.Enqueue(heroPosition);
        used[heroPosition.Y, heroPosition.X] = true;
        reachable.Add(heroPosition);

        while (queue.Count > 0)
        {
            var point = queue.Dequeue();

            foreach (var next in Neighbors(point))
            {
                if (next.X < 0 || next.Y < 0 || next.X >= MapColumns || next.Y >= MapRows || used[next.Y, next.X])
                {
                    continue;
                }

                if (CanWalkThrough(next.X, next.Y))
                {
                    used[next.Y, next.X] = true;
                    reachable.Add(next);
                    queue.Enqueue(next);
                }
            }
        }

        return reachable;
    }

    private static bool HasReachableAdjacentCell(GridPoint destination, HashSet<GridPoint> reachableCells)
    {
        return Neighbors(destination).Any(reachableCells.Contains);
    }

    private static IEnumerable<GridPoint> Neighbors(GridPoint point)
    {
        yield return new GridPoint(point.X, point.Y - 1);
        yield return new GridPoint(point.X, point.Y + 1);
        yield return new GridPoint(point.X - 1, point.Y);
        yield return new GridPoint(point.X + 1, point.Y);
    }

    private bool CanWalkThrough(int x, int y)
    {
        if (exploreMap is null || x < 0 || y < 0 || x >= MapColumns || y >= MapRows)
        {
            return false;
        }

        return exploreMap.action[y, x] is null && !IsBlockingBackground(ToSpriteRect(exploreMap.background[y, x]));
    }

    private bool IsBlockingCell(int x, int y)
    {
        if (exploreMap is null || x < 0 || y < 0 || x >= MapColumns || y >= MapRows)
        {
            return true;
        }

        return exploreMap.action[y, x] is OriginalObstacle or OriginalArmy || IsBlockingBackground(ToSpriteRect(exploreMap.background[y, x]));
    }

    private static bool IsBlockingBackground(SpriteRect sprite)
    {
        return sprite.Y == 192;
    }

    private void FinishBattleIfNeeded()
    {
        if (battle is null || exploreMap is null || !battle.BattleEnd)
        {
            return;
        }

        if (exploreMap.hero.Army.Empty)
        {
            screen = GameScreen.Defeat;
            battleDestination = null;
            return;
        }

        if (battleDestination is GridPoint destination)
        {
            exploreMap.action[destination.Y, destination.X] = null;
            battleDestination = null;
        }

        battle = null;
        if (screen == GameScreen.Battle)
        {
            screen = GameScreen.Exploring;
        }
    }

    private GameSnapshot BuildSnapshot()
    {
        return screen switch
        {
            GameScreen.Exploring => new GameSnapshot(screen, Explore: BuildExploreSnapshot()),
            GameScreen.Battle => new GameSnapshot(screen, Explore: BuildExploreSnapshot(), Battle: battle?.BuildSnapshot()),
            GameScreen.Camp => new GameSnapshot(screen, Explore: BuildExploreSnapshot(), Camp: BuildCampSnapshot()),
            GameScreen.Win => new GameSnapshot(screen, Message: "Победа", Score: score),
            GameScreen.Defeat => new GameSnapshot(screen, Message: "Поражение"),
            _ => new GameSnapshot(screen, Message: message)
        };
    }

    private ExploreSnapshot? BuildExploreSnapshot()
    {
        if (exploreMap is null)
        {
            return null;
        }

        var objects = new List<SceneObjectSnapshot>();

        for (var y = 0; y < MapRows; y++)
        {
            for (var x = 0; x < MapColumns; x++)
            {
                var obj = exploreMap.action[y, x];
                if (obj is null)
                {
                    continue;
                }

                var kind = obj switch
                {
                    OriginalHero => SceneObjectKind.Hero,
                    OriginalArmy => SceneObjectKind.Army,
                    OriginalUnit => SceneObjectKind.Unit,
                    _ => SceneObjectKind.Obstacle
                };
                objects.Add(new SceneObjectSnapshot(kind, x, y, ToSpriteRect(obj.Tile.Area)));
            }
        }

        var background = new List<BackgroundCellSnapshot>();
        for (var y = 0; y < MapRows; y++)
        {
            for (var x = 0; x < MapColumns; x++)
            {
                background.Add(new BackgroundCellSnapshot(x, y, ToSpriteRect(exploreMap.background[y, x])));
            }
        }

        return new ExploreSnapshot(MapColumns, MapRows, background, objects, exploreMap.hero.Respect, exploreMap.locationName);
    }

    private int MapRows => exploreMap?.Width ?? 0;

    private int MapColumns => GameConstants.CanvasWidth / GameConstants.CellSize;

    private void UpgradeStats(OriginalUnit unit, CampUpgradeOption option)
    {
        if (exploreMap is null)
        {
            return;
        }

        var upgradeCost = unit.Level * 100;
        if (exploreMap.hero.Respect < upgradeCost)
        {
            return;
        }

        switch (option)
        {
            case CampUpgradeOption.Hp:
                unit.MaxHp += 3;
                unit.Hp += 3;
                break;
            case CampUpgradeOption.Damage:
                unit.Damage += 1;
                break;
            case CampUpgradeOption.Range:
                unit.Range += 1;
                break;
            default:
                return;
        }

        CompleteUpgrade(unit, upgradeCost);
    }

    private void UpgradeAttackType(OriginalUnit unit, CampUpgradeOption option)
    {
        if (exploreMap is null)
        {
            return;
        }

        const int upgradeCost = 500;
        if (exploreMap.hero.Respect < upgradeCost)
        {
            return;
        }

        switch (option)
        {
            case CampUpgradeOption.MeleeAttack:
                unit.Attack = OriginalAttackType.MELEE;
                break;
            case CampUpgradeOption.RangedAttack:
                unit.Attack = OriginalAttackType.RANGE;
                break;
            default:
                return;
        }

        unit.Range += 1;
        CompleteUpgrade(unit, upgradeCost);
    }

    private void CompleteUpgrade(OriginalUnit unit, int upgradeCost)
    {
        if (exploreMap is null)
        {
            return;
        }

        unit.Level += 1;
        unit.NextLevel *= 2;
        exploreMap.hero.Respect -= upgradeCost;
    }

    private CampSnapshot? BuildCampSnapshot()
    {
        if (exploreMap is null)
        {
            return null;
        }

        var units = new List<CampUnitSnapshot>();
        for (var i = 0; i < 7; i++)
        {
            var unit = exploreMap.hero.Army.Units[i];
            units.Add(unit is null
                ? new CampUnitSnapshot(i, null, 0, 0, 0, 0, 0, 0, 0, false, 0, CampUpgradeMode.None)
                : new CampUnitSnapshot(
                    i,
                    ToSpriteRect(unit.Tile.Area),
                    unit.Hp,
                    unit.MaxHp,
                    unit.Damage,
                    unit.Range,
                    unit.Exp,
                    unit.NextLevel,
                    unit.Level,
                    unit.Exp >= unit.NextLevel,
                    unit.Range == 5 ? 500 : unit.Level * 100,
                    unit.Exp < unit.NextLevel ? CampUpgradeMode.None : unit.Range == 5 ? CampUpgradeMode.AttackType : CampUpgradeMode.Stats));
        }

        return new CampSnapshot(exploreMap.hero.Respect, units);
    }

    private static SpriteRect ToSpriteRect(System.Drawing.Rectangle rectangle)
    {
        return new SpriteRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }
}
