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

        private void OnPaint(object sender, PaintEventArgs e)
        {
            surface.Clear(Color.White);

            DrawBackground();
            DrawAction();
            DrawGrid();
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
            int cellX = e.X / Global.CellSize,
                cellY = e.Y / Global.CellSize;

            action[cellY, cellX] = new Hero();

            DrawAction();
        }
    }
}
