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

        public Rectangle GetTile()
        {
            return Units[0].GetTile();
        }
    }
}
