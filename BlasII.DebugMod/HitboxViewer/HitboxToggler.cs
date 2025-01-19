using BlasII.ModdingAPI;
using System;
using System.Collections.Generic;

namespace BlasII.DebugMod.HitboxViewer;

internal class HitboxToggler
{
    private readonly Dictionary<HitboxType, bool> _shownHitboxes;

    public HitboxToggler()
    {
        _shownHitboxes = new Dictionary<HitboxType, bool>();
        foreach (HitboxType hitbox in Enum.GetValues(typeof(HitboxType)))
            _shownHitboxes.Add(hitbox, true);
    }

    public bool ProcessToggles()
    {
        HitboxType input = HitboxType.Invalid;

        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Inactive")) input = HitboxType.Inactive;
        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Hazard")) input = HitboxType.Hazard;
        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Damageable")) input = HitboxType.Damageable;
        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Player")) input = HitboxType.Player;
        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Sensor")) input = HitboxType.Sensor;
        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Enemy")) input = HitboxType.Enemy;
        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Interactable")) input = HitboxType.Interactable;
        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Trigger")) input = HitboxType.Trigger;
        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Geometry")) input = HitboxType.Geometry;
        if (Main.DebugMod.InputHandler.GetKeyDown("Hitbox_Other")) input = HitboxType.Other;

        if (input == HitboxType.Invalid)
            return false;

        // If one of the toggle keys was pressed, toggle its status and tell the viewer to refresh
        ModLog.Info($"Toggling '{input}' hitboxes");
        _shownHitboxes[input] = !_shownHitboxes[input];
        return true;
    }

    public bool this[HitboxType hitbox] => _shownHitboxes[hitbox];
}
