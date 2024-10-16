using BlasII.DebugMod.InfoDisplay;
using BlasII.DebugMod.EventLogger;
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

    internal InfoModule InfoModule { get; private set; }
    internal HitboxViewer HitboxViewer { get; private set; }
    internal ClipModule ClipModule { get; private set; }
    internal CameraModule CameraModule { get; private set; }
    internal LoggerModule LoggerModule { get; private set; }

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

        // The other modules should be moved to this system where settings are passed in ctor
        // They should also be named similarly and their namespace should be the official name
        InfoModule = new InfoModule(DebugSettings.InfoDisplay);
        HitboxViewer = new HitboxViewer();
        ClipModule = new ClipModule(DebugSettings.NoClip);
        CameraModule = new CameraModule(DebugSettings.FreeCam);
        LoggerModule = new LoggerModule(DebugSettings.EventLogger);
    }

    /// <summary>
    /// Handle sceneLoaded event
    /// </summary>
    protected override void OnSceneLoaded(string sceneName)
    {
        InfoModule.SceneLoaded();
        HitboxViewer.SceneLoaded();
        ClipModule.SceneLoaded();
        CameraModule.SceneLoaded();
    }

    /// <summary>
    /// Handle sceneUnloaded event
    /// </summary>
    protected override void OnSceneUnloaded(string sceneName)
    {
        InfoModule.SceneUnloaded();
        HitboxViewer.SceneUnloaded();
        ClipModule.SceneUnloaded();
        CameraModule.SceneUnloaded();
    }

    /// <summary>
    /// Handle update event if in-game
    /// </summary>
    protected override void OnUpdate()
    {
        if (!SceneHelper.GameSceneLoaded)
            return;

        InfoModule.Update();
        HitboxViewer.Update();
        ClipModule.Update();
        CameraModule.Update();
    }
}
