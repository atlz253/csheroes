using csheroes.src.Textures;
using System;
using System.Drawing;
using System.IO;

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
        NORMAL,
        HARD,
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
        MAC_WORKER,
        RONALD,
        BOXER_ALI,
        BOXER_ROCKY,
        BOXER_STUDENT,
        COSMONAUT,
        COSMONAUT_RANGE,
        SOLDIER_RANGE,
        JULIA_S,
        FEDOR,
        MIGAS,
        VANYA,
        DIMA,
        JULIA_B,
        MISHA
    }

    public class UnitSnapshot : ISnapshot
    {
        public readonly Tile tile;
        public readonly AttackType type;

        public readonly int hp, exp, maxHp, range, damage, level, nextLevel;

        public UnitSnapshot(int hp, int maxHp, int exp, int range, int damage, int level, int nextLevel, Tile tile, AttackType type)
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
            tile = new Tile(reader.ReadInt32(), reader.ReadInt32());

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
        private Tile tile;
        private AttackType type = AttackType.MELEE;
        private int hp = 10;
        private int exp = 0;
        private int maxHp = 10;
        private int range = 3;
        private int level = 1;
        private int damage = 1;
        private int nextLevelExp = 1;

        public Unit(bool hero = false)
        {
            tile = Tile.GetRandomTile(hero);
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
                    tile = new Tile(256 + Global.Rand.Next(0, 2) * Global.CellSize, 32);
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
                case UnitTemplate.NORMAL:
                    maxHp = 7;
                    range = 3;
                    damage = 2;
                    level = 1;
                    break;
                case UnitTemplate.HARD:
                    maxHp = 10;
                    range = 3;
                    damage = 3;
                    level = 2;
                    break;
                case UnitTemplate.PHYSIC_MELEE:
                    tile = new Tile(224, 64);
                    maxHp = 10;
                    range = 5;
                    damage = 1;
                    level = 3;
                    break;
                case UnitTemplate.PHYSIC_RANGE:
                    tile = new Tile(256, 64);
                    type = AttackType.RANGE;
                    maxHp = 3;
                    range = 3;
                    damage = 1;
                    level = 3;
                    break;
                case UnitTemplate.ECONOMIST:
                    tile = new Tile(288 + Global.Rand.Next(0, 3) * Global.CellSize, 64);
                    maxHp = 10;
                    range = 3;
                    damage = 2;
                    level = 3;
                    break;
                case UnitTemplate.STALKER_1:
                    tile = new Tile(384, 64);
                    maxHp = 5;
                    range = 3;
                    damage = 2;
                    level = 2;
                    break;
                case UnitTemplate.STALKER_2:
                    tile = new Tile(416, 64);
                    maxHp = 5;
                    range = 3;
                    damage = 2;
                    level = 2;
                    break;
                case UnitTemplate.PHILOSOPH_BALANCED:
                    tile = new Tile(704, 64);
                    maxHp = 10;
                    range = 3;
                    damage = 2;
                    level = 3;
                    break;
                case UnitTemplate.PHILOSOPH_FAST:
                    tile = new Tile(672, 64);
                    maxHp = 3;
                    range = 6;
                    damage = 1;
                    level = 1;
                    break;
                case UnitTemplate.PHILOSOPH_RANGE:
                    tile = new Tile(768, 64);
                    type = AttackType.RANGE;
                    maxHp = 6;
                    range = 3;
                    damage = 2;
                    level = 2;
                    break;
                case UnitTemplate.PHILOSOPH_STRONG:
                    tile = new Tile(736, 64);
                    maxHp = 10;
                    range = 1;
                    damage = 4;
                    level = 5;
                    break;
                case UnitTemplate.PHILOSOPH_TENACIOUS:
                    tile = new Tile(640, 64);
                    maxHp = 30;
                    range = 2;
                    damage = 1;
                    level = 4;
                    break;
                case UnitTemplate.MATRIX_FAST:
                    tile = new Tile(128, 64);
                    maxHp = 20;
                    range = 6;
                    damage = 2;
                    level = 4;
                    break;
                case UnitTemplate.MATRIX_STRONG:
                    tile = new Tile(160, 64);
                    maxHp = 20;
                    range = 2;
                    damage = 4;
                    level = 4;
                    break;
                case UnitTemplate.MATRIX_BALANCED:
                    tile = new Tile(192, 64);
                    maxHp = 15;
                    range = 3;
                    damage = 3;
                    level = 4;
                    break;
                case UnitTemplate.ANONIMUS:
                    tile = new Tile(448, 64);
                    maxHp = 50;
                    range = 3;
                    damage = 3;
                    level = 6;
                    break;
                case UnitTemplate.BITARD:
                    tile = new Tile(480, 64);
                    maxHp = 5;
                    range = 6;
                    damage = 1;
                    level = 4;
                    break;
                case UnitTemplate.SUSLOV:
                    tile = new Tile(800, 64);
                    maxHp = 100;
                    range = 1;
                    damage = 2;
                    level = 10;
                    break;
                case UnitTemplate.KERNEL_PANIC:
                    tile = new Tile(832, 64);
                    maxHp = 10;
                    range = 7;
                    damage = 1;
                    level = 3;
                    break;
                case UnitTemplate.ERROR:
                    tile = new Tile(864, 64);
                    maxHp = 10;
                    range = 5;
                    damage = 1;
                    level = 3;
                    break;
                case UnitTemplate.BUG:
                    tile = new Tile(896, 64);
                    maxHp = 10;
                    range = 5;
                    damage = 2;
                    level = 3;
                    break;
                case UnitTemplate.HACKER_JUNIOR:
                    tile = new Tile(128, 96);
                    maxHp = 8;
                    range = 3;
                    damage = 2;
                    level = 3;
                    break;
                case UnitTemplate.HACKER_MIDDLE:
                    tile = new Tile(160, 96);
                    maxHp = 12;
                    range = 2;
                    damage = 2;
                    level = 4;
                    break;
                case UnitTemplate.HACKER_SENIOR:
                    tile = new Tile(192, 96);
                    maxHp = 15;
                    range = 2;
                    damage = 3;
                    level = 5;
                    break;
                case UnitTemplate.HACKER_FAST:
                    tile = new Tile(224, 96);
                    maxHp = 10;
                    range = 5;
                    damage = 2;
                    level = 5;
                    break;
                case UnitTemplate.SQUID_EASY:
                    tile = new Tile(256, 96);
                    maxHp = 30;
                    range = 3;
                    damage = 2;
                    level = 5;
                    break;
                case UnitTemplate.SQUID_MEDIUM:
                    tile = new Tile(288, 96);
                    maxHp = 50;
                    range = 3;
                    damage = 2;
                    level = 5;
                    break;
                case UnitTemplate.SQUID_HARD:
                    tile = new Tile(320, 96);
                    maxHp = 50;
                    range = 3;
                    damage = 3;
                    level = 5;
                    break;
                case UnitTemplate.SQUID_RANGE:
                    tile = new Tile(352, 96);
                    type = AttackType.RANGE;
                    maxHp = 20;
                    range = 3;
                    damage = 2;
                    level = 5;
                    break;
                case UnitTemplate.MAC_WORKER:
                    tile = new Tile(384, 96);
                    maxHp = 20;
                    range = 3;
                    damage = 2;
                    level = 5;
                    break;
                case UnitTemplate.RONALD:
                    tile = new Tile(416, 96);
                    maxHp = 100;
                    range = 2;
                    damage = 4;
                    level = 7;
                    break;
                case UnitTemplate.BOXER_ROCKY:
                    tile = new Tile(448, 96);
                    maxHp = 100;
                    range = 2;
                    damage = 10;
                    level = 10;
                    break;
                case UnitTemplate.BOXER_ALI:
                    tile = new Tile(480, 96);
                    maxHp = 45;
                    range = 7;
                    damage = 3;
                    level = 10;
                    break;
                case UnitTemplate.BOXER_STUDENT:
                    tile = new Tile(512, 96);
                    maxHp = 20;
                    range = 5;
                    damage = 5;
                    level = 7;
                    break;
                case UnitTemplate.COSMONAUT:
                    tile = new Tile(512 + Global.Rand.Next(0, 3) * Global.CellSize, 64);
                    maxHp = 50;
                    range = 3;
                    damage = 2;
                    level = 10;
                    break;
                case UnitTemplate.COSMONAUT_RANGE:
                    tile = new Tile(608, 64);
                    type = AttackType.RANGE;
                    maxHp = 20;
                    range = 3;
                    damage = 2;
                    level = 10;
                    break;
                case UnitTemplate.SOLDIER_RANGE:
                    tile = new Tile(576, 96);
                    type = AttackType.RANGE;
                    maxHp = 10;
                    range = 3;
                    damage = 2;
                    level = 10;
                    break;
                case UnitTemplate.JULIA_S:
                    tile = new Tile(544, 96);
                    maxHp = 50;
                    range = 6;
                    damage = 2;
                    level = 10;
                    break;
                case UnitTemplate.FEDOR:
                    tile = new Tile(608, 96);
                    maxHp = 999;
                    range = 1;
                    damage = 0;
                    level = 1;
                    break;
                case UnitTemplate.MIGAS:
                    tile = new Tile(160, 128);
                    maxHp = 50;
                    range = 3;
                    damage = 4;
                    level = 10;
                    break;
                case UnitTemplate.VANYA:
                    tile = new Tile(224, 128);
                    maxHp = 69;
                    range = 5;
                    damage = 2;
                    level = 10;
                    break;
                case UnitTemplate.DIMA:
                    tile = new Tile(256, 128);
                    maxHp = 80;
                    range = 2;
                    damage = 10;
                    level = 10;
                    break;
                case UnitTemplate.JULIA_B:
                    tile = new Tile(320, 128);
                    maxHp = 50;
                    range = 3;
                    damage = 2;
                    level = 10;
                    break;
                case UnitTemplate.MISHA:
                    tile = new Tile(384, 128);
                    maxHp = 50;
                    range = 3;
                    damage = 2;
                    level = 10;
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

        internal AttackType Attack { get => type; set => type = value; }

        public int Range { get => range; set => range = value; }

        public int Damage { get => damage; set => damage = value; }

        public int Hp 
        { 
            get => hp; 
            
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Unit health can't be negative");
                }
                else if (value > MaxHp)
                {
                    throw new ArgumentOutOfRangeException($"Unit health can't be more then {MaxHp}");
                }

                hp = value;
            } 
        }

        public int MaxHp { get => maxHp; set => maxHp = value; }

        public int Exp { get => exp; set => exp = value; }

        public int NextLevel { get => nextLevelExp; set => nextLevelExp = value; }

        public int Level { get => level; set => level = value; }

        public Tile Tile => tile;

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
