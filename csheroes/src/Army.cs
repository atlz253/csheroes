using csheroes.src.unit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    public class Army : IGameObj
    {
        Unit[] units;

        public Army(params Unit[] units)
        {
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

        public Rectangle GetTile()
        {
            return Units[0].GetTile();
        }
    }
}
