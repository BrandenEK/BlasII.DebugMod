using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

/// <summary>
/// Information regarding a Collider2D
/// </summary>
public struct HitboxInfo(Collider2D collider, ColliderType ctype, HitboxType htype, bool isVisible)
{
    /// <summary>
    /// The actual collider
    /// </summary>
    public Collider2D Collider { get; } = collider;

    /// <summary>
    /// The type of collider
    /// </summary>
    public ColliderType Ctype { get; } = ctype;

    /// <summary>
    /// The type of hitbox
    /// </summary>
    public HitboxType Htype { get; } = htype;

    /// <summary>
    /// Whether this collider should be displayed
    /// </summary>
    public bool IsVisible { get; } = isVisible;
}
