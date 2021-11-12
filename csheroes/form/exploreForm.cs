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
        const int cellSize = 32;
        
        static Random rand = new Random();
        static Pen gridPen = new Pen(Color.Black, 1);
        static Image texture = Image.FromFile("../../../res/textures.png");

        Graphics surface;

        public ExploreForm()
        {
            InitializeComponent();

            surface = CreateGraphics();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            surface.Clear(Color.White);

            DrawBackground();
            DrawGrid();
        }

        public void DrawGrid()
        {
            for (int i = 0; i < Width / cellSize; i++)
                surface.DrawLine(gridPen, cellSize * i, 0, cellSize * i, Height);

            for (int i = 0; i < Height / cellSize; i++)
                surface.DrawLine(gridPen, 0, cellSize * i, Width, cellSize * i);
        }

        public void DrawBackground()
        {

            for (int i = 0; i < Width / cellSize; i++)
                for (int j = 0; j < Height / cellSize; j++) // TODO: массив tile
                    surface.DrawImage(texture, new Rectangle(cellSize * j, cellSize * i, cellSize, cellSize), new Rectangle(cellSize * rand.Next(0, 2), cellSize * rand.Next(0, 2), cellSize, cellSize), GraphicsUnit.Pixel);

        }
    }
}
