using csheroes.src.Textures;

namespace csheroes.src.Units
{
    public class UnitFactory
    {
        public static Unit GetUnitByTemplate(string template)
        {
            return template switch
            {
                "creep" => new()
                {
                    MaxHp = 3,
                    Hp = 3,
                    Range = 3,
                    Damage = 1,
                    Level = 1,
                },
                "creep range" => new()
                {
                    Tile = new Tile(256 + Global.Rand.Next(0, 2) * Global.CellSize, 32),
                    Attack = AttackType.RANGE,
                    MaxHp = 3,
                    Hp = 3,
                    Range = 3,
                    Damage = 1,
                    Level = 1,
                },
                "weak" => new()
                {
                    MaxHp = 5,
                    Hp = 5,
                    Range = 3,
                    Damage = 1,
                    Level = 1,
                },
                "normal" => new()
                {
                    MaxHp = 7,
                    Hp = 7,
                    Range = 3,
                    Damage = 2,
                    Level = 1,
                },
                "hard" => new()
                {
                    MaxHp = 10,
                    Hp = 10,
                    Range = 3,
                    Damage = 3,
                    Level = 2,
                },
                "physic" => new()
                {
                    Tile = new Tile(224, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 5,
                    Damage = 1,
                    Level = 3,
                },
                "physic range" => new()
                {
                    Tile = new Tile(256, 64),
                    Attack = AttackType.RANGE,
                    MaxHp = 3,
                    Hp = 3,
                    Range = 3,
                    Damage = 1,
                    Level = 3,
                },
                "economist" => new()
                {
                    Tile = new Tile(288 + Global.Rand.Next(0, 3) * Global.CellSize, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 3,
                    Damage = 2,
                    Level = 3,
                },
                "stalker 1" => new()
                {
                    Tile = new Tile(384, 64),
                    MaxHp = 5,
                    Hp = 5,
                    Range = 3,
                    Damage = 2,
                    Level = 2,
                },
                "stalker 2" => new()
                {
                    Tile = new Tile(416, 64),
                    MaxHp = 5,
                    Hp = 5,
                    Range = 3,
                    Damage = 2,
                    Level = 2,
                },
                "balanced philosoph" => new()
                {
                    Tile = new Tile(704, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 3,
                    Damage = 2,
                    Level = 3,
                },
                "fast philosoph" => new()
                {
                    Tile = new Tile(672, 64),
                    MaxHp = 3,
                    Hp = 3,
                    Range = 6,
                    Damage = 1,
                    Level = 1,
                },
                "philosoph range" => new()
                {
                    Tile = new Tile(768, 64),
                    Attack = AttackType.RANGE,
                    MaxHp = 6,
                    Hp = 6,
                    Range = 3,
                    Damage = 2,
                    Level = 2,
                },
                "stron philosoph" => new()
                {
                    Tile = new Tile(736, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 1,
                    Damage = 4,
                    Level = 5,
                },
                "tenactious philosoph" => new()
                {
                    Tile = new Tile(640, 64),
                    MaxHp = 30,
                    Hp = 30,
                    Range = 2,
                    Damage = 1,
                    Level = 4,
                },
                "fast matrix" => new()
                {
                    Tile = new Tile(128, 64),
                    MaxHp = 20,
                    Hp = 20,
                    Range = 6,
                    Damage = 2,
                    Level = 4,
                },
                "strong matrix" => new()
                {
                    Tile = new Tile(160, 64),
                    MaxHp = 20,
                    Hp = 20,
                    Range = 2,
                    Damage = 4,
                    Level = 4,
                },
                "balanced matrix" => new()
                {
                    Tile = new Tile(192, 64),
                    MaxHp = 15,
                    Hp = 15,
                    Range = 3,
                    Damage = 3,
                    Level = 4,
                },
                "anonimus" => new()
                {
                    Tile = new Tile(448, 64),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 3,
                    Level = 6,
                },
                "bitard" => new()
                {
                    Tile = new Tile(480, 64),
                    MaxHp = 5,
                    Hp = 5,
                    Range = 6,
                    Damage = 1,
                    Level = 4,
                },
                "suslov" => new()
                {
                    Tile = new Tile(800, 64),
                    MaxHp = 100,
                    Hp = 100,
                    Range = 1,
                    Damage = 2,
                    Level = 10,
                },
                "kernel panic" => new()
                {
                    Tile = new Tile(832, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 7,
                    Damage = 1,
                    Level = 3,
                },
                "error" => new()
                {
                    Tile = new Tile(864, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 5,
                    Damage = 1,
                    Level = 3,
                },
                "bug" => new()
                {
                    Tile = new Tile(896, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 5,
                    Damage = 2,
                    Level = 3,
                },
                "hacker junior" => new()
                {
                    Tile = new Tile(128, 96),
                    MaxHp = 8,
                    Hp = 8,
                    Range = 3,
                    Damage = 2,
                    Level = 3,
                },
                "hacker middle" => new()
                {
                    Tile = new Tile(160, 96),
                    MaxHp = 12,
                    Hp = 12,
                    Range = 2,
                    Damage = 2,
                    Level = 4,
                },
                "hacker senior" => new()
                {
                    Tile = new Tile(192, 96),
                    MaxHp = 15,
                    Hp = 15,
                    Range = 2,
                    Damage = 3,
                    Level = 5,
                },
                "hacker fast" => new()
                {
                    Tile = new Tile(224, 96),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 5,
                    Damage = 2,
                    Level = 5,
                },
                "squid easy" => new()
                {
                    Tile = new Tile(256, 96),
                    MaxHp = 30,
                    Hp = 30,
                    Range = 3,
                    Damage = 2,
                    Level = 5,
                },
                "squid medium" => new()
                {
                    Tile = new Tile(288, 96),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 2,
                    Level = 5,
                },
                "squid hard" => new()
                {
                    Tile = new Tile(320, 96),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 3,
                    Level = 5,
                },
                "squid range" => new()
                {
                    Tile = new Tile(352, 96),
                    Attack = AttackType.RANGE,
                    MaxHp = 20,
                    Hp = 20,
                    Range = 3,
                    Damage = 2,
                    Level = 5,
                },
                "mac worker" => new()
                {
                    Tile = new Tile(384, 96),
                    MaxHp = 20,
                    Hp = 20,
                    Range = 3,
                    Damage = 2,
                    Level = 5,
                },
                "ronald" => new()
                {
                    Tile = new Tile(416, 96),
                    MaxHp = 100,
                    Hp = 100,
                    Range = 2,
                    Damage = 4,
                    Level = 7,
                },
                "boxer Rocky" => new()
                {
                    Tile = new Tile(448, 96),
                    MaxHp = 100,
                    Hp = 100,
                    Range = 2,
                    Damage = 10,
                    Level = 10,
                },
                "boxer Ali" => new()
                {
                    Tile = new Tile(480, 96),
                    MaxHp = 45,
                    Hp = 45,
                    Range = 7,
                    Damage = 3,
                    Level = 10,
                },
                "boxer student" => new()
                {
                    Tile = new Tile(512, 96),
                    MaxHp = 20,
                    Hp = 20,
                    Range = 5,
                    Damage = 5,
                    Level = 7,
                },
                "cosmonaut" => new()
                {
                    Tile = new Tile(512 + Global.Rand.Next(0, 3) * Global.CellSize, 64),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                "cosmonaut range" => new()
                {
                    Tile = new Tile(608, 64),
                    Attack = AttackType.RANGE,
                    MaxHp = 20,
                    Hp = 20,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                "soldier range" => new()
                {
                    Tile = new Tile(576, 96),
                    Attack = AttackType.RANGE,
                    MaxHp = 10,
                    Hp = 10,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                "Julia S" => new()
                {
                    Tile = new Tile(544, 96),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 6,
                    Damage = 2,
                    Level = 10,
                },
                "Fedor" => new()
                {
                    Tile = new Tile(608, 96),
                    MaxHp = 999,
                    Hp = 999,
                    Range = 1,
                    Damage = 0,
                    Level = 1,
                },
                "Migas" => new()
                {
                    Tile = new Tile(160, 128),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 4,
                    Level = 10,
                },
                "Vanya" => new()
                {
                    Tile = new Tile(224, 128),
                    MaxHp = 69,
                    Hp = 69,
                    Range = 5,
                    Damage = 2,
                    Level = 10,
                },
                "Dima" => new()
                {
                    Tile = new Tile(256, 128),
                    MaxHp = 80,
                    Hp = 80,
                    Range = 2,
                    Damage = 10,
                    Level = 10,
                },
                "Julia B" => new()
                {
                    Tile = new Tile(320, 128),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                "Misha" => new()
                {
                    Tile = new Tile(384, 128),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                "test unit" => new()
                {
                    Tile = new Tile(12, 12),
                    MaxHp = 123,
                    Hp = 100,
                    Range = 33,
                    Damage = 50,
                    Level = 12
                },
                _ => throw new UnitTemplateNotFoundException($"failed to create a unit from {template} template"),
            };
        }
    }
}
