using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.Attack;
using Il2CppTGK.Game.Components.Collisions;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Components.Persistence;
using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

/// <summary>
/// Useful methods for working with the hitboxes
/// </summary>
public static class HitboxExtensions
{
    /// <summary>
    /// Recursively checks if the component is in any of its parents
    /// </summary>
    public static bool HasComponentInParent<T>(this Transform transform)
    {
        Transform parent = transform;
        while (parent != null)
        {
            if (parent.GetComponent<T>() != null)
                return true;
            parent = parent.parent;
        }

        return false;
    }

    /// <summary>
    /// Determines the hitbox type from the components
    /// </summary>
    public static HitboxType GetHitboxType(this Collider2D collider)
    {
        if (!collider.enabled)
        {
            return HitboxType.Inactive;
        }
        if (collider.transform.GetComponent<AttackHit>() != null)
        {
            return HitboxType.Hazard;
        }
        if (collider.transform.HasComponentInParent<CollisionsCallback>())
        {
            return HitboxType.Damageable;
        }
        if (collider.transform.HasComponentInParent<PlayerPersistentComponent>())
        {
            return HitboxType.Player;
        }
        if (collider.transform.HasComponentInParent<TriggersCallback>())
        {
            return HitboxType.Sensor;
        }
        if (collider.transform.HasComponentInParent<AliveEntity>())
        {
            return HitboxType.Enemy;
        }
        if (collider.transform.HasComponentInParent<IInteractable>())
        {
            return HitboxType.Interactable;
        }
        if (collider.isTrigger)
        {
            return HitboxType.Trigger;
        }
        if (collider.name.StartsWith("GEO_"))
        {
            return HitboxType.Geometry;
        }

        return HitboxType.Other;
    }
}
