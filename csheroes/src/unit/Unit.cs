using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src.unit
{
    enum UnitType
    {
        ABBITURENT
    }

    class Unit : IGameObj
    {
        readonly IAttack attack;
        readonly Rectangle tile;

        public Unit(UnitType type)
        {
            switch (type)
            {
                case UnitType.ABBITURENT:
                    tile = new Rectangle(256, 0, Global.CellSize, Global.CellSize);
                    attack = new MeleeAttack();
                    break;
            }
        }

        internal IAttack Attack { get => attack; }

        public Rectangle GetTile()
        {
            return tile;
        }
    }
}
