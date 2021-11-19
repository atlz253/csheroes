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
        Arrows[,] arrow = null;

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

        bool MoveUnit(Point dest, Unit unit, Point[] friendCords, int index)
        {
            bool move = true;
            Point tmp = friendCords[index];

            while (move)
            {
                if (tmp == dest) // перемещение на пустую клетку
                {
                    action[friendCords[index].Y, friendCords[index].X] = null;

                    friendCords[index] = dest;

                    action[dest.Y, dest.X] = unit;

                    move = false;
                    return true;
                }
                else if (action[dest.Y, dest.X] != null && ((tmp.X == dest.X && Math.Abs(dest.Y - tmp.Y) <= unit.Range) || (tmp.Y == dest.Y && Math.Abs(dest.X - tmp.X) <= unit.Range))) // ближний бой
                {
                    action[friendCords[index].Y, friendCords[index].X] = null;

                    if (tmp.X == dest.X)
                        if (dest.Y - tmp.Y < 0 && action[dest.Y - 1, dest.X] == null)
                            while (tmp.Y != dest.Y + 1)
                                tmp.Y--;
                        else if (action[dest.Y + 1, dest.X] == null)
                            while (tmp.Y != dest.Y - 1)
                                tmp.Y++;
                    else if (dest.X - tmp.X < 0 && action[dest.Y, dest.X - 1] == null)
                        while (tmp.X != dest.X + 1)
                            tmp.X--;
                    else if (action[dest.Y, dest.X + 1] == null)
                        while (tmp.X != dest.X - 1)
                            tmp.X++;

                    friendCords[index] = tmp;
                    action[tmp.Y, tmp.X] = unit;

                    move = false;
                    return true;
                }
                else if (tmp.Y < dest.Y && tmp.Y != Height / Global.BattleCellSize - 2 && action[tmp.Y + 1, tmp.X] == null)
                {
                    while (tmp.Y != dest.Y && tmp.Y != Height / Global.BattleCellSize - 2 && action[tmp.Y + 1, tmp.X] == null)
                    {
#if DEBUG
                        DrawArrow(Arrows.DOWN, tmp.X, tmp.Y);
#endif
                        arrow[tmp.Y, tmp.X] = Arrows.DOWN;
                        tmp.Y++;
                    }
                }
                else if (tmp != dest && tmp.Y != Height / Global.BattleCellSize - 2 && tmp.X != 0 && action[tmp.Y, tmp.X - 1] != null) // Пытаемся обойти препятствие
                {
#if DEBUG
                    DrawArrow(Arrows.DOWN, tmp.X, tmp.Y);
#endif
                    arrow[tmp.Y, tmp.X] = Arrows.DOWN;
                    tmp.Y++;
                }
                else if (tmp.Y != dest.Y && tmp.Y != 0 && action[tmp.Y - 1, tmp.X] == null && arrow[tmp.Y - 1, tmp.X] == Arrows.EMPTY)
                {
                    while (tmp.Y != dest.Y && tmp.Y != 0 && action[tmp.Y - 1, tmp.X] == null && arrow[tmp.Y - 1, tmp.X] == Arrows.EMPTY)
                    {
#if DEBUG
                        DrawArrow(Arrows.UP, tmp.X, tmp.Y);
#endif
                        arrow[tmp.Y, tmp.X] = Arrows.UP;
                        tmp.Y--;
                    }
                }
                else if (tmp.X < dest.X && tmp.X != Width / Global.BattleCellSize - 1 && action[tmp.Y, tmp.X + 1] == null && arrow[tmp.Y, tmp.X + 1] == Arrows.EMPTY)
                {
                    while (tmp.X != dest.X && tmp.X != Width / Global.BattleCellSize - 1 && action[tmp.Y, tmp.X + 1] == null && arrow[tmp.Y, tmp.X + 1] == Arrows.EMPTY)
                    {
#if DEBUG
                        DrawArrow(Arrows.RIGHT, tmp.X, tmp.Y);
#endif

                        arrow[tmp.Y, tmp.X] = Arrows.RIGHT;
                        tmp.X++;
                    }
                }
                else if (tmp.X != 0 && action[tmp.Y, tmp.X - 1] == null)
                {
                    while (tmp.X != dest.X && tmp.X != 0 && action[tmp.Y, tmp.X - 1] == null)
                    {
#if DEBUG
                        DrawArrow(Arrows.LEFT, tmp.X, tmp.Y);
#endif

                        arrow[tmp.Y, tmp.X] = Arrows.LEFT;
                        tmp.X--;
                    }
                }
                else if (tmp.X != 0 && tmp.X - 1 != 0 && action[tmp.Y, tmp.X - 1] == null) // обход второй половины доступных ходов 
                {
#if DEBUG
                    DrawArrow(Arrows.LEFT, tmp.X, tmp.Y);
#endif
                    arrow[tmp.Y, tmp.X] = Arrows.LEFT;
                    tmp.X -= 2;
                }
                else
                {
                    move = false;
                }
            }

            return false;
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

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            arrow = new Arrows[Width / Global.BattleCellSize, Height / Global.BattleCellSize];
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

            bool unitMove = ((action[dest.Y, dest.X] == null || unit.Attack == AttackType.MELEE) && Math.Abs(dest.X - tmp.X) <= unit.Range && Math.Abs(dest.Y - tmp.Y) <= unit.Range), 
                 unitTurn = false;

            WriteSnapshot();
            if (unitMove)
                unitTurn = MoveUnit(dest, unit, friendCords, index); // TODO: если на range + 1 противник, то ударить его

            bool unitAttack = ((unit.Attack == AttackType.RANGE) && (action[dest.Y, dest.X] != null)) || ((unit.Attack == AttackType.MELEE) && ((tmp.X == dest.X && Math.Abs(dest.Y - friendCords[index].Y) == 1) || (tmp.Y == dest.Y && Math.Abs(dest.X - friendCords[index].X) == 1)));

            if (unitAttack)
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

        void AIMove()
        {
            arrow = new Arrows[Width / Global.BattleCellSize, Height / Global.BattleCellSize];
            Point[] friendCords = turn ? firstArmyCords : secondArmyCords,
                    enemyCords = turn ? secondArmyCords : firstArmyCords;
            int index = turn ? firstArmyTurn : secondArmyTurn;
            Unit unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            Unit[] enemyUnits = turn ? firstArmy.Units : secondArmy.Units;

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

            Point dest = new(0, 0), tmp = new(friendCords[index].X, friendCords[index].Y);

            for (int i = 0; i < 7; i++)
                if (enemyUnits[i] != null)
                    dest = enemyCords[i];
            
            foreach (Point cords in enemyCords)
                if (Math.Sqrt((dest.X - tmp.X)*(dest.X - tmp.X)+(dest.Y - tmp.Y)*(dest.Y - tmp.Y)) > Math.Sqrt((dest.X - cords.X) * (dest.X - cords.X) + (dest.Y - cords.Y) * (dest.Y - cords.Y)))
                    dest = cords;

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
                if (dest.Y > tmp.Y)
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

            bool unitMove = ((unit.Attack == AttackType.MELEE) && (Math.Abs(dest.X - tmp.X) <= unit.Range && Math.Abs(dest.Y - tmp.Y) <= unit.Range)),
                 unitTurn = false;

            if (unitMove)
                unitTurn = MoveUnit(dest, unit, friendCords, index); // TODO: если на range + 1 противник, то ударить его

            bool unitAttack = ((unit.Attack == AttackType.RANGE) && (action[dest.Y, dest.X] != null)) || ((unit.Attack == AttackType.MELEE) && ((tmp.X == dest.X && Math.Abs(dest.Y - friendCords[index].Y) == 1) || (tmp.Y == dest.Y && Math.Abs(dest.X - friendCords[index].X) == 1)));

            if (unitAttack)
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

        void DrawArrow(Arrows direction, int x, int y)
        {
            Rectangle tile = new Rectangle(0, 128, Global.CellSize, Global.CellSize);

            switch (direction)
            {
                case Arrows.EMPTY:
                    return;
                case Arrows.LEFT:
                    tile = new Rectangle(32, 128, Global.CellSize, Global.CellSize);
                    break;
                case Arrows.DOWN:
                    tile = new Rectangle(0, 128, Global.CellSize, Global.CellSize);
                    break;
                case Arrows.RIGHT:
                    tile = new Rectangle(32, 96, Global.CellSize, Global.CellSize);
                    break;
                case Arrows.UP:
                    tile = new Rectangle(0, 96, Global.CellSize, Global.CellSize);
                    break;
            }

            surface.DrawImage(Global.Texture, new Rectangle(Global.BattleCellSize * x, Global.BattleCellSize * y, Global.BattleCellSize, Global.BattleCellSize), tile, GraphicsUnit.Pixel);
        }

        void DrawArrows()
        {
            if (arrow != null)
                for (int i = 0; i < Width / Global.BattleCellSize; i++)
                    for (int j = 0; j < Height / Global.BattleCellSize; j++)
                        DrawArrow(arrow[i, j], j, i);
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
