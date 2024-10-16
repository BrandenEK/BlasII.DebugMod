using BlasII.DebugMod.DebugInfo;
using BlasII.DebugMod.FreeCam;
using BlasII.DebugMod.Hitboxes;
using BlasII.DebugMod.NoClip;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.DebugMod;

/// <summary>
/// Provides various debug utilities such as hitbox viewer and free cam
/// </summary>
public class DebugMod : BlasIIMod
{
    internal DebugMod() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    internal InfoDisplay InfoDisplay { get; private set; }
    internal HitboxViewer HitboxViewer { get; private set; }
    internal NoClipper NoClipper { get; private set; }
    internal CameraMover CameraMover { get; private set; }

    internal DebugSettings DebugSettings { get; private set; }

    /// <summary>
    /// Register handlers and initialize modules
    /// </summary>
    protected override void OnInitialize()
    {
        InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
        {
            { "InfoDisplay", KeyCode.F1 },
            { "HitboxViewer", KeyCode.F2 },
            { "NoClip", KeyCode.F3 },
            { "FreeCam", KeyCode.F4 },
            { "Hitbox_Hazard", KeyCode.Keypad1 },
            { "Hitbox_Damageable", KeyCode.Keypad2 },
            { "Hitbox_Player", KeyCode.Keypad3 },
            { "Hitbox_Sensor", KeyCode.Keypad4 },
            { "Hitbox_Enemy", KeyCode.Keypad5 },
            { "Hitbox_Interactable", KeyCode.Keypad6 },
            { "Hitbox_Trigger", KeyCode.Keypad7 },
            { "Hitbox_Geometry", KeyCode.Keypad8 },
            { "Hitbox_Other", KeyCode.Keypad9 },
        });
        
        DebugSettings = ConfigHandler.Load<DebugSettings>();

        InfoDisplay = new InfoDisplay();
        HitboxViewer = new HitboxViewer();
        NoClipper = new NoClipper();
        CameraMover = new CameraMover();
    }

    /// <summary>
    /// Handle sceneLoaded event
    /// </summary>
    protected override void OnSceneLoaded(string sceneName)
    {
        InfoDisplay.SceneLoaded();
        HitboxViewer.SceneLoaded();
        NoClipper.SceneLoaded();
        CameraMover.SceneLoaded();
    }

    /// <summary>
    /// Handle sceneUnloaded event
    /// </summary>
    protected override void OnSceneUnloaded(string sceneName)
    {
        InfoDisplay.SceneUnloaded();
        HitboxViewer.SceneUnloaded();
        NoClipper.SceneUnloaded();
        CameraMover.SceneUnloaded();
    }

    /// <summary>
    /// Handle update event if in-game
    /// </summary>
    protected override void OnUpdate()
    {
        if (!SceneHelper.GameSceneLoaded)
            return;

        InfoDisplay.Update();
        HitboxViewer.Update();
        NoClipper.Update();
        CameraMover.Update();
    }
}
