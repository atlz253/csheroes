using csheroes.src.Saves;
using csheroes.src.Units;
using System.Drawing;
using System.IO;

namespace csheroes.src
{
    public class ExploreMap: ISaveable
    {
        private readonly int width;
        private readonly int height;
        
        public IGameObj[,] action;
        public Rectangle[,] background;

        public Hero hero;
        public Point heroCords;

        public Point winCell;

        public Rectangle battleMapBackgroundTile; // the background for the battle is the only one on the whole explore map

        public string locationName;

        public int Width => width;
        public int Height => height;

        public ExploreMap(int width, int height)
        {
            this.width = width;
            this.height = height;

            background = new Rectangle[width, height];
        }

        public void WriteSave(BinaryWriter writer)
        {
            writer.Write(locationName);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Rectangle backgroundCell = background[i, j];

                    writer.Write(backgroundCell.X);
                    writer.Write(backgroundCell.Y);
                }  
            }

            writer.Write(battleMapBackgroundTile.X);
            writer.Write(battleMapBackgroundTile.Y);
            writer.Write(winCell.X);
            writer.Write(winCell.Y);

            ISnapshot[,] actionstate = new ISnapshot[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (action[i, j] != null)
                    {
                        action[i, j].MakeSnapshot().Save(writer);
                    }
                    else
                    {
                        writer.Write("NullObj");
                    }
                }
            }
        }

        public void ReadSave(BinaryReader reader)
        {
            action = new IGameObj[width, height];

            locationName = reader.ReadString();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    background[i, j] = new(reader.ReadInt32(), reader.ReadInt32(), Global.CellSize, Global.CellSize);
                }
            }

            battleMapBackgroundTile = new(reader.ReadInt32(), reader.ReadInt32(), Global.CellSize, Global.CellSize);
            winCell = new(reader.ReadInt32(), reader.ReadInt32());

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    string name = reader.ReadString();

                    if (name == "NullObj")
                    {
                        continue;
                    }
                    else if (name == "Obstacle")
                    {
                        action[i, j] = new Obstacle(reader.ReadInt32(), reader.ReadInt32());
                    }
                    else if (name == "Hero")
                    {
                        int respect = reader.ReadInt32();

                        reader.ReadString(); // считываем строку "Army"
                        bool ai = reader.ReadBoolean();
                        Unit[] units = new Unit[7];
                        for (int k = 0; k < 7; k++)
                        {
                            string unitName = reader.ReadString();

                            if (unitName == "NoUnit")
                            {
                                continue;
                            }

                            Unit unit = new(new UnitSnapshot(reader));
                            units[k] = unit;
                        }

                        hero = new Hero(new Army(ai, units), respect);
                        action[i, j] = hero;
                        heroCords = new Point(j, i);
                    }
                    else if (name == "Army")
                    {
                        bool ai = reader.ReadBoolean();
                        Unit[] units = new Unit[7];
                        for (int k = 0; k < 7; k++)
                        {
                            string unitName = reader.ReadString();

                            if (unitName == "NoUnit")
                            {
                                continue;
                            }

                            Unit unit = new(new UnitSnapshot(reader));
                            units[k] = unit;
                        }

                        action[i, j] = new Army(ai, units);
                    }
                }
            }
        }
    }
}
