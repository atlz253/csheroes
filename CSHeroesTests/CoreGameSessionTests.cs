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

    private static SceneObjectSnapshot GetHero(GameSession session)
    {
        return session.Snapshot.Explore!.Objects.Single(obj => obj.Kind == SceneObjectKind.Hero);
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

    private static void WriteUnit(BinaryWriter writer)
    {
        writer.Write("Unit");
        writer.Write(256);
        writer.Write(0);
        writer.Write("MELEE");
        writer.Write(10);
        writer.Write(10);
        writer.Write(0);
        writer.Write(3);
        writer.Write(1);
        writer.Write(1);
        writer.Write(1);
    }
}
