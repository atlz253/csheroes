﻿using System.Drawing;
using System.IO;

namespace csheroes.src
{
    internal enum ObstacleType
    {
        NEW_KORPUS_WALL,
        MAC_WALL,
        MAC_TABLE_1,
        MAC_TABLE_2,
        MAC_TABLE_3,
        MAC_TABLE_4,
        MAC_TABLE_5,
        MAC_TABLE_6,
        MAC_SOFA,
        MAC_CHAIR_1,
        MAC_CHAIR_2,
        MAC_CHAIR_3,
        MAC_CHAIR_4,
        MAC_BARRIER,
        MAC_WORK_TABLE
    }

    internal class ObstacleSnapshot : ISnapshot
    {
        public readonly Point tile;

        public ObstacleSnapshot(Rectangle tile)
        {
            this.tile = new(tile.X, tile.Y);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write("Obstacle");
            writer.Write(tile.X);
            writer.Write(tile.Y);
        }
    }

    internal class Obstacle : IGameObj
    {
        private Rectangle tile;

        public Obstacle(ObstacleType type)
        {
            switch (type)
            {
                case ObstacleType.NEW_KORPUS_WALL:
                    tile = new(0, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_WALL:
                    tile = new(32, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_TABLE_1:
                    tile = new(64, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_TABLE_2:
                    tile = new(96, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_TABLE_3:
                    tile = new(128, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_TABLE_4:
                    tile = new(160, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_SOFA:
                    tile = new(192, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_CHAIR_1:
                    tile = new(224, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_CHAIR_2:
                    tile = new(256, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_CHAIR_3:
                    tile = new(288, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_CHAIR_4:
                    tile = new(320, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_TABLE_5:
                    tile = new(352, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_TABLE_6:
                    tile = new(384, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_BARRIER:
                    tile = new(416, 192, Global.CellSize, Global.CellSize);
                    break;
                case ObstacleType.MAC_WORK_TABLE:
                    tile = new(448, 192, Global.CellSize, Global.CellSize);
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

        public ISnapshot MakeSnapshot()
        {
            return new ObstacleSnapshot(tile);
        }

        public override string ToString() => "Obstacle";
    }
}
