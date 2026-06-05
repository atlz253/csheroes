using CSHeroes.Core.Model;

namespace CSHeroes.Core.Services;

public enum GameScreen
{
    MainMenu,
    NewGame,
    Exploring,
    Battle,
    Camp,
    Win,
    Defeat
}

public enum SceneObjectKind
{
    Hero,
    Army,
    Unit,
    Obstacle
}

public enum CampUpgradeOption
{
    Hp,
    Damage,
    Range,
    MeleeAttack,
    RangedAttack
}

public enum CampUpgradeMode
{
    None,
    Stats,
    AttackType
}

public sealed record SceneObjectSnapshot(SceneObjectKind Kind, int X, int Y, SpriteRect Sprite, int? Hp = null);

public sealed record HighlightSnapshot(int X, int Y, string Kind);

public sealed record BackgroundCellSnapshot(int X, int Y, SpriteRect Sprite);

public sealed record ExploreSnapshot(
    int Width,
    int Height,
    IReadOnlyList<BackgroundCellSnapshot> Background,
    IReadOnlyList<SceneObjectSnapshot> Objects,
    int Respect,
    string LocationName);

public sealed record BattleSnapshot(
    int Width,
    int Height,
    SpriteRect Background,
    IReadOnlyList<SceneObjectSnapshot> Objects,
    IReadOnlyList<HighlightSnapshot> Highlights,
    bool PlayerTurn,
    bool AiEnabled);

public sealed record CampUnitSnapshot(
    int Index,
    SpriteRect? Sprite,
    int Hp,
    int MaxHp,
    int Damage,
    int Range,
    int Exp,
    int NextLevel,
    int Level,
    bool CanUpgrade,
    int UpgradeCost,
    CampUpgradeMode UpgradeMode);

public sealed record CampSnapshot(int Respect, IReadOnlyList<CampUnitSnapshot> Units);

public sealed record GameSnapshot(
    GameScreen Screen,
    ExploreSnapshot? Explore = null,
    BattleSnapshot? Battle = null,
    CampSnapshot? Camp = null,
    string? Message = null,
    int? Score = null);
