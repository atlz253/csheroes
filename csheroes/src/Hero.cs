using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    public class Hero : IGameObj
    {
        private static readonly Rectangle tile = new(928, 0, Global.CellSize, Global.CellSize);
        private readonly Army army;

        public Hero(Army army)
        {
            this.army = army;
        }

        internal Army Army => army;

        public Rectangle GetTile()
        {
            return tile;
        }
    }
}
