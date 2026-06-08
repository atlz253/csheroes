extern alias CoreLib;

using CoreLib::CSHeroes.Core.Services;

namespace CSHeroesTests;

[TestClass]
public class CoreGameSessionTests
{
    [TestMethod]
    public void StartNewGameShouldLoadExistingMapBytes()
    {
        var session = new GameSession();
        var map = File.ReadAllBytes(Path.Combine("..", "..", "..", "..", "csheroes", "Resources", "Maps", "FirstMap"));

        session.StartNewGame(map);

        Assert.AreEqual(GameScreen.Exploring, session.Snapshot.Screen);
        Assert.IsNotNull(session.Snapshot.Explore);
        Assert.AreEqual(25, session.Snapshot.Explore.Width);
        Assert.AreEqual(25, session.Snapshot.Explore.Height);
        Assert.IsTrue(session.Snapshot.Explore.Objects.Count > 0);
    }

    [TestMethod]
    public void SaveAndLoadShouldPreserveExploreSnapshot()
    {
        var session = new GameSession();
        var map = File.ReadAllBytes(Path.Combine("..", "..", "..", "..", "csheroes", "Resources", "Maps", "FirstMap"));
        session.StartNewGame(map);

        var save = session.Save();
        var clone = new GameSession();
        clone.Load(save);

        Assert.AreEqual(GameScreen.Exploring, clone.Snapshot.Screen);
        Assert.AreEqual(session.Snapshot.Explore!.LocationName, clone.Snapshot.Explore!.LocationName);
        Assert.AreEqual(session.Snapshot.Explore.Objects.Count, clone.Snapshot.Explore.Objects.Count);
    }

    [TestMethod]
    public void ClickObstacleShouldNotMoveHero()
    {
        var session = new GameSession();
        var map = File.ReadAllBytes(Path.Combine("..", "..", "..", "..", "csheroes", "Resources", "Maps", "FirstMap"));
        session.StartNewGame(map);
        var before = GetHero(session);

        session.ClickExploreCell(3, 0);

        var after = GetHero(session);
        Assert.AreEqual(before.X, after.X);
        Assert.AreEqual(before.Y, after.Y);
    }

    [TestMethod]
    public void ClickEnemyArmyShouldStartBattleWithoutMovingHeroOntoArmyCell()
    {
        var session = new GameSession();
        var map = File.ReadAllBytes(Path.Combine("..", "..", "..", "..", "csheroes", "Resources", "Maps", "FirstMap"));
        session.StartNewGame(map);
        var before = GetHero(session);

        session.ClickExploreCell(21, 2);

        Assert.AreEqual(GameScreen.Battle, session.Snapshot.Screen);
        var after = GetHero(session);
        Assert.AreEqual(before.X, after.X);
        Assert.AreEqual(before.Y, after.Y);
    }

    [TestMethod]
    public void EnemyArmyShouldBlockPathToCellsBehindIt()
    {
        var session = new GameSession();
        var map = CreateBlockedRoomMap();
        session.StartNewGame(map);
        var before = GetHero(session);

        session.ClickExploreCell(4, 1);

        var after = GetHero(session);
        Assert.AreEqual(GameScreen.Exploring, session.Snapshot.Screen);
        Assert.AreEqual(before.X, after.X);
        Assert.AreEqual(before.Y, after.Y);
    }

    [TestMethod]
    public void EnemyArmyShouldNotBeUsedAsBridgeToReachEmptyCellBehindIt()
    {
        var session = new GameSession();
        var map = CreateBlockedRoomMap();
        session.StartNewGame(map);

        session.ClickExploreCell(3, 1);

        var hero = GetHero(session);
        Assert.AreEqual(1, hero.X);
        Assert.AreEqual(1, hero.Y);
    }

