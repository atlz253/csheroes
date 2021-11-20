using csheroes.src;
using csheroes.src.unit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csheroes.form
{
    public partial class BattleForm : Form
    {
        Graphics surface;
        ExploreForm parent;

        Rectangle[,] background = null;
        IGameObj[,] action = null;
        Direction[,] arrow = null;

        Hero hero;
        Army firstArmy,
             secondArmy;
        Point[] firstArmyCords,
                secondArmyCords;
        int firstArmyTurn = 0,
            secondArmyTurn = 0;
        bool turn = true,
             close = false,
             ai = true; // включен ли искусственный интелект
        
        BattleFormSnapshot[] snapshots;
        int lastSnapshotIndex = -1;

        public BattleForm(ExploreForm parent, Hero hero, Army enemy)
        {
            InitializeComponent();

            this.hero = hero;
            this.parent = parent;
            snapshots = new BattleFormSnapshot[10];

            InitBackground();

            action = new IGameObj[Width / Global.BattleCellSize, Height / Global.BattleCellSize];

            firstArmy = hero.Army;
            LinedArmy(firstArmy, out firstArmyCords, 0);

            secondArmy = enemy;
            LinedArmy(secondArmy, out secondArmyCords, Width / Global.BattleCellSize - 1);

            surface = CreateGraphics();
        }

        void Draw()
        {
            DrawBackground();
            DrawArrows();
            DrawAction();
            DrawGrid();
            DrawHighlight();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        void DrawHighlight()
        {
            Point[] friendCords = turn ? firstArmyCords : secondArmyCords;
            int index = turn ? firstArmyTurn : secondArmyTurn;
            Unit unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];

            while (unit == null) // если юнит, который должен был ходить, трагически погиб
            {
                if (turn)
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                else
                    NextTurn(secondArmy.Units, ref secondArmyTurn);

                if (close) // не осталось юнитов
                    return;

                index = turn ? firstArmyTurn : secondArmyTurn;
                unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            }

            Point tmp = new(friendCords[index].X, friendCords[index].Y);
            surface.DrawRectangle(Global.HighlightPen, new Rectangle(tmp.X * Global.BattleCellSize, tmp.Y * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
        }

        void DrawGrid()
        {
            for (int i = 0; i < Width / Global.BattleCellSize; i++)
                surface.DrawLine(Global.GridPen, Global.BattleCellSize * i, 0, Global.BattleCellSize * i, Height);

            for (int i = 0; i < Height / Global.BattleCellSize; i++)
                surface.DrawLine(Global.GridPen, 0, Global.BattleCellSize * i, Width, Global.BattleCellSize * i);
        }

        void DrawAction()
        {
            for (int i = 0; i < Width / Global.BattleCellSize; i++)
                for (int j = 0; j < Height / Global.BattleCellSize; j++)
                    if (action[i, j] != null)
                        surface.DrawImage(Global.Texture, new Rectangle(Global.BattleCellSize * j, Global.BattleCellSize * i, Global.BattleCellSize, Global.BattleCellSize), action[i, j].GetTile(), GraphicsUnit.Pixel);
        }

        void InitBackground()
        {
            background = new Rectangle[Width / Global.BattleCellSize, Height / Global.BattleCellSize];

            for (int i = 0; i < Width / Global.BattleCellSize; i++)
                for (int j = 0; j < Height / Global.BattleCellSize; j++)
                    background[i, j] = new Rectangle(Global.CellSize * Global.Rand.Next(0, 2), Global.CellSize * Global.Rand.Next(0, 2), Global.CellSize, Global.CellSize);
        }

        void MovePoint(Direction direction, ref Point src)
        {
#if DEBUG
            DrawArrow(direction, src);
#endif
            switch (direction)
            {
                case Direction.DOWN:
                    src.Y++;
                    break;
                case Direction.LEFT:
                    src.X--;
                    break;
                case Direction.RIGHT:
                    src.X++;
                    break;
                case Direction.UP:
                    src.Y--;
                    break;
            }

            arrow[src.Y, src.X] = direction;
        }

        void DrawArrow(Direction direction, Point dest)
        {
            Rectangle tile = new Rectangle(0, 128, Global.CellSize, Global.CellSize);

            switch (direction)
            {
                case Direction.NONE:
                    return;
                case Direction.LEFT:
                    tile = new Rectangle(32, 128, Global.CellSize, Global.CellSize);
                    break;
                case Direction.DOWN:
                    tile = new Rectangle(0, 128, Global.CellSize, Global.CellSize);
                    break;
                case Direction.RIGHT:
                    tile = new Rectangle(32, 96, Global.CellSize, Global.CellSize);
                    break;
                case Direction.UP:
                    tile = new Rectangle(0, 96, Global.CellSize, Global.CellSize);
                    break;
            }

            surface.DrawImage(Global.Texture, new Rectangle(Global.BattleCellSize * dest.X, Global.BattleCellSize * dest.Y, Global.BattleCellSize, Global.BattleCellSize), tile, GraphicsUnit.Pixel);
        }

        bool CellIsEmpty(int x, int y)
        {
            if (x < 0 || x > Width / Global.BattleCellSize - 1 || y < 0 || y > Height / Global.BattleCellSize - 2) // TODO: проверить, что за фигня с -1 и -2
                return false;

            return (action[y, x] == null);
        }

        bool MoveUnit(Point dest, Unit unit, Point[] friendCords, int index)
        {
            Point tmp = friendCords[index];

            if (!CellIsEmpty(dest.X, dest.Y)) // на клетке стоит юнит
                if (CellIsEmpty(dest.X, dest.Y - 1)) // TODO: просчитывать расстояния до точек
                    dest.Y--;
                else if (CellIsEmpty(dest.X, dest.Y + 1))
                    dest.Y++;
                else if (CellIsEmpty(dest.X - 1, dest.Y))
                    dest.X--;
                else if (CellIsEmpty(dest.X + 1, dest.Y))
                    dest.X++;
                else
                    return false; // к юниту невозможно подойти

            while (tmp != dest)
                if (((tmp.Y < dest.Y || (tmp.X < dest.X && tmp.X + 1 != dest.X && !CellIsEmpty(tmp.X + 1, tmp.Y))) && CellIsEmpty(tmp.X, tmp.Y + 1)))
                    MovePoint(Direction.DOWN, ref tmp);
                else if ((tmp.X > dest.X && tmp.Y - 1 != dest.Y && CellIsEmpty(tmp.X - 1, tmp.Y) && arrow[tmp.Y, tmp.X - 1] == Direction.NONE) ||
                        (tmp.X == dest.X && CellIsEmpty(tmp.X - 1, tmp.Y) && tmp.Y != 0 && !CellIsEmpty(tmp.X, tmp.Y - 1)))
                    MovePoint(Direction.LEFT, ref tmp);
                else if (tmp.Y != dest.Y && CellIsEmpty(tmp.X, tmp.Y - 1) && arrow[tmp.Y - 1, tmp.X] == Direction.NONE)
                    MovePoint(Direction.UP, ref tmp);
                else if (tmp.X < dest.X && CellIsEmpty(tmp.X + 1, tmp.Y))
                    MovePoint(Direction.RIGHT, ref tmp);
                else
                    return false; // что-то пошло не так

            action[friendCords[index].Y, friendCords[index].X] = null; // перемещение на пустую клетку
            friendCords[index] = dest;
            action[dest.Y, dest.X] = unit;

            return true;
        }

        void AttackUnit(Unit enemy, Point pos, Unit damager)
        {
            enemy.Hp -= damager.Damage;

            if (enemy.Hp <= 0)
            {
                Army enemyArmy = turn ? secondArmy : firstArmy;

                for (int i = 0; i < enemyArmy.Units.Length; i++)
                    if (action[pos.Y, pos.X] == enemyArmy.Units[i])
                        enemyArmy.Units[i] = null;

                action[pos.Y, pos.X] = null;

                if (hero.Army != enemyArmy)
                {
                    hero.Respect += 100;
                    damager.Exp += 1;
                }

            }
        }

        bool UnitNearby(Point cords, Unit unit)
        {
            if  ((cords.X != 0 && action[cords.Y, cords.X - 1] == unit) ||
                (cords.X != Width / Global.BattleCellSize - 1 && action[cords.Y, cords.X + 1] == unit) ||
                (cords.Y != 0 && action[cords.Y - 1, cords.X] == unit) ||
                (cords.Y != Height / Global.BattleCellSize - 2 && action[cords.Y + 1, cords.X] == unit))
                return true;
            else
                return false;
        }

        bool IsNeedUnitMove(Unit unit, Point src, Point dest)
        {
            return ((action[dest.Y, dest.X] == null || (unit.Attack == AttackType.MELEE && !UnitNearby(dest, unit))) && Math.Abs(dest.X - src.X) <= unit.Range && Math.Abs(dest.Y - src.Y) <= unit.Range);
        }

        bool IsReadyAttack(Unit unit, Point[] friends, Point dest)
        {
            if (CellIsEmpty(dest.X, dest.Y))
                return false;

            foreach (Point friend in friends)
                if (dest == friend)
                    return false;

            return ((unit.Attack == AttackType.RANGE)) || ((unit.Attack == AttackType.MELEE) && UnitNearby(dest, unit));
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            arrow = new Direction[Width / Global.BattleCellSize, Height / Global.BattleCellSize];
            Point dest = new Point(e.X / Global.BattleCellSize, e.Y / Global.BattleCellSize);
            Point[] friendCords = turn ? firstArmyCords : secondArmyCords;

            foreach (Point cords in friendCords)
                if (dest == cords) // клик на дружественного юнита
                    return;

            int index = turn ? firstArmyTurn : secondArmyTurn;
            Unit unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];

            while (unit == null) // если юнит, который должен был ходить, трагически погиб
            {
                if (turn)
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                else
                    NextTurn(secondArmy.Units, ref secondArmyTurn);

                if (close) // не осталось юнитов
                    return;

                index = turn ? firstArmyTurn : secondArmyTurn;
                unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            }

            Point tmp = new(friendCords[index].X, friendCords[index].Y);

            bool unitTurn = false;

            WriteSnapshot();
            if (IsNeedUnitMove(unit, tmp, dest))
                unitTurn = MoveUnit(dest, unit, friendCords, index); // TODO: если на range + 1 противник, то ударить его

            if (IsReadyAttack(unit, friendCords, dest))
            {
                AttackUnit((Unit)action[dest.Y, dest.X], dest, unit);

                unitTurn = true;
            }

            if (unitTurn)
            {
                if (turn)
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                else
                    NextTurn(secondArmy.Units, ref secondArmyTurn);

                turn = !turn;

                Army next = turn ? firstArmy : secondArmy;

                if (ai && next.Ai)
                    AIMove();
            }
            Draw();
        }

        double VectorLenght(int x, int y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        void AIMove()
        {
            arrow = new Direction[Width / Global.BattleCellSize, Height / Global.BattleCellSize];
            Point[] friendCords = turn ? firstArmyCords : secondArmyCords,
                    enemyCords = turn ? secondArmyCords : firstArmyCords;
            int index = turn ? firstArmyTurn : secondArmyTurn;
            Unit unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            Unit[] enemyUnits = turn ? secondArmy.Units :firstArmy.Units;

            WriteSnapshot();
            while (unit == null) // если юнит, который должен был ходить, трагически погиб
            {
                if (turn)
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                else
                    NextTurn(secondArmy.Units, ref secondArmyTurn);

                if (close) // не осталось юнитов
                    return;

                index = turn ? firstArmyTurn : secondArmyTurn;
                unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            }

            Point dest = new(-1, -1), tmp = new(friendCords[index].X, friendCords[index].Y);

            for (int i = 0; i < 7; i++)
                if (enemyUnits[i] != null && (dest.X == -1 || (VectorLenght(dest.X - tmp.X, dest.Y - tmp.Y) > VectorLenght(enemyCords[i].X - tmp.X, enemyCords[i].Y - tmp.Y))))
                    dest = enemyCords[i];

            if (Math.Abs(dest.X - tmp.X) > unit.Range)
                if (dest.X < tmp.X)
                    {
                        dest.X = tmp.X - unit.Range;

                        if (dest.X < 0)
                            dest.X = 0;
                    }
                else // dest.X < tmp.X
                {
                    dest.X = tmp.X + unit.Range;

                    if (dest.X > Width / Global.BattleCellSize - 1)
                        dest.X = Width / Global.BattleCellSize - 1;
                }

            if (Math.Abs(dest.Y - tmp.Y) > unit.Range)
                if (dest.Y < tmp.Y)
                {

                    dest.Y = tmp.Y - unit.Range;

                    if (dest.Y < 0)
                        dest.Y = 0;
                }
                else
                { 
                    dest.Y = tmp.Y + unit.Range;

                    if (dest.Y > Height / Global.BattleCellSize - 1)
                        dest.Y = Height / Global.BattleCellSize - 1;
                }

            bool unitTurn = false;

            if (IsNeedUnitMove(unit, tmp, dest))
                unitTurn = MoveUnit(dest, unit, friendCords, index); // TODO: если на range + 1 противник, то ударить его

            if (IsReadyAttack(unit, friendCords, dest))
            {
                AttackUnit((Unit)action[dest.Y, dest.X], dest, unit);

                unitTurn = true;
            }

            if (unitTurn)
            {
                if (turn)
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                else
                    NextTurn(secondArmy.Units, ref secondArmyTurn);

                turn = !turn;
            }
            Draw();
        }

        void NextTurn(Unit[] units, ref int index)
        {
            for (int i = index + 1; i < 7; i++) // проверяем остаток массива
                if (units[i] != null)
                {
                    index = i;
                    return;
                }

            for (int i = 0; i < index + 1; i++) // начинаем смотреть сначала
                if (units[i] != null)
                {
                    index = i;
                    return;
                }

            EndBattle();
        }

        private void SwitchAIBtn(object sender, EventArgs e)
        {
            ai = false;
        }

        private void WaitBtnClick(object sender, EventArgs e)
        {
            WriteSnapshot();

            if (turn)
                NextTurn(firstArmy.Units, ref firstArmyTurn);
            else
                NextTurn(secondArmy.Units, ref secondArmyTurn);

            turn = !turn;

            Army next = turn ? firstArmy : secondArmy;

            if (ai && next.Ai)
                AIMove();
        }

        void EndBattle()
        {
#if !TEST_BATTLE
            parent.Location = new Point(Location.X, Location.Y);
#endif

            Close();
            close = true;
        }

        void DrawArrows()
        {
            if (arrow != null)
                for (int i = 0; i < Width / Global.BattleCellSize; i++)
                    for (int j = 0; j < Height / Global.BattleCellSize; j++)
                        DrawArrow(arrow[i, j], new Point(j, i));
        }

        private void TurnBackBtn(object sender, EventArgs e)
        {
            RestoreSnapshot();
        }

        void DrawBackground()
        {
            for (int i = 0; i < Width / Global.BattleCellSize; i++)
                for (int j = 0; j < Height / Global.BattleCellSize; j++)
                    surface.DrawImage(Global.Texture, new Rectangle(Global.BattleCellSize * j, Global.BattleCellSize * i, Global.BattleCellSize, Global.BattleCellSize), background[i, j], GraphicsUnit.Pixel);
        }

        void LinedArmy(Army army, out Point[] cordsArr, int column)
        {
            cordsArr = new Point[7];

            for (int i = 0; i < Height && i < 7; i++)
            {
                action[i*2, column] = army.Units[i];
                cordsArr[i] = new Point(column, i * 2);
            }
        }

        BattleFormSnapshot MakeSnapshot()
        {
            return new(hero.MakeSnapshot(), secondArmy.MakeSnapshot(), firstArmyCords, secondArmyCords, firstArmyTurn, secondArmyTurn, turn, ai);
        }

        void WriteSnapshot()
        {
            if (lastSnapshotIndex != 9)
                lastSnapshotIndex++;
            else
                lastSnapshotIndex = 0;
            snapshots[lastSnapshotIndex] = MakeSnapshot();
        }

        void RestoreSnapshot()
        {
            BattleFormSnapshot snapshot = snapshots[lastSnapshotIndex];
            snapshots[lastSnapshotIndex] = null;

            if (snapshot == null)
                return;

            if (lastSnapshotIndex != 0)
                lastSnapshotIndex--;
            else
                lastSnapshotIndex = 9;

            hero = new Hero(snapshot.hero);
            firstArmy = hero.Army;
            secondArmy = new Army(snapshot.secondArmy);
            action = new IGameObj[Width / Global.BattleCellSize, Height / Global.BattleCellSize];
            arrow = null;

            for (int i = 0; i < 7; i++)
            {
                firstArmyCords[i] = snapshot.firstArmyCords[i];
                if (firstArmy.Units[i] != null)
                    action[firstArmyCords[i].Y, firstArmyCords[i].X] = firstArmy.Units[i];

                secondArmyCords[i] = snapshot.secondArmyCords[i];
                if (secondArmy.Units[i] != null)
                    action[secondArmyCords[i].Y, secondArmyCords[i].X] = secondArmy.Units[i];
            }

            firstArmyTurn = snapshot.firstArmyTurn;
            secondArmyTurn = snapshot.secondArmyTurn;
            turn = snapshot.turn;
            ai = snapshot.ai;

            Draw();
        }
    }

    enum Direction
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public class BattleFormSnapshot
    {
        public readonly HeroSnapshot hero;
        public readonly ArmyShapshot secondArmy;
        public readonly Point[] firstArmyCords,
                                secondArmyCords;
        public readonly int firstArmyTurn,
            secondArmyTurn;
        public readonly bool turn, ai;

        public BattleFormSnapshot(HeroSnapshot hero, ArmyShapshot army, Point[] firstArmyCords, Point[] secondArmyCords, int firstArmyTurn, int secondArmyTurn, bool turn, bool ai)
        {
            this.hero = hero;
            secondArmy = army;

            this.firstArmyCords = new Point[7];
            for (int i = 0; i < 7; i++)
                this.firstArmyCords[i] = firstArmyCords[i];

            this.secondArmyCords = new Point[7];
            for (int i = 0; i < 7; i++)
                this.secondArmyCords[i] = secondArmyCords[i];

            this.firstArmyTurn = firstArmyTurn;
            this.secondArmyTurn = secondArmyTurn;
            this.turn = turn;
            this.ai = ai;
        }
    }
}
