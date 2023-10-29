using csheroes.src.Textures;
using System;
using System.IO;

namespace csheroes.src.Units
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
                hp = value;
            }
        }

        public int MaxHp { get => maxHp; set => maxHp = value; }

        public int Exp { get => exp; set => exp = value; }

        public int NextLevel { get => nextLevelExp; set => nextLevelExp = value; }

        public int Level { get => level; set => level = value; }

        public Tile Tile
        {
            get
            {
                return tile;
            }

            set { tile = value; }
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
