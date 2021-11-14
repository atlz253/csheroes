using csheroes.src.unit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    class Army : IGameObj
    {
        Unit[] units;

        public Army(params Unit[] units)
        {
            this.units = new Unit[7];

            for (int i = 0; i < units.Length && i < 7; i++)
                this.units[i] = units[i];
        }

        public Rectangle GetTile()
        {
            return units[0].GetTile();
        }
    }
}
