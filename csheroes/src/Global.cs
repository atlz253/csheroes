using System;
using System.Drawing;

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

    internal static class Global
    {
        private static readonly int cellSize = 32,
                            battleCellSize = 50;
        private static readonly Pen gridPen = new(Color.Black, 1),
                            highlightPen = new(Color.Red, 1);
        private static readonly SolidBrush moveHighlightBrush = new(Color.FromArgb(70, 174, 198, 207)),
                                   enemyHighlightBrush = new(Color.FromArgb(70, 219, 88, 86));
        private static readonly Random rand = new();
#if RELEASE
        static readonly Image texture = Image.FromFile("textures.png");
#else
        private static readonly Image texture = Image.FromFile("../../../res/textures.png");
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

        public static Random Rand => rand;

        public static int BattleCellSize => battleCellSize;

        public static SolidBrush MoveHighlightBrush => moveHighlightBrush;

        public static SolidBrush EnemyHighlightBrush => enemyHighlightBrush;
    }
}
