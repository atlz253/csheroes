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
        CREEP_RANGE,
        WEAK,
        PHYSIC_MELEE,
        PHYSIC_RANGE,
        ECONOMIST,
        STALKER_1,
        STALKER_2,
        PHILOSOPH_STRONG,
        PHILOSOPH_FAST,
        PHILOSOPH_BALANCED,
        PHILOSOPH_TENACIOUS,
        PHILOSOPH_RANGE,
        MATRIX_FAST,
        MATRIX_STRONG,
        MATRIX_BALANCED,
        ANONIMUS,
        BITARD,
        SUSLOV,
        KERNEL_PANIC,
        ERROR,
        BUG,
        HACKER_JUNIOR,
        HACKER_MIDDLE,
        HACKER_SENIOR,
        HACKER_FAST,
        SQUID_EASY,
        SQUID_MEDIUM,
        SQUID_HARD,
        SQUID_RANGE,
        VAXTER,
        OXRANA,

    }

    public class UnitSnapshot : ISnapshot
    {
        public readonly Point tile;
        public readonly AttackType type;

        public readonly int hp, exp, maxHp, range, damage, level, nextLevel;

        public UnitSnapshot(int hp, int maxHp, int exp, int range, int damage, int level, int nextLevel, Point tile, AttackType type)
        {
            this.tile = tile;
            this.type = type;

            this.hp = hp;
            this.maxHp = maxHp;
            this.exp = exp;
            this.range = range;
            this.damage = damage;

            this.level = level;
            this.nextLevel = nextLevel;
        }

        public UnitSnapshot(BinaryReader reader)
        {
            tile = new Point(reader.ReadInt32(), reader.ReadInt32());

            switch (reader.ReadString())
            {
                case "MELEE":
                    type = AttackType.MELEE;
                    break;
                case "RANGE":
                    type = AttackType.RANGE;
                    break;
            }

            hp = reader.ReadInt32();
            maxHp = reader.ReadInt32();
            exp = reader.ReadInt32();
            range = reader.ReadInt32();
            damage = reader.ReadInt32();
            level = reader.ReadInt32();
            nextLevel = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write("Unit");
            writer.Write(tile.X);
            writer.Write(tile.Y);
            writer.Write(type.ToString());
            writer.Write(hp);
            writer.Write(maxHp);
            writer.Write(exp);
            writer.Write(range);
            writer.Write(damage);
            writer.Write(level);
            writer.Write(nextLevel);
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
                    level = 1;
                    break;
                case UnitTemplate.CREEP_RANGE:
                    tile = new Point(256 + Global.Rand.Next(0, 2) * Global.CellSize, 32);
                    type = AttackType.RANGE;
                    maxHp = 3;
                    range = 3;
                    damage = 1;
                    level = 1;
                    break;
                case UnitTemplate.WEAK:
                    maxHp = 5;
                    range = 3;
                    damage = 1;
                    level = 1;
                    break;
                case UnitTemplate.PHYSIC_MELEE:
                    tile = new Point(224, 64);
                    maxHp = 10;
                    range = 5;
                    damage = 1;
                    level = 3;
                    break;
                case UnitTemplate.PHYSIC_RANGE:
                    tile = new Point(256, 64);
                    type = AttackType.RANGE;
                    maxHp = 3;
                    range = 3;
                    damage = 1;
                    level = 3;
                    break;
                case UnitTemplate.ECONOMIST:
                    tile = new Point(288 + Global.Rand.Next(0, 3) * Global.CellSize, 64);
                    maxHp = 10;
                    range = 3;
                    damage = 2;
                    level = 3;
                    break;
                case UnitTemplate.STALKER_1:
                    tile = new Point(384, 64);
                    maxHp = 5;
                    range = 3;
                    damage = 2;
                    level = 2;
                    break;
                case UnitTemplate.STALKER_2:
                    tile = new Point(416, 64);
                    maxHp = 5;
                    range = 3;
                    damage = 2;
                    level = 2;
                    break;
                case UnitTemplate.PHILOSOPH_BALANCED:
                    tile = new Point(704, 64);
                    maxHp = 10;
                    range = 3;
                    damage = 2;
                    level = 3;
                    break;
                case UnitTemplate.PHILOSOPH_FAST:
                    tile = new Point(672, 64);
                    maxHp = 3;
                    range = 6;
                    damage = 1;
                    level = 1;
                    break;
                case UnitTemplate.PHILOSOPH_RANGE:
                    tile = new Point(768, 64);
                    type = AttackType.RANGE;
                    maxHp = 6;
                    range = 3;
                    damage = 2;
                    level = 2;
                    break;
                case UnitTemplate.PHILOSOPH_STRONG:
                    tile = new Point(736, 64);
                    maxHp = 10;
                    range = 1;
                    damage = 4;
                    level = 5;
                    break;
                case UnitTemplate.PHILOSOPH_TENACIOUS:
                    tile = new Point(640, 64);
                    maxHp = 30;
                    range = 2;
                    damage = 1;
                    level = 4;
                    break;
                case UnitTemplate.MATRIX_FAST:
                    tile = new Point(128, 64);
                    maxHp = 20;
                    range = 6;
                    damage = 2;
                    level = 4;
                    break;
                case UnitTemplate.MATRIX_STRONG:
                    tile = new Point(160, 64);
                    maxHp = 20;
                    range = 2;
                    damage = 4;
                    level = 4;
                    break;
                case UnitTemplate.MATRIX_BALANCED:
                    tile = new Point(192, 64);
                    maxHp = 15;
                    range = 3;
                    damage = 3;
                    level = 4;
                    break;
                case UnitTemplate.ANONIMUS:
                    tile = new Point(448, 64);
                    maxHp = 70;
                    range = 3;
                    damage = 4;
                    level = 6;
                    break;
                case UnitTemplate.BITARD:
                    tile = new Point(480, 64);
                    maxHp = 10;
                    range = 5;
                    damage = 1;
                    level = 4;
                    break;
                case UnitTemplate.SUSLOV:
                    tile = new Point(800, 64);
                    maxHp = 100;
                    range = 1;
                    damage = 2;
                    level = 10;
                    break;
                case UnitTemplate.KERNEL_PANIC:
                    tile = new Point(832, 64);
                    maxHp = 10;
                    range = 7;
                    damage = 1;
                    level = 3;
                    break;
                case UnitTemplate.ERROR:
                    tile = new Point(864, 64);
                    maxHp = 10;
                    range = 5;
                    damage = 1;
                    level = 3;
                    break;
                case UnitTemplate.BUG:
                    tile = new Point(896, 64);
                    maxHp = 10;
                    range = 5;
                    damage = 2;
                    level = 3;
                    break;
                case UnitTemplate.HACKER_JUNIOR:
                    tile = new Point(128, 96);
                    maxHp = 10;
                    range = 3;
                    damage = 2;
                    level = 3;
                    break;
                case UnitTemplate.HACKER_MIDDLE:
                    tile = new Point(160, 96);
                    maxHp = 15;
                    range = 3;
                    damage = 2;
                    level = 4;
                    break;
                case UnitTemplate.HACKER_SENIOR:
                    tile = new Point(192, 96);
                    maxHp = 20;
                    range = 3;
                    damage = 3;
                    level = 5;
                    break;
                case UnitTemplate.HACKER_FAST:
                    tile = new Point(224, 96);
                    maxHp = 10;
                    range = 5;
                    damage = 2;
                    level = 5;
                    break;
                case UnitTemplate.SQUID_EASY:
                    tile = new Point(256, 96);
                    maxHp = 30;
                    range = 3;
                    damage = 2;
                    level = 5;
                    break;
                case UnitTemplate.SQUID_MEDIUM:
                    tile = new Point(288, 96);
                    maxHp = 50;
                    range = 3;
                    damage = 2;
                    level = 5;
                    break;
                case UnitTemplate.SQUID_HARD:
                    tile = new Point(320, 96);
                    maxHp = 50;
                    range = 3;
                    damage = 3;
                    level = 5;
                    break;
                case UnitTemplate.SQUID_RANGE:
                    tile = new Point(320, 96);
                    type = AttackType.RANGE;
                    maxHp = 20;
                    range = 3;
                    damage = 2;
                    level = 5;
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
            this.type = type;
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
            level = snapshot.level;
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

        public ISnapshot MakeSnapshot()
        {
            return new UnitSnapshot(hp, maxHp, exp, range, damage, level, nextLevelExp, tile, type);
        }

        public override string ToString()
        {
            return "Unit";
        }
    }
}
