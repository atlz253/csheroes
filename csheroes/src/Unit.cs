using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src.unit
{
    public enum UnitType
    {
        ABBITURENT,
        TECHNAR,
        GUMANITARIY
    }

    public enum AttackType
    {
        MELEE,
        RANGE
    }

    public class Unit : IGameObj
    {
        readonly AttackType type;
        readonly Rectangle tile;

        private int hp, exp = 0;
        readonly int range;
        readonly int damage;
        private readonly int maxHp, nextLevel;
        readonly string name;

        public Unit(UnitType type)
        {
            switch (type)
            {
                case UnitType.ABBITURENT:
                    tile = new Rectangle(256, 0, Global.CellSize, Global.CellSize);
                    this.type = AttackType.MELEE;
                    maxHp = 3;
                    Hp = 3;
                    range = 3;
                    damage = 1;
                    nextLevel = 1;
                    name = "Абитурент";
                    break;
                case UnitType.TECHNAR:
                    tile = new Rectangle(320, 0, Global.CellSize, Global.CellSize);
                    this.type = AttackType.RANGE; // TODO: дальний бой
                    maxHp = 3;
                    Hp = 3;
                    range = 3;
                    damage = 2;
                    nextLevel = 2;
                    name = "Технарь";
                    break;
                case UnitType.GUMANITARIY:
                    tile = new Rectangle(288, 0, Global.CellSize, Global.CellSize);
                    this.type = AttackType.MELEE;
                    maxHp = 5;
                    Hp = 5;
                    range = 5;
                    damage = 3;
                    nextLevel = 2;
                    name = "Гуманитарий";
                    break;
            }
        }

        internal AttackType Attack { get => type; }

        public int Range => range;

        public int Damage => damage;

        public int Hp { get => hp; set => hp = value; }

        public int MaxHp => maxHp;

        public int Exp { get => exp; set => exp = value; }

        public int NextLevel => nextLevel;

        public Rectangle GetTile()
        {
            return tile;
        }

        public override string ToString()
        {
            return name;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(name);
            writer.Write(hp);
            writer.Write(exp);
        }
    }
}