    [TestMethod]
    public void HiddenSerializedColumnShouldNotAllowBypassingEnemyOnFirstMap()
    {
        var session = new GameSession();
        var map = File.ReadAllBytes(Path.Combine("..", "..", "..", "..", "csheroes", "Resources", "Maps", "FirstMap"));
        session.StartNewGame(map);
        var before = GetHero(session);

        session.ClickExploreCell(21, 3);

        var after = GetHero(session);
        Assert.AreEqual(before.X, after.X);
        Assert.AreEqual(before.Y, after.Y);
    }

    [TestMethod]
    public void CampHpUpgradeShouldConsumeRespectAndIncreaseHealth()
    {
        var session = StartCampSession(CreateCampUpgradeMap(respect: 500, level: 2, exp: 2, nextLevel: 2));

        session.UpgradeUnit(0, CampUpgradeOption.Hp);

        var unit = GetCampUnit(session);
        Assert.AreEqual(300, session.Snapshot.Camp!.Respect);
        Assert.AreEqual(13, unit.Hp);
        Assert.AreEqual(13, unit.MaxHp);
        Assert.AreEqual(3, unit.Level);
        Assert.AreEqual(4, unit.NextLevel);
    }

    [TestMethod]
    public void CampDamageUpgradeShouldIncreaseDamage()
    {
        var session = StartCampSession(CreateCampUpgradeMap(respect: 100, level: 1, exp: 1, nextLevel: 1));

        session.UpgradeUnit(0, CampUpgradeOption.Damage);

        var unit = GetCampUnit(session);
        Assert.AreEqual(0, session.Snapshot.Camp!.Respect);
        Assert.AreEqual(2, unit.Damage);
        Assert.AreEqual(2, unit.Level);
        Assert.AreEqual(2, unit.NextLevel);
    }

    [TestMethod]
    public void CampRangeUpgradeShouldIncreaseRange()
    {
        var session = StartCampSession(CreateCampUpgradeMap(respect: 100, level: 1, exp: 1, nextLevel: 1));

        session.UpgradeUnit(0, CampUpgradeOption.Range);

        var unit = GetCampUnit(session);
        Assert.AreEqual(0, session.Snapshot.Camp!.Respect);
        Assert.AreEqual(4, unit.Range);
        Assert.AreEqual(2, unit.Level);
        Assert.AreEqual(2, unit.NextLevel);
    }

    [TestMethod]
    public void CampUpgradeShouldBeRejectedWhenExperienceIsTooLow()
    {
        var session = StartCampSession(CreateCampUpgradeMap(respect: 500, level: 1, exp: 0, nextLevel: 1));

        session.UpgradeUnit(0, CampUpgradeOption.Hp);

        var unit = GetCampUnit(session);
        Assert.AreEqual(500, session.Snapshot.Camp!.Respect);
        Assert.AreEqual(10, unit.Hp);
        Assert.AreEqual(10, unit.MaxHp);
        Assert.AreEqual(1, unit.Level);
        Assert.AreEqual(1, unit.NextLevel);
    }

    [TestMethod]
    public void CampUpgradeShouldBeRejectedWhenRespectIsTooLow()
    {
        var session = StartCampSession(CreateCampUpgradeMap(respect: 99, level: 1, exp: 1, nextLevel: 1));

        session.UpgradeUnit(0, CampUpgradeOption.Damage);

        var unit = GetCampUnit(session);
        Assert.AreEqual(99, session.Snapshot.Camp!.Respect);
        Assert.AreEqual(1, unit.Damage);
        Assert.AreEqual(1, unit.Level);
        Assert.AreEqual(1, unit.NextLevel);
    }

    [TestMethod]
    public void CampRangeFiveUpgradeShouldConsumeRespectAndChooseAttackType()
    {
        var session = StartCampSession(CreateCampUpgradeMap(respect: 500, level: 2, exp: 2, nextLevel: 2, range: 5));

        session.UpgradeUnit(0, CampUpgradeOption.RangedAttack);

        var unit = GetCampUnit(session);
        Assert.AreEqual(0, session.Snapshot.Camp!.Respect);
        Assert.AreEqual(6, unit.Range);
        Assert.AreEqual(3, unit.Level);
        Assert.AreEqual(4, unit.NextLevel);
    }

