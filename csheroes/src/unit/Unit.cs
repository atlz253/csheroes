using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src.unit
{
    public enum UnitType
    {
        ABBITURENT
    }

    public class Unit : IGameObj
    {
        readonly IAttack attack;
        readonly Rectangle tile;

        int hp;
        readonly int range;
        readonly int damage;

        public Unit(UnitType type)
        {
            switch (type)
            {
                case UnitType.ABBITURENT:
                    tile = new Rectangle(256, 0, Global.CellSize, Global.CellSize);
                    attack = new MeleeAttack();
                    Hp = 3;
                    range = 3;
                    damage = 1;
                    break;
            }
        }

        internal IAttack Attack { get => attack; }

        public int Range => range;

        public int Damage => damage;

        public int Hp { get => hp; set => hp = value; }

        public Rectangle GetTile()
        {
            return tile;
        }
    }
}
