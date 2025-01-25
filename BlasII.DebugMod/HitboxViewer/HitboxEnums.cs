
namespace BlasII.DebugMod.HitboxViewer;

/// <summary>
/// Types of hitboxes
/// </summary>
public enum HitboxType
{
    /// <summary> Invalid type </summary>
    Invalid,

    /// <summary> Currently disabled </summary>
    Inactive,
    /// <summary> Part of the level geometry </summary>
    Geometry,
    /// <summary> Any other hitbox </summary>
    Other,
    /// <summary> Has the isTrigger flag </summary>
    Trigger,
    /// <summary> Can be interacted with </summary>
    Interactable,
    /// <summary> Part of an enemy object </summary>
    Enemy,
    /// <summary> Detects when something enters its area </summary>
    Sensor,
    /// <summary> Part of the player object </summary>
    Player,
    /// <summary> Can be damaged </summary>
    Damageable,
    /// <summary> Damages the player on contact </summary>
    Hazard,
}

/// <summary>
/// Types of colliders
/// </summary>
public enum ColliderType
{
    /// <summary> Invalid type </summary>
    Invalid,

    /// <summary> <see cref="UnityEngine.BoxCollider2D"/> </summary>
    Box,
    /// <summary> <see cref="UnityEngine.CircleCollider2D"/> </summary>
    Circle,
    /// <summary> <see cref="UnityEngine.CapsuleCollider2D"/> </summary>
    Capsule,
    /// <summary> <see cref="UnityEngine.PolygonCollider2D"/> </summary>
    Polygon,
}