    [TestMethod]
    public void RangedUnitShouldAttackEnemyOutsideMovementRange()
    {
        var session = new GameSession();
        session.StartNewGame(CreateBattleMap(heroAttackType: "RANGE", heroRange: 1, heroDamage: 3, enemyHp: 10));
        session.ClickExploreCell(2, 1);
        session.ToggleAi();

        var enemy = session.Snapshot.Battle!.Objects.Single(obj => obj.X > 0);
        Assert.IsTrue(session.Snapshot.Battle.Highlights.Any(
            highlight => highlight.X == enemy.X && highlight.Y == enemy.Y && highlight.Kind == "enemy"));

        session.ClickBattleCell(enemy.X, enemy.Y);

        var damagedEnemy = session.Snapshot.Battle!.Objects.Single(obj => obj.X > 0);
        Assert.AreEqual(7, damagedEnemy.Hp);
    }

    [TestMethod]
    public void UnitUpgradedToRangedShouldAttackEnemyOutsideMovementRange()
    {
        var session = new GameSession();
        session.StartNewGame(CreateBattleMap(
            heroAttackType: "MELEE",
            heroRange: 5,
            heroDamage: 3,
            enemyHp: 10,
            heroRespect: 500,
            heroExp: 1));
        session.EnterCamp();
        session.UpgradeUnit(0, CampUpgradeOption.RangedAttack);
        session.ExitCamp();
        session.ClickExploreCell(2, 1);
        session.ToggleAi();

        var enemy = session.Snapshot.Battle!.Objects.Single(obj => obj.X > 0);
        Assert.IsTrue(session.Snapshot.Battle.Highlights.Any(
            highlight => highlight.X == enemy.X && highlight.Y == enemy.Y && highlight.Kind == "enemy"));

        session.ClickBattleCell(enemy.X, enemy.Y);

        var damagedEnemy = session.Snapshot.Battle!.Objects.Single(obj => obj.X > 0);
        Assert.AreEqual(7, damagedEnemy.Hp);
    }

    private static SceneObjectSnapshot GetHero(GameSession session)
    {
        return session.Snapshot.Explore!.Objects.Single(obj => obj.Kind == SceneObjectKind.Hero);
    }

    private static CampUnitSnapshot GetCampUnit(GameSession session)
    {
        return session.Snapshot.Camp!.Units[0];
    }

    private static GameSession StartCampSession(byte[] map)
    {
        var session = new GameSession();
        session.StartNewGame(map);
        session.EnterCamp();
        return session;
    }

    private static byte[] CreateBlockedRoomMap()
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        writer.Write("Blocked room");

        for (var y = 0; y < 25; y++)
        {
            for (var x = 0; x < 26; x++)
            {
                writer.Write(0);
                writer.Write(0);
            }
        }

        writer.Write(0);
        writer.Write(0);
        writer.Write(24);
        writer.Write(24);

        for (var y = 0; y < 25; y++)
        {
            for (var x = 0; x < 26; x++)
            {
                if (x == 1 && y == 1)
                {
                    WriteHero(writer);
                }
                else if (x == 2 && y == 1)
                {
                    WriteArmy(writer);
                }
                else if (IsBlockedRoomWall(x, y))
                {
                    writer.Write("Obstacle");
                    writer.Write(480);
                    writer.Write(192);
                }
                else
                {
                    writer.Write("NullObj");
                }
            }
        }

