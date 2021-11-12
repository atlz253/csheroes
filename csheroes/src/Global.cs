using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    static class Global
    {
        static int cellSize = 32;

        static Pen gridPen = new Pen(Color.Black, 1);

        static Random rand = new Random();
        static Image texture = Image.FromFile("../../../res/textures.png");

        public static int CellSize
        {
            get
            {
                return cellSize;
            }
        }

        public static Pen GridPen
        {
            get
            {
                return gridPen;
            }
        }

        public static Image Texture
        {
            get
            {
                return texture;
            }
        }

        public static Random Rand
        {
            get
            {
                return rand;
            }
        }
    }
}
