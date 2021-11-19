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
    public class Army : IGameObj
    {
        Unit[] units;
        readonly bool ai;

        public Army(bool ai = true, params Unit[] units)
        {
            this.ai = ai;
            this.Units = new Unit[7];

            for (int i = 0; i < units.Length && i < 7; i++)
                this.Units[i] = units[i];
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
    }
}
