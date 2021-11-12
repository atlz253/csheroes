using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    class Hero : IGameObj
    {
        private static readonly Rectangle heroTile = new(928, 0, Global.CellSize, Global.CellSize);

        public Hero()
        {

        }

        public Rectangle GetTile()
        {
            return heroTile;
        }
    }
}
