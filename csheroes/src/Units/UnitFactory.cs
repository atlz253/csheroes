using csheroes.src.Textures;

namespace csheroes.src.Units
{
    public class UnitFactory
    {
        public static Unit GetUnitByTemplate(UnitTemplate template)
        {
            return template switch
            {
                UnitTemplate.CREEP => new()
                {
                    MaxHp = 3,
                    Hp = 3,
                    Range = 3,
                    Damage = 1,
                    Level = 1,
                },
                UnitTemplate.CREEP_RANGE => new()
                {
                    Tile = new Tile(256 + Global.Rand.Next(0, 2) * Global.CellSize, 32),
                    Attack = AttackType.RANGE,
                    MaxHp = 3,
                    Hp = 3,
                    Range = 3,
                    Damage = 1,
                    Level = 1,
                },
                UnitTemplate.WEAK => new()
                {
                    MaxHp = 5,
                    Hp = 5,
                    Range = 3,
                    Damage = 1,
                    Level = 1,
                },
                UnitTemplate.NORMAL => new()
                {
                    MaxHp = 7,
                    Hp = 7,
                    Range = 3,
                    Damage = 2,
                    Level = 1,
                },
                UnitTemplate.HARD => new()
                {
                    MaxHp = 10,
                    Hp = 10,
                    Range = 3,
                    Damage = 3,
                    Level = 2,
                },
                UnitTemplate.PHYSIC_MELEE => new()
                {
                    Tile = new Tile(224, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 5,
                    Damage = 1,
                    Level = 3,
                },
                UnitTemplate.PHYSIC_RANGE => new()
                {
                    Tile = new Tile(256, 64),
                    Attack = AttackType.RANGE,
                    MaxHp = 3,
                    Hp = 3,
                    Range = 3,
                    Damage = 1,
                    Level = 3,
                },
                UnitTemplate.ECONOMIST => new()
                {
                    Tile = new Tile(288 + Global.Rand.Next(0, 3) * Global.CellSize, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 3,
                    Damage = 2,
                    Level = 3,
                },
                UnitTemplate.STALKER_1 => new()
                {
                    Tile = new Tile(384, 64),
                    MaxHp = 5,
                    Hp = 5,
                    Range = 3,
                    Damage = 2,
                    Level = 2,
                },
                UnitTemplate.STALKER_2 => new()
                {
                    Tile = new Tile(416, 64),
                    MaxHp = 5,
                    Hp = 5,
                    Range = 3,
                    Damage = 2,
                    Level = 2,
                },
                UnitTemplate.PHILOSOPH_BALANCED => new()
                {
                    Tile = new Tile(704, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 3,
                    Damage = 2,
                    Level = 3,
                },
                UnitTemplate.PHILOSOPH_FAST => new()
                {
                    Tile = new Tile(672, 64),
                    MaxHp = 3,
                    Hp = 3,
                    Range = 6,
                    Damage = 1,
                    Level = 1,
                },
                UnitTemplate.PHILOSOPH_RANGE => new()
                {
                    Tile = new Tile(768, 64),
                    Attack = AttackType.RANGE,
                    MaxHp = 6,
                    Hp = 6,
                    Range = 3,
                    Damage = 2,
                    Level = 2,
                },
                UnitTemplate.PHILOSOPH_STRONG => new()
                {
                    Tile = new Tile(736, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 1,
                    Damage = 4,
                    Level = 5,
                },
                UnitTemplate.PHILOSOPH_TENACIOUS => new()
                {
                    Tile = new Tile(640, 64),
                    MaxHp = 30,
                    Hp = 30,
                    Range = 2,
                    Damage = 1,
                    Level = 4,
                },
                UnitTemplate.MATRIX_FAST => new()
                {
                    Tile = new Tile(128, 64),
                    MaxHp = 20,
                    Hp = 20,
                    Range = 6,
                    Damage = 2,
                    Level = 4,
                },
                UnitTemplate.MATRIX_STRONG => new()
                {
                    Tile = new Tile(160, 64),
                    MaxHp = 20,
                    Hp = 20,
                    Range = 2,
                    Damage = 4,
                    Level = 4,
                },
                UnitTemplate.MATRIX_BALANCED => new()
                {
                    Tile = new Tile(192, 64),
                    MaxHp = 15,
                    Hp = 15,
                    Range = 3,
                    Damage = 3,
                    Level = 4,
                },
                UnitTemplate.ANONIMUS => new()
                {
                    Tile = new Tile(448, 64),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 3,
                    Level = 6,
                },
                UnitTemplate.BITARD => new()
                {
                    Tile = new Tile(480, 64),
                    MaxHp = 5,
                    Hp = 5,
                    Range = 6,
                    Damage = 1,
                    Level = 4,
                },
                UnitTemplate.SUSLOV => new()
                {
                    Tile = new Tile(800, 64),
                    MaxHp = 100,
                    Hp = 100,
                    Range = 1,
                    Damage = 2,
                    Level = 10,
                },
                UnitTemplate.KERNEL_PANIC => new()
                {
                    Tile = new Tile(832, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 7,
                    Damage = 1,
                    Level = 3,
                },
                UnitTemplate.ERROR => new()
                {
                    Tile = new Tile(864, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 5,
                    Damage = 1,
                    Level = 3,
                },
                UnitTemplate.BUG => new()
                {
                    Tile = new Tile(896, 64),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 5,
                    Damage = 2,
                    Level = 3,
                },
                UnitTemplate.HACKER_JUNIOR => new()
                {
                    Tile = new Tile(128, 96),
                    MaxHp = 8,
                    Hp = 8,
                    Range = 3,
                    Damage = 2,
                    Level = 3,
                },
                UnitTemplate.HACKER_MIDDLE => new()
                {
                    Tile = new Tile(160, 96),
                    MaxHp = 12,
                    Hp = 12,
                    Range = 2,
                    Damage = 2,
                    Level = 4,
                },
                UnitTemplate.HACKER_SENIOR => new()
                {
                    Tile = new Tile(192, 96),
                    MaxHp = 15,
                    Hp = 15,
                    Range = 2,
                    Damage = 3,
                    Level = 5,
                },
                UnitTemplate.HACKER_FAST => new()
                {
                    Tile = new Tile(224, 96),
                    MaxHp = 10,
                    Hp = 10,
                    Range = 5,
                    Damage = 2,
                    Level = 5,
                },
                UnitTemplate.SQUID_EASY => new()
                {
                    Tile = new Tile(256, 96),
                    MaxHp = 30,
                    Hp = 30,
                    Range = 3,
                    Damage = 2,
                    Level = 5,
                },
                UnitTemplate.SQUID_MEDIUM => new()
                {
                    Tile = new Tile(288, 96),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 2,
                    Level = 5,
                },
                UnitTemplate.SQUID_HARD => new()
                {
                    Tile = new Tile(320, 96),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 3,
                    Level = 5,
                },
                UnitTemplate.SQUID_RANGE => new()
                {
                    Tile = new Tile(352, 96),
                    Attack = AttackType.RANGE,
                    MaxHp = 20,
                    Hp = 20,
                    Range = 3,
                    Damage = 2,
                    Level = 5,
                },
                UnitTemplate.MAC_WORKER => new()
                {
                    Tile = new Tile(384, 96),
                    MaxHp = 20,
                    Hp = 20,
                    Range = 3,
                    Damage = 2,
                    Level = 5,
                },
                UnitTemplate.RONALD => new()
                {
                    Tile = new Tile(416, 96),
                    MaxHp = 100,
                    Hp = 100,
                    Range = 2,
                    Damage = 4,
                    Level = 7,
                },
                UnitTemplate.BOXER_ROCKY => new()
                {
                    Tile = new Tile(448, 96),
                    MaxHp = 100,
                    Hp = 100,
                    Range = 2,
                    Damage = 10,
                    Level = 10,
                },
                UnitTemplate.BOXER_ALI => new()
                {
                    Tile = new Tile(480, 96),
                    MaxHp = 45,
                    Hp = 45,
                    Range = 7,
                    Damage = 3,
                    Level = 10,
                },
                UnitTemplate.BOXER_STUDENT => new()
                {
                    Tile = new Tile(512, 96),
                    MaxHp = 20,
                    Hp = 20,
                    Range = 5,
                    Damage = 5,
                    Level = 7,
                },
                UnitTemplate.COSMONAUT => new()
                {
                    Tile = new Tile(512 + Global.Rand.Next(0, 3) * Global.CellSize, 64),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                UnitTemplate.COSMONAUT_RANGE => new()
                {
                    Tile = new Tile(608, 64),
                    Attack = AttackType.RANGE,
                    MaxHp = 20,
                    Hp = 20,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                UnitTemplate.SOLDIER_RANGE => new()
                {
                    Tile = new Tile(576, 96),
                    Attack = AttackType.RANGE,
                    MaxHp = 10,
                    Hp = 10,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                UnitTemplate.JULIA_S => new()
                {
                    Tile = new Tile(544, 96),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 6,
                    Damage = 2,
                    Level = 10,
                },
                UnitTemplate.FEDOR => new()
                {
                    Tile = new Tile(608, 96),
                    MaxHp = 999,
                    Hp = 999,
                    Range = 1,
                    Damage = 0,
                    Level = 1,
                },
                UnitTemplate.MIGAS => new()
                {
                    Tile = new Tile(160, 128),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 4,
                    Level = 10,
                },
                UnitTemplate.VANYA => new()
                {
                    Tile = new Tile(224, 128),
                    MaxHp = 69,
                    Hp = 69,
                    Range = 5,
                    Damage = 2,
                    Level = 10,
                },
                UnitTemplate.DIMA => new()
                {
                    Tile = new Tile(256, 128),
                    MaxHp = 80,
                    Hp = 80,
                    Range = 2,
                    Damage = 10,
                    Level = 10,
                },
                UnitTemplate.JULIA_B => new()
                {
                    Tile = new Tile(320, 128),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                UnitTemplate.MISHA => new()
                {
                    Tile = new Tile(384, 128),
                    MaxHp = 50,
                    Hp = 50,
                    Range = 3,
                    Damage = 2,
                    Level = 10,
                },
                UnitTemplate.TEST_UNIT => new()
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
