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

        Rectangle[,] background = null;
        IGameObj[,] action = null;
        Arrows[,] arrow = null;

        Army firstArmy,
             secondArmy;
        Point[] firstArmyCords,
                secondArmyCords;
        int firstArmyTurn = 0,
            secondArmyTurn = 0;
        bool turn = true;

        public BattleForm(Hero hero, Army enemy)
        {
            InitializeComponent();

            InitBackground();

            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize];

            firstArmy = hero.Army;
            LinedArmy(firstArmy, out firstArmyCords, 0);

            secondArmy = enemy;
            LinedArmy(secondArmy, out secondArmyCords, Width / Global.CellSize - 1);

            surface = CreateGraphics();
        }

        void Draw()
        {
            DrawBackground();
            DrawArrows();
            DrawAction();
            DrawGrid();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Draw();
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

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            arrow = new Arrows[Width / Global.CellSize, Height / Global.CellSize];
            Point dest = new Point(e.X / Global.CellSize, e.Y / Global.CellSize);
            Point[] friendCords = turn ? firstArmyCords : secondArmyCords,
                    enemyCords = turn ? secondArmyCords : firstArmyCords;

            foreach (Point cords in friendCords)
                if (dest == cords) // клик на дружественного юнита
                    return;

            int index = turn ? firstArmyTurn : secondArmyTurn;
            Unit unit = turn ? firstArmy.Units[index] : secondArmy.Units[index];
            Point tmp = new(friendCords[index].X, friendCords[index].Y);

            bool unitMove = (dest.X <= unit.Range || dest.Y <= unit.Range); // TODO: дальний бой

            while (unitMove)
            {
                if (tmp == dest)
                {
                    action[friendCords[index].Y, friendCords[index].X] = null;

                    friendCords[index] = dest;

                    action[dest.Y, dest.X] = unit;

                    Draw();

                    unitMove = false;
                }
                else if (tmp.Y < dest.Y && tmp.Y != Height / Global.CellSize - 2 && action[tmp.Y + 1, tmp.X] == null)
                {
                    while (tmp.Y != Height / Global.CellSize - 2 && action[tmp.Y + 1, tmp.X] == null)
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
                    while (tmp.Y != 0 && action[tmp.Y - 1, tmp.X] == null && arrow[tmp.Y - 1, tmp.X] == Arrows.EMPTY)
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
                    while (tmp.X != Width / Global.CellSize - 1 && action[tmp.Y, tmp.X + 1] == null && arrow[tmp.Y, tmp.X + 1] == Arrows.EMPTY)
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
                    while (tmp.X != 0 && action[tmp.Y, tmp.X - 1] == null)
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
                    Draw();
                    unitMove = false;
                }
            }
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
                cordsArr[i] = new Point(i * 4, column);
            }
        }
    }
}
