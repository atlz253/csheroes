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

        private int hp;
        readonly int range;
        readonly int damage;
        private readonly int maxHp;

        public Unit(UnitType type)
        {
            switch (type)
            {
                case UnitType.ABBITURENT:
                    tile = new Rectangle(256, 0, Global.CellSize, Global.CellSize);
                    attack = new MeleeAttack();
                    maxHp = 3;
                    Hp = 1;
                    range = 10;
                    damage = 1;
                    break;
            }
        }

        internal IAttack Attack { get => attack; }

        public int Range => range;

        public int Damage => damage;

        public int Hp { get => hp; set => hp = value; }

        public int MaxHp => maxHp;

        public Rectangle GetTile()
        {
            return tile;
        }
    }
}
