using CSHeroes.Core.Model;
using OriginalArmy = csheroes.src.Army;
using OriginalHero = csheroes.src.Hero;
using OriginalIGameObj = csheroes.src.IGameObj;
using OriginalUnit = csheroes.src.Units.Unit;
using OriginalAttackType = csheroes.src.Units.AttackType;

namespace CSHeroes.Core.Services;

internal sealed class BattleSession
{
    private readonly SpriteRect background;
    private readonly OriginalIGameObj?[,] action;
    private readonly OriginalHero hero;
    private readonly OriginalArmy firstArmy;
    private readonly OriginalArmy secondArmy;
    private readonly GridPoint[] firstArmyCords = new GridPoint[7];
    private readonly GridPoint[] secondArmyCords = new GridPoint[7];
    private int firstArmyTurn = -1;
    private int secondArmyTurn = -1;
    private bool turn = true;
    private bool ai;

    public BattleSession(OriginalHero hero, OriginalArmy enemy, SpriteRect background, bool ai)
    {
        this.hero = hero;
        this.background = background;
        this.ai = ai;
        action = new OriginalIGameObj?[GameConstants.CanvasHeight / GameConstants.BattleCellSize, GameConstants.CanvasWidth / GameConstants.BattleCellSize];

        firstArmy = hero.Army;
        LineArmy(firstArmy, firstArmyCords, 0);
        secondArmy = enemy;
        LineArmy(secondArmy, secondArmyCords, GameConstants.CanvasWidth / GameConstants.BattleCellSize - 1);
        NextTurn(firstArmy.Units, ref firstArmyTurn);
    }

    public bool BattleEnd { get; private set; }

    public void SetAi(bool value) => ai = value;

    public void ClickCell(int x, int y)
    {
        if (BattleEnd || !turn)
        {
            return;
        }

        PerformTurn(new GridPoint(x, y));
    }

    public void WaitTurn()
    {
        if (BattleEnd)
        {
            return;
        }

        AdvanceTurn();
    }

    public BattleSnapshot BuildSnapshot()
    {
        var objects = new List<SceneObjectSnapshot>();
        for (var y = 0; y < action.GetLength(0); y++)
        {
            for (var x = 0; x < action.GetLength(1); x++)
            {
                if (action[y, x] is OriginalUnit unit)
                {
                    objects.Add(new SceneObjectSnapshot(SceneObjectKind.Unit, x, y, ToSpriteRect(unit.Tile.Area), unit.Hp));
                }
            }
        }

        return new BattleSnapshot(
            action.GetLength(1),
            action.GetLength(0),
            background,
            objects,
            BuildHighlights(),
            turn,
            ai);
    }

    private IReadOnlyList<HighlightSnapshot> BuildHighlights()
    {
        var highlights = new List<HighlightSnapshot>();
        var friendCords = turn ? firstArmyCords : secondArmyCords;
        var friendArmy = turn ? firstArmy : secondArmy;
        var enemyArmy = turn ? secondArmy : firstArmy;
        var enemyCords = turn ? secondArmyCords : firstArmyCords;
        var index = turn ? firstArmyTurn : secondArmyTurn;

        if (index < 0 || friendArmy.Units[index] is not OriginalUnit unit)
        {
            return highlights;
        }

        var src = friendCords[index];
        highlights.Add(new HighlightSnapshot(src.X, src.Y, "active"));
        var reachableCells = GetReachableCells(src, unit.Range);
        var reachableSet = reachableCells.ToHashSet();

        foreach (var point in reachableCells)
        {
            if (point != src)
            {
                highlights.Add(new HighlightSnapshot(point.X, point.Y, "move"));
            }
        }

        for (var i = 0; i < 7; i++)
        {
            if (enemyArmy.Units[i] is null || enemyArmy.Units[i]!.Hp <= 0)
            {
                continue;
            }

            var enemyPoint = enemyCords[i];
            var canAttack = unit.Attack == OriginalAttackType.RANGE
                ? IsCellInRange(src, enemyPoint, unit.Range)
                : Neighbors(enemyPoint).Any(reachableSet.Contains);

            if (canAttack)
            {
                highlights.Add(new HighlightSnapshot(enemyPoint.X, enemyPoint.Y, "enemy"));
            }
        }

        return highlights;
    }

