using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src.unit
{
    public enum AttackType
    {
        MELEE,
        RANGE
    }

    public enum UnitStats
    {
        HP,
        RANGE,
        DAMAGE
    }

    public enum UnitTemplate
    {
        CREEP,
        PHYSIC_MELEE,
        PHYSIC_RANGE,
        ECONOMIST,
        STALKER_1,
        STALKER_2
    }

    public class UnitSnapshot
    {
        public readonly Point tile;
        public readonly AttackType type;

        public readonly int hp, exp, maxHp, range, damage, nextLevel;

        public UnitSnapshot(int hp, int maxHp, int exp, int range, int damage, int nextLevel, Point tile, AttackType type)
        {
            this.tile = tile;
            this.type = type;

            this.hp = hp;
            this.maxHp = maxHp;
            this.exp = exp;
            this.range = range;
            this.damage = damage;

            this.nextLevel = nextLevel;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write("Unit");
        }
    }

    public class Unit : IGameObj
    {
        Point tile;
        AttackType type = AttackType.MELEE;

        int hp = 10;
        int exp = 0;
        int maxHp = 10;
        int range = 3;
        int level = 1;
        int damage = 1;
        int nextLevelExp = 1;

        public Unit()
        {
            tile = RandomTile();
        }

        public Unit(UnitTemplate template) : this()
        {
            switch (template)
            {
                case UnitTemplate.CREEP:
                    maxHp = 3;
                    range = 3;
                    damage = 1;
                    break;
                case UnitTemplate.PHYSIC_MELEE:
                    tile = new Point(224, 64);
                    maxHp = 10;
                    range = 5;
                    damage = 1;
                    break;
                case UnitTemplate.PHYSIC_RANGE:
                    tile = new Point(256, 64);
                    type = AttackType.RANGE;
                    maxHp = 3;
                    range = 3;
                    damage = 1;
                    break;
                case UnitTemplate.ECONOMIST:
                    tile = new Point(288 + Global.Rand.Next(0, 3) * Global.CellSize, 64);
                    maxHp = 10;
                    range = 3;
                    damage = 3;
                    break;
                case UnitTemplate.STALKER_1:
                    tile = new Point(384, 64);
                    maxHp = 5;
                    range = 3;
                    damage = 2;
                    break;
                case UnitTemplate.STALKER_2:
                    tile = new Point(416, 64);
                    maxHp = 5;
                    range = 3;
                    damage = 2;
                    break;
            }

            hp = maxHp;
        }

        public Unit(int hp, int range, int damage, AttackType type = AttackType.MELEE) : this()
        {
            maxHp = hp;
            this.hp = hp;
            this.range = range;
            this.damage = damage;
        }

        public Unit(UnitSnapshot snapshot)
        {
            tile = snapshot.tile;
            type = snapshot.type;
            maxHp = snapshot.maxHp;
            hp = snapshot.hp;
            range = snapshot.range;
            damage = snapshot.damage;
            exp = snapshot.exp;
            nextLevelExp = snapshot.nextLevel;
        }

        Point RandomTile()
        {
            return new(256 + Global.CellSize * Global.Rand.Next(0, 4), 0);
        }

        internal AttackType Attack { get => type; set => type = value; }

        public int Range { get => range; set => range = value; }

        public int Damage { get => damage; set => damage = value; }

        public int Hp { get => hp; set => hp = value; }

        public int MaxHp { get => maxHp; set => maxHp = value; }

        public int Exp { get => exp; set => exp = value; }

        public int NextLevel { get => nextLevelExp; set => nextLevelExp = value; }

        public int Level { get => level; set => level = value; }

        public Rectangle GetTile()
        {
            return new(tile.X, tile.Y, Global.CellSize, Global.CellSize);
        }

        public void Save(BinaryWriter writer) // TODO: remove
        {
        }

        public UnitSnapshot MakeSnaphot()
        {
            return new(hp, maxHp, exp, range, damage, nextLevelExp, tile, type);
        }
    }
}
