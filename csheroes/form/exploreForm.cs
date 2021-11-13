using csheroes.src;
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
    public partial class ExploreForm : Form
    {
        Graphics surface;

        Rectangle[,] background = null;
        IGameObj[,] action = null;

        Hero hero = null;
        Point heroCords;

        public ExploreForm()
        {
            InitializeComponent();

            InitBackground();
            InitAction();

            surface = CreateGraphics();
        }

        void Draw()
        {
            surface.Clear(Color.White);

            DrawBackground();
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

        void InitBackground()
        {
            background = new Rectangle[Width / Global.CellSize, Height / Global.CellSize];

            for (int i = 0; i < Width / Global.CellSize; i++)
                for (int j = 0; j < Height / Global.CellSize; j++)
                    background[i, j] = new Rectangle(Global.CellSize * Global.Rand.Next(0, 2), Global.CellSize * Global.Rand.Next(0, 2), Global.CellSize, Global.CellSize);
        }

        void DrawBackground()
        {
            for (int i = 0; i < Width / Global.CellSize; i++)
                for (int j = 0; j < Height / Global.CellSize; j++)
                    surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), background[i,j], GraphicsUnit.Pixel);
        }

        void InitAction()
        {
            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize];

            hero = new Hero();
            heroCords = new Point(0, 0);
            action[0, 0] = hero;

            action[0, 2] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[1, 2] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[2, 0] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[2, 1] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[2, 2] = new Obstacle(ObstacleType.MOUNTAIN_1);
        }

        void DrawAction()
        {
            for (int i = 0; i < Width / Global.CellSize; i++)
                for (int j = 0; j < Height / Global.CellSize; j++)
                    if (action[i, j] != null)
                        surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), action[i, j].GetTile(), GraphicsUnit.Pixel);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            int destX= e.X / Global.CellSize,
                destY = e.Y / Global.CellSize,
                tmpX = heroCords.X,
                tmpY = heroCords.Y;
            bool heroMove = true;
            bool[,] moves = new bool[Width / Global.CellSize, Height / Global.CellSize];

            while (heroMove)
            {
                moves[tmpY, tmpX] = true;

                if (tmpX == destX && tmpY == destY)
                {
                    MoveHero(destX, destY);
                }
                else if (tmpY != destY)
                {
                    if (action[tmpY + 1, tmpX] == null && moves[tmpY + 1, tmpX] == false)
                    {
                        surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * tmpX, Global.CellSize * tmpY, Global.CellSize, Global.CellSize), new Rectangle(0, 128, Global.CellSize, Global.CellSize), GraphicsUnit.Pixel);
                        tmpY++;
                    }
                    else if (tmpX != 0 && action[tmpY, tmpX - 1] == null && moves[tmpY, tmpX - 1] == false)
                    {
                        surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * tmpX, Global.CellSize * tmpY, Global.CellSize, Global.CellSize), new Rectangle(32, 128, Global.CellSize, Global.CellSize), GraphicsUnit.Pixel);
                        tmpX--;
                    }
                    else if (tmpY != 0 && action[tmpY - 1, tmpX] == null && moves[tmpY - 1, tmpX] == false)
                    {
                        surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * tmpX, Global.CellSize * tmpY, Global.CellSize, Global.CellSize), new Rectangle(0, 96, Global.CellSize, Global.CellSize), GraphicsUnit.Pixel);
                        tmpY--;
                    }
                    else if (tmpX != Width / Global.CellSize && action[tmpY, tmpX + 1] == null)
                    {
                        surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * tmpX, Global.CellSize * tmpY, Global.CellSize, Global.CellSize), new Rectangle(32, 96, Global.CellSize, Global.CellSize), GraphicsUnit.Pixel);
                        tmpX++;
                    }
                    else
                    {
                        heroMove = false;
                    }
                }
                else
                {
                    heroMove = false; // TODO: remove
                }
            }
            
                        
            //if (heroMove)
            //    MoveHero(cellX, cellY);
        }

        void MoveHero(int x, int y)
        {
            action[heroCords.Y, heroCords.X] = null;
            surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * heroCords.Y, Global.CellSize * heroCords.X, Global.CellSize, Global.CellSize), background[heroCords.Y, heroCords.X], GraphicsUnit.Pixel);

            heroCords.X = x;
            heroCords.Y = y;

            action[heroCords.Y, heroCords.X] = hero;
            surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * heroCords.Y, Global.CellSize * heroCords.X, Global.CellSize, Global.CellSize), action[heroCords.Y, heroCords.X].GetTile(), GraphicsUnit.Pixel);

            DrawGrid();
        }
    }
}