    private void PerformTurn(GridPoint dest)
    {
        var friendCords = turn ? firstArmyCords : secondArmyCords;
        var friendArmy = turn ? firstArmy : secondArmy;
        var enemyArmy = turn ? secondArmy : firstArmy;
        var enemyCords = turn ? secondArmyCords : firstArmyCords;
        var index = turn ? firstArmyTurn : secondArmyTurn;

        if (index < 0 || friendArmy.Units[index] is not OriginalUnit unit)
        {
            AdvanceTurn();
            return;
        }

        var src = friendCords[index];
        var acted = false;

        if (IsEnemyAt(dest, enemyArmy, enemyCords))
        {
            if (unit.Attack == OriginalAttackType.MELEE && !IsAdjacent(src, dest))
            {
                acted = MoveUnit(src, dest, unit, friendCords, index);
            }

            if (IsReadyAttack(unit, dest))
            {
                AttackUnit((OriginalUnit)action[dest.Y, dest.X]!, dest, unit);
                acted = true;
            }
        }
        else if (CellIsEmpty(dest.X, dest.Y) && IsCellInRange(src, dest, unit.Range))
        {
            acted = MoveUnit(src, dest, unit, friendCords, index);
        }

        if (acted)
        {
            AdvanceTurn();
        }
    }

    private void AdvanceTurn()
    {
        if (turn)
        {
            NextTurn(secondArmy.Units, ref secondArmyTurn);
        }
        else
        {
            NextTurn(firstArmy.Units, ref firstArmyTurn);
        }

        turn = !turn;

        if (!BattleEnd && ai && !turn)
        {
            AiMove();
        }
    }

    private void AiMove()
    {
        var index = secondArmyTurn;
        if (index < 0 || secondArmy.Units[index] is not OriginalUnit unit)
        {
            AdvanceTurn();
            return;
        }

        var src = secondArmyCords[index];
        var livingTargets = firstArmy.Units
            .Select((target, i) => new { target, point = firstArmyCords[i] })
            .Where(item => item.target is not null && item.target.Hp > 0)
            .OrderBy(item => Distance(src, item.point))
            .ToList();

        if (livingTargets.Count == 0)
        {
            EndBattle();
            return;
        }

        var target = livingTargets[0].point;
        var moved = false;
        if (unit.Attack == OriginalAttackType.MELEE && !IsAdjacent(src, target))
        {
            moved = MoveUnit(src, target, unit, secondArmyCords, index);
        }

        if (IsReadyAttack(unit, target))
        {
            AttackUnit((OriginalUnit)action[target.Y, target.X]!, target, unit);
        }
        else if (unit.Attack == OriginalAttackType.RANGE || !moved)
        {
            var current = secondArmyCords[index];
            var step = FindBestReachableStep(current, target, unit.Range);
            if (CellIsEmpty(step.X, step.Y))
            {
                action[current.Y, current.X] = null;
                action[step.Y, step.X] = unit;
                secondArmyCords[index] = step;
            }
        }

        if (!BattleEnd)
        {
            NextTurn(firstArmy.Units, ref firstArmyTurn);
            turn = true;
        }
    }

    private void AttackUnit(OriginalUnit enemy, GridPoint pos, OriginalUnit damager)
    {
        enemy.Hp -= damager.Damage;

        if (enemy.Hp > 0)
        {
            return;
        }

        enemy.Hp = 0;
        action[pos.Y, pos.X] = null;

        var enemyArmy = turn ? secondArmy : firstArmy;
        if (hero.Army != enemyArmy)
        {
            hero.Respect += enemy.Level * 100;
            damager.Exp += 1;
        }

        if (firstArmy.Empty || secondArmy.Empty)
        {
            EndBattle();
        }
    }

    private bool IsReadyAttack(OriginalUnit unit, GridPoint dest)
    {
        if (!IsCellInRange(CurrentUnitPoint(), dest, unit.Range))
        {
            return false;
        }

        return unit.Attack == OriginalAttackType.RANGE || IsAdjacent(CurrentUnitPoint(), dest);
    }

    private GridPoint CurrentUnitPoint()
    {
        return turn ? firstArmyCords[firstArmyTurn] : secondArmyCords[secondArmyTurn];
    }

    private bool IsEnemyAt(GridPoint point, OriginalArmy enemyArmy, GridPoint[] enemyCords)
    {
        if (point.X < 0 || point.Y < 0 || point.Y >= action.GetLength(0) || point.X >= action.GetLength(1))
        {
            return false;
        }

        for (var i = 0; i < 7; i++)
        {
            if (enemyArmy.Units[i] is not null && enemyArmy.Units[i]!.Hp > 0 && enemyCords[i] == point)
            {
                return true;
            }
        }

        return false;
    }

    private bool CellIsEmpty(int x, int y)
    {
        return x >= 0 && y >= 0 && y < action.GetLength(0) && x < action.GetLength(1) && action[y, x] is null;
    }

