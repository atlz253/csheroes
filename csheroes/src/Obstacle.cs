using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    enum ObstacleType
    {
        MOUNTAIN_1,
        NEW_KORPUS_WALL
    }

    class Obstacle : IGameObj
    {
        private Rectangle tile;

        public Obstacle(ObstacleType type)
        {
            switch (type)
            {
                case ObstacleType.MOUNTAIN_1:
                    tile = new(96, 0, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.NEW_KORPUS_WALL:
                    tile = new(0, 192, Global.CellSize, Global.CellSize);
                    break;
            }
        }

        public Obstacle(int tileX, int tileY)
        {
            tile = new Rectangle(tileX, tileY, Global.CellSize, Global.CellSize);
        }

        public Rectangle GetTile()
        {
            return tile;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(ToString());
            writer.Write(tile.X);
            writer.Write(tile.Y);
        }

        public override string ToString()
        {
            return "Obstacle";
        }
    }
}
