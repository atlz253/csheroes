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
        bool turn = true;
        bool close = false;

        public BattleForm(ExploreForm parent, Hero hero, Army enemy)
        {
            InitializeComponent();

            this.hero = hero;
            this.parent = parent;

            InitBackground();

            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize];

            firstArmy = hero.Army;
            LinedArmy(firstArmy, out firstArmyCords, 0);

            secondArmy = enemy;
            LinedArmy(secondArmy, out secondArmyCords, 6); // Width / Global.CellSize - 1

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
            surface.DrawRectangle(Global.HighlightPen, new Rectangle(tmp.X * Global.CellSize, tmp.Y * Global.CellSize, Global.CellSize, Global.CellSize));
        }

        void DrawGrid()
        {
            for (int i = 0; i < Width / Global.CellSize; i++)
                surface.DrawLine(Global.GridPen, Global.CellSize * i, 0, Global.CellSize * i, Height);

            for (int i = 0; i < Height / Global.CellSize; i++)
                surface.DrawLine(Global.GridPen, 0, Global.CellSize * i, Width, Global.CellSize * i);
        }

        void DrawAction()
        {
            for (int i = 0; i < Width / Global.CellSize; i++)
                for (int j = 0; j < Height / Global.CellSize; j++)
                    if (action[i, j] != null)
                        surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), action[i, j].GetTile(), GraphicsUnit.Pixel);
        }

        void InitBackground()
        {
            background = new Rectangle[Width / Global.CellSize, Height / Global.CellSize];

            for (int i = 0; i < Width / Global.CellSize; i++)
                for (int j = 0; j < Height / Global.CellSize; j++)
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
                        if (dest.Y - tmp.Y < 0)
                            while (tmp.Y != dest.Y + 1)
                                tmp.Y--;
                        else
                            while (tmp.Y != dest.Y - 1)
                                tmp.Y++;
                    else
                        if (dest.X - tmp.X < 0)
                        while (tmp.X != dest.X + 1)
                            tmp.X--;
                    else
                        while (tmp.X != dest.X - 1)
                            tmp.X++;

                    friendCords[index] = tmp;
                    action[tmp.Y, tmp.X] = unit;

                    move = false;
                    return true;
                }
                else if (tmp.Y < dest.Y && tmp.Y != Height / Global.CellSize - 2 && action[tmp.Y + 1, tmp.X] == null)
                {
                    while (tmp.Y != dest.Y && tmp.Y != Height / Global.CellSize - 2 && action[tmp.Y + 1, tmp.X] == null)
                    {
#if DEBUG
                        DrawArrow(Arrows.DOWN, tmp.X, tmp.Y);
#endif
                        arrow[tmp.Y, tmp.X] = Arrows.DOWN;
                        tmp.Y++;
                    }
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
                else if (tmp.X < dest.X && tmp.X != Width / Global.CellSize - 1 && action[tmp.Y, tmp.X + 1] == null && arrow[tmp.Y, tmp.X + 1] == Arrows.EMPTY)
                {
                    while (tmp.X != dest.X && tmp.X != Width / Global.CellSize - 1 && action[tmp.Y, tmp.X + 1] == null && arrow[tmp.Y, tmp.X + 1] == Arrows.EMPTY)
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
            arrow = new Arrows[Width / Global.CellSize, Height / Global.CellSize];
            Point dest = new Point(e.X / Global.CellSize, e.Y / Global.CellSize);
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

                if (next.Ai)
                    AIMove();
            }
            Draw();
        }

        void AIMove()
        {
            arrow = new Arrows[Width / Global.CellSize, Height / Global.CellSize];
            Point[] friendCords = turn ? firstArmyCords : secondArmyCords,
                    enemyCords = turn ? secondArmyCords : firstArmyCords;
            int index = turn ? firstArmyTurn : secondArmyTurn;
            Unit unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            Unit[] enemyUnits = turn ? firstArmy.Units : secondArmy.Units;

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

            if (Math.Abs(dest.X - tmp.X) > unit.Range && dest.X > tmp.X)
                dest.X = tmp.X + unit.Range;
            else if (Math.Abs(dest.X - tmp.X) > unit.Range && dest.X < tmp.X)
                dest.X = tmp.X - unit.Range;

            if (Math.Abs(dest.Y - tmp.Y) > unit.Range && dest.Y > tmp.Y)
                dest.Y = tmp.Y + unit.Range;
            else if (Math.Abs(dest.Y - tmp.Y) > unit.Range && dest.Y < tmp.Y)
                dest.Y = tmp.Y - unit.Range;

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

            surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * x, Global.CellSize * y, Global.CellSize, Global.CellSize), tile, GraphicsUnit.Pixel);
        }

        void DrawArrows()
        {
            if (arrow != null)
                for (int i = 0; i < Width / Global.CellSize; i++)
                    for (int j = 0; j < Height / Global.CellSize; j++)
                        DrawArrow(arrow[i, j], j, i);
        }

        void DrawBackground()
        {
            for (int i = 0; i < Width / Global.CellSize; i++)
                for (int j = 0; j < Height / Global.CellSize; j++)
                    surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), background[i, j], GraphicsUnit.Pixel);
        }

        void LinedArmy(Army army, out Point[] cordsArr, int column)
        {
            cordsArr = new Point[7];

            for (int i = 0; i < Height && i < 7; i++)
            {
                action[i*4, column] = army.Units[i];
                cordsArr[i] = new Point(column, i * 4);
            }
        }
    }
}
