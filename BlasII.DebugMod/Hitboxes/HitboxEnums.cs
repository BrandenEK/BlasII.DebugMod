﻿
namespace BlasII.DebugMod.Hitboxes;

public enum HitboxType
{
    Hazard,
    Damageable,
    Player,
    Sensor,
    Enemy,
    Interactable,
    Trigger,
    Geometry,
    Other,

    Invalid
}

public enum ColliderType
{
    Box,
    Circle,
    Capsule,
    Polygon,

    Invalid
}