    private bool MoveUnit(GridPoint src, GridPoint dest, OriginalUnit unit, GridPoint[] friendCords, int index)
    {
        if (CellIsEmpty(src.X, src.Y))
        {
            return false;
        }

        var used = new bool[action.GetLength(0), action.GetLength(1)];
        var queue = new Queue<GridPoint>();
        queue.Enqueue(src);
        used[src.Y, src.X] = true;

        while (queue.Count > 0)
        {
            var point = queue.Dequeue();

            if (CellIsEmpty(dest.X, dest.Y))
            {
                if (point == dest)
                {
                    PlaceUnit(src, dest, unit, friendCords, index);
                    return true;
                }
            }
            else
            {
                var attackPosition = GetAttackPosition(point, dest);
                if (attackPosition is GridPoint position)
                {
                    PlaceUnit(src, position, unit, friendCords, index);
                    return true;
                }
            }

            foreach (var next in Neighbors(point))
            {
                if (!CellIsEmpty(next.X, next.Y) || used[next.Y, next.X] || !IsCellInRange(src, next, unit.Range))
                {
                    continue;
                }

                used[next.Y, next.X] = true;
                queue.Enqueue(next);
            }
        }

        return false;
    }

    private GridPoint? GetAttackPosition(GridPoint point, GridPoint target)
    {
        if (CellIsEmpty(target.X - 1, target.Y) && point.X == target.X - 1 && point.Y == target.Y)
        {
            return new GridPoint(target.X - 1, target.Y);
        }

        if (CellIsEmpty(target.X + 1, target.Y) && point.X == target.X + 1 && point.Y == target.Y)
        {
            return new GridPoint(target.X + 1, target.Y);
        }

        if (CellIsEmpty(target.X, target.Y - 1) && point.X == target.X && point.Y == target.Y - 1)
        {
            return new GridPoint(target.X, target.Y - 1);
        }

        if (CellIsEmpty(target.X, target.Y + 1) && point.X == target.X && point.Y == target.Y + 1)
        {
            return new GridPoint(target.X, target.Y + 1);
        }

        return null;
    }

    private void PlaceUnit(GridPoint src, GridPoint dest, OriginalUnit unit, GridPoint[] friendCords, int index)
    {
        friendCords[index] = dest;
        action[src.Y, src.X] = null;
        action[dest.Y, dest.X] = unit;
    }

    private static IEnumerable<GridPoint> Neighbors(GridPoint point)
    {
        yield return new GridPoint(point.X, point.Y - 1);
        yield return new GridPoint(point.X, point.Y + 1);
        yield return new GridPoint(point.X - 1, point.Y);
        yield return new GridPoint(point.X + 1, point.Y);
    }

    private static bool IsAdjacent(GridPoint a, GridPoint b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) == 1;
    }

    private static bool IsCellInRange(GridPoint src, GridPoint dest, int range)
    {
        return Math.Abs(dest.X - src.X) <= range && Math.Abs(dest.Y - src.Y) <= range;
    }

    private static double Distance(GridPoint a, GridPoint b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }

    private GridPoint FindBestReachableStep(GridPoint src, GridPoint target, int range)
    {
        var best = GetReachableCells(src, range)
            .Where(point => point != src)
            .OrderBy(point => Distance(point, target))
            .Cast<GridPoint?>()
            .FirstOrDefault();

        return best ?? src;
    }

    private IReadOnlyList<GridPoint> GetReachableCells(GridPoint src, int range)
    {
        var cells = new List<GridPoint>();
        var used = new bool[action.GetLength(0), action.GetLength(1)];
        var queue = new Queue<GridPoint>();
        queue.Enqueue(src);
        used[src.Y, src.X] = true;

        while (queue.Count > 0)
        {
            var point = queue.Dequeue();
            cells.Add(point);

            foreach (var next in Neighbors(point))
            {
                if (!CellIsEmpty(next.X, next.Y) || used[next.Y, next.X] || !IsCellInRange(src, next, range))
                {
                    continue;
                }

                used[next.Y, next.X] = true;
                queue.Enqueue(next);
            }
        }

        return cells;
    }

    private void NextTurn(OriginalUnit?[] units, ref int index)
    {
        for (var i = index + 1; i < 7; i++)
        {
            if (units[i] is not null && units[i]!.Hp != 0)
            {
                index = i;
                return;
            }
        }

        for (var i = 0; i < index + 1; i++)
        {
            if (units[i] is not null && units[i]!.Hp != 0)
            {
                index = i;
                return;
            }
        }

        EndBattle();
    }

    private void EndBattle()
    {
        BattleEnd = true;
    }

    private void LineArmy(OriginalArmy army, GridPoint[] cords, int column)
    {
        for (var i = 0; i < 7; i++)
        {
            var y = i * 2;
            if (army.Units[i] is not null && army.Units[i]!.Hp != 0)
            {
                action[y, column] = army.Units[i];
            }

            cords[i] = new GridPoint(column, y);
        }
    }

    private static SpriteRect ToSpriteRect(System.Drawing.Rectangle rectangle)
    {
        return new SpriteRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }
}
