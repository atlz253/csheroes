using System.Drawing;

namespace csheroes.src.Textures
{
    public class Tile
    {
        private Point tilePosition;

        public Tile(int x, int y)
        {
            tilePosition = new Point(x, y);
        }

        public static Tile GetRandomTile(bool hero)
        {
            if (hero)
            {
                if (Global.Rand.Next(0, 2) == 1)
                {
                    return new(256 + Global.CellSize * Global.Rand.Next(0, 4), 0);
                }
                else
                {
                    return new(128 + 32 * Global.Rand.Next(0, 9), 128);
                }
            }
            else
            {
                return new(256 + Global.CellSize * Global.Rand.Next(0, 4), 0);
            }
        }

        public int X => tilePosition.X;
        public int Y => tilePosition.Y;

        public Rectangle Area => new(tilePosition.X, tilePosition.Y, Global.CellSize, Global.CellSize);
    }
}
