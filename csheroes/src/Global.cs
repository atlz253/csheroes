using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    public enum Arrows
    {
        EMPTY,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    static class Global
    {
        static readonly int cellSize = 32,
                            battleCellSize = 50;

        static readonly Pen gridPen = new(Color.Black, 1),
                            highlightPen = new(Color.Red, 1);

        static readonly Random rand = new();
#if RELEASE
        static readonly Image texture = Image.FromFile("textures.png");
#else
        static readonly Image texture = Image.FromFile("../../../res/textures.png");
#endif

        public static int CellSize
        {
            get
            {
                return cellSize;
            }
        }

        public static Pen GridPen => gridPen;
        public static Pen HighlightPen => highlightPen;

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

        public static int BattleCellSize => battleCellSize;
    }
}
