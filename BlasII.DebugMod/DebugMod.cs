﻿using BlasII.DebugMod.EventLogger;
using BlasII.DebugMod.FreeCam;
using BlasII.DebugMod.HitboxViewer;
using BlasII.DebugMod.InfoDisplay;
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
    internal HitboxModule HitboxModule { get; private set; }
    internal ClipModule ClipModule { get; private set; }
    internal CameraModule CameraModule { get; private set; }
    internal LoggerModule LoggerModule { get; private set; }

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
            { "Hitbox_Inactive", KeyCode.Keypad0 },
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
        DebugSettings settings = ConfigHandler.Load<DebugSettings>();
        
        InfoModule = new InfoModule(settings.InfoDisplay);
        HitboxModule = new HitboxModule(settings.HitboxViewer);
        ClipModule = new ClipModule(settings.NoClip);
        CameraModule = new CameraModule(settings.FreeCam);
        LoggerModule = new LoggerModule(settings.EventLogger);
    }

    /// <summary>
    /// Handle sceneLoaded event
    /// </summary>
    protected override void OnSceneLoaded(string sceneName)
    {
        InfoModule.SceneLoaded();
        HitboxModule.SceneLoaded();
        ClipModule.SceneLoaded();
        CameraModule.SceneLoaded();
    }

    /// <summary>
    /// Handle sceneUnloaded event
    /// </summary>
    protected override void OnSceneUnloaded(string sceneName)
    {
        InfoModule.SceneUnloaded();
        HitboxModule.SceneUnloaded();
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
        HitboxModule.Update();
        ClipModule.Update();
        CameraModule.Update();
    }
}
