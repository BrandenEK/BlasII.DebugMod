
namespace BlasII.DebugMod.Hitboxes
{
    public enum HitboxType
    {
        Geometry,
        Damageable,
        Player,
        Enemy,
        Hazard,
        Interactable,
        Sensor,
        Trigger,
        Other,
    }

    public enum ColliderType
    {
        Box,
        Circle,
        Capsule,
        Polygon,
        Invalid
    }
}
