using csheroes.src.Textures;
using csheroes.src.Units;
using System.Drawing;
using System.IO;

namespace csheroes.src
{
    public class ArmyShapshot : ISnapshot
    {
        public bool ai;
        public ISnapshot[] units;

        public ArmyShapshot(bool ai, ISnapshot[] units)
        {
            this.ai = ai;
            this.units = units;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write("Army");
            writer.Write(ai);

            for (int i = 0; i < 7; i++)
            {
                if (units[i] != null)
                {
                    units[i].Save(writer);
                }
                else
                {
                    writer.Write("NoUnit");
                }
            }
        }
    }

    public class Army : IGameObj
    {
        private Unit[] units;
        private readonly bool ai;

        public Army(bool ai = true, params Unit[] units)
        {
            this.ai = ai;
            this.units = new Unit[7];

            for (int i = 0; i < units.Length && i < 7; i++)
            {
                this.units[i] = units[i];
            }
        }

        public Army(ArmyShapshot snapshot)
        {
            ai = snapshot.ai;
            units = new Unit[7];

            for (int i = 0; i < 7; i++)
            {
                if (snapshot.units[i] != null)
                {
                    units[i] = new Unit((UnitSnapshot)snapshot.units[i]);
                }
            }
        }

        public Unit[] Units { get => units; set => units = value; }

        public bool Empty
        {
            get
            {
                foreach (Unit unit in units)
                {
                    if (unit != null && unit.Hp != 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool Ai => ai;

        public Tile Tile => Units[0].Tile;

        public ISnapshot MakeSnapshot()
        {
            ISnapshot[] unitstate = new ISnapshot[7];

            for (int i = 0; i < 7; i++)
            {
                if (units[i] != null)
                {
                    unitstate[i] = units[i].MakeSnapshot();
                }
            }

            return new ArmyShapshot(ai, unitstate);
        }

        public override string ToString()
        {
            return "Army";
        }
    }
}
