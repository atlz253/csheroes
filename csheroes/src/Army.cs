using csheroes.src.unit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    public class ArmyShapshot
    {
        public bool ai;
        public UnitSnapshot[] units;

        public ArmyShapshot(bool ai, UnitSnapshot[] units)
        {
            this.ai = ai;
            this.units = units;
        }
    }

    public class Army : IGameObj
    {
        Unit[] units;
        readonly bool ai;

        public Army(bool ai = true, params Unit[] units)
        {
            this.ai = ai;
            this.units = new Unit[7];

            for (int i = 0; i < units.Length && i < 7; i++)
                this.units[i] = units[i];
        }

        public Army(ArmyShapshot snapshot)
        {
            ai = snapshot.ai;
            units = new Unit[7];

            for (int i = 0; i < 7; i++)
                if (snapshot.units[i] != null)
                    units[i] = new Unit(snapshot.units[i]);
        }

        public Unit[] Units { get => units; set => units = value; }

        public bool Empty
        {
            get
            {
                foreach (Unit unit in units)
                    if (unit != null)
                        return false;
                return true;
            }
        }

        public bool Ai => ai;

        public Rectangle GetTile()
        {
            return Units[0].GetTile();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(ToString());

            for (int i = 0; i < 7; i++)
                if (units[i] != null)
                    units[i].Save(writer);
                else
                    writer.Write("NoUnit");
        }

        public override string ToString()
        {
            return "Army";
        }

        public ArmyShapshot MakeSnapshot()
        {
            UnitSnapshot[] unitstate = new UnitSnapshot[7];
            for (int i = 0; i < 7; i++)
                if (units[i] != null)
                    unitstate[i] = units[i].MakeSnaphot();

            return new(ai, unitstate);
        }
    }
}
