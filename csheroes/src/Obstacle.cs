using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    enum ObstacleType
    {
        MOUNTAIN_1
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
            }
        }

        public Rectangle GetTile()
        {
            return tile;
        }
    }
}
