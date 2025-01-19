
namespace BlasII.DebugMod.HitboxViewer;

/// <summary>
/// Types of hitboxes
/// </summary>
public enum HitboxType
{
    /// <summary> Currently disabled </summary>
    Inactive,
    /// <summary> Damages the player on contact </summary>
    Hazard,
    /// <summary> Can be damaged </summary>
    Damageable,
    /// <summary> Part of the player object </summary>
    Player,
    /// <summary> Detects when something enters its area </summary>
    Sensor,
    /// <summary> Part of an enemy object </summary>
    Enemy,
    /// <summary> Can be interacted with </summary>
    Interactable,
    /// <summary> Has the isTrigger flag </summary>
    Trigger,
    /// <summary> Part of the level geometry </summary>
    Geometry,
    /// <summary> Any other hitbox </summary>
    Other,

    /// <summary> Invalid type </summary>
    Invalid
}

/// <summary>
/// Types of colliders
/// </summary>
public enum ColliderType
{
    /// <summary> <see cref="UnityEngine.BoxCollider2D"/> </summary>
    Box,
    /// <summary> <see cref="UnityEngine.CircleCollider2D"/> </summary>
    Circle,
    /// <summary> <see cref="UnityEngine.CapsuleCollider2D"/> </summary>
    Capsule,
    /// <summary> <see cref="UnityEngine.PolygonCollider2D"/> </summary>
    Polygon,

    /// <summary> Invalid type </summary>
    Invalid
}