        writer.Flush();
        return stream.ToArray();
    }

    private static bool IsBlockedRoomWall(int x, int y)
    {
        if (x < 0 || x > 2 || y < 0 || y > 2)
        {
            return false;
        }

        if (x == 2 && y == 1)
        {
            return false;
        }

        return x == 0 || x == 2 || y == 0 || y == 2;
    }

    private static void WriteHero(BinaryWriter writer)
    {
        writer.Write("Hero");
        writer.Write(100);
        writer.Write("Army");
        writer.Write(false);
        WriteUnit(writer);
        for (var i = 1; i < 7; i++)
        {
            writer.Write("NoUnit");
        }
    }

    private static void WriteArmy(BinaryWriter writer)
    {
        writer.Write("Army");
        writer.Write(true);
        WriteUnit(writer);
        for (var i = 1; i < 7; i++)
        {
            writer.Write("NoUnit");
        }
    }

    private static byte[] CreateCampUpgradeMap(
        int respect,
        int level,
        int exp,
        int nextLevel,
        int hp = 10,
        int maxHp = 10,
        int range = 3,
        int damage = 1)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        writer.Write("Camp upgrades");

        for (var y = 0; y < 25; y++)
        {
            for (var x = 0; x < 26; x++)
            {
                writer.Write(0);
                writer.Write(0);
            }
        }

        writer.Write(0);
        writer.Write(0);
        writer.Write(24);
        writer.Write(24);

        for (var y = 0; y < 25; y++)
        {
            for (var x = 0; x < 26; x++)
            {
                if (x == 1 && y == 1)
                {
                    WriteHero(writer, respect, hp, maxHp, exp, range, damage, level, nextLevel);
                }
                else
                {
                    writer.Write("NullObj");
                }
            }
        }

        writer.Flush();
        return stream.ToArray();
    }

    private static byte[] CreateBattleMap(
        string heroAttackType,
        int heroRange,
        int heroDamage,
        int enemyHp,
        int heroRespect = 0,
        int heroExp = 0)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        writer.Write("Ranged battle");

        for (var y = 0; y < 25; y++)
        {
            for (var x = 0; x < 26; x++)
            {
                writer.Write(0);
                writer.Write(0);
            }
        }

        writer.Write(0);
        writer.Write(0);
        writer.Write(24);
        writer.Write(24);

        for (var y = 0; y < 25; y++)
        {
            for (var x = 0; x < 26; x++)
            {
                if (x == 1 && y == 1)
                {
                    writer.Write("Hero");
                    writer.Write(heroRespect);
                    writer.Write("Army");
                    writer.Write(false);
                    WriteUnit(
                        writer,
                        exp: heroExp,
                        range: heroRange,
                        damage: heroDamage,
                        attackType: heroAttackType);
                    WriteEmptyUnitSlots(writer);
                }
                else if (x == 2 && y == 1)
                {
                    writer.Write("Army");
                    writer.Write(true);
                    WriteUnit(writer, hp: enemyHp, maxHp: enemyHp);
                    WriteEmptyUnitSlots(writer);
                }
                else
                {
                    writer.Write("NullObj");
                }
            }
        }

        writer.Flush();
        return stream.ToArray();
    }

    private static void WriteEmptyUnitSlots(BinaryWriter writer)
    {
        for (var i = 1; i < 7; i++)
        {
            writer.Write("NoUnit");
        }
    }

    private static void WriteHero(
        BinaryWriter writer,
        int respect,
        int hp,
        int maxHp,
        int exp,
        int range,
        int damage,
        int level,
        int nextLevel)
    {
        writer.Write("Hero");
        writer.Write(respect);
        writer.Write("Army");
        writer.Write(false);
        WriteUnit(writer, hp, maxHp, exp, range, damage, level, nextLevel);
        for (var i = 1; i < 7; i++)
        {
            writer.Write("NoUnit");
        }
    }

    private static void WriteUnit(
        BinaryWriter writer,
        int hp = 10,
        int maxHp = 10,
        int exp = 0,
        int range = 3,
        int damage = 1,
        int level = 1,
        int nextLevel = 1,
        string attackType = "MELEE")
    {
        writer.Write("Unit");
        writer.Write(256);
        writer.Write(0);
        writer.Write(attackType);
        writer.Write(hp);
        writer.Write(maxHp);
        writer.Write(exp);
        writer.Write(range);
        writer.Write(damage);
        writer.Write(level);
        writer.Write(nextLevel);
    }
}
