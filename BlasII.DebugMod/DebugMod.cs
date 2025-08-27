using BlasII.DebugMod.EventLogger;
using BlasII.DebugMod.FreeCam;
using BlasII.DebugMod.HitboxViewer;
using BlasII.DebugMod.InfoDisplay;
using BlasII.DebugMod.NoClip;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using Il2CppLightbug.Kinematic2D.Core;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.UI;
using MelonLoader;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        ModLog.Error("Creating render texture");
        renderTexture = new RenderTexture(WIDTH, HEIGHT, 24, RenderTextureFormat.ARGB32);
        renderTexture.Create();

        //CoreCache.PlayerSpawn.add_OnPlayerSpawned((Il2CppSystem.Action)Spawn);
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

        if (sceneName == "MainMenu")
            return;

        CoreCache.UINavigationHelper.HideHud = true;
        //CoreCache.UINavigationHelper.ClearFade(0);
        //CoreCache.UINavigationHelper.CloseFadeWindow();
        //var fade = Object.FindObjectOfType<FadeWindowLogic>();
        //fade.colorImage.color = new Color(0, 0, 0, 0);

        var parallax = Object.FindObjectOfType<ParallaxComponent>();
        if (parallax != null)
        {
            GameObject obj = parallax.layers[0].layer;

            ModLog.Warn("Changing parallax: " + obj.name);
            obj.SetActive(false);
            parallax.influenceX = 0;
            parallax.influenceY = 0;
        }

        positions = new Dictionary<EnemyRoomEntity, Vector3>();
        foreach (var enemy in Object.FindObjectsOfType<EnemyRoomEntity>())
        {
            ModLog.Warn("Found: " + enemy.name);
            positions.Add(enemy, enemy.transform.position);
        }

        //MelonCoroutines.Start(Wait());
    }

    private Dictionary<EnemyRoomEntity, Vector3> positions;

    private System.Collections.IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 0;
    }

    protected override void OnLateUpdate()
    {
        if (!SceneHelper.GameSceneLoaded)
            return;

        //foreach (var enemy in Object.FindObjectsOfType<EnemyRoomEntity>())
        //{
        //    if (positions.TryGetValue(enemy, out Vector3 pos))
        //    {
        //        enemy.transform.position = pos;
        //    }
        //}

        foreach (var spawn in CoreCache.EnemySpawn.activeSpawnPoints)
        {
            Transform point = spawn.spawnParent;

            if (point == null)
                continue;

            GameObject enemy = spawn.ObtainSpawnedEnemy();

            if (enemy == null)
                continue;

            var impl = enemy.GetComponent<CharacterBody2DImpl>();

            if (impl == null)
                continue;

            ModLog.Info($"Updating position for {enemy.name}");
            enemy.transform.position = point.position;
            impl.bodyTransform = new BodyTransform { position = point.position };

            if (Input.GetKey(KeyCode.Keypad7))
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }

    private void Spawn()
    {
        ModLog.Info("Spawned");
        Time.timeScale = 0;
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

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            ModLog.Warn("Reset");
            if (bigTexture != null)
                Object.Destroy(bigTexture);
            bigTexture = null;
            count = 0;
        }

        if (!Input.GetKeyDown(KeyCode.Keypad9))
            return;

        ModLog.Error("Saving picture");
        //Time.timeScale = 0;

        //foreach (var comp in Object.FindObjectsOfType<ParallaxComponent>())
        //{
        //    ModLog.Info("Hiding " + comp.name);
        //    comp.influenceX = 0;
        //    comp.influenceY = 0;
        //    //comp.gameObject.SetActive(false);
        //}
        CoreCache.PlayerSpawn.PlayerInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        //foreach (var renderer in CoreCache.PlayerSpawn.PlayerInstance.GetComponentsInChildren<Renderer>())
        //{
        //    ModLog.Info("Invis: " + renderer.name);
        //    renderer.enabled = false;
        //}

        //foreach (var comp in Object.FindObjectsOfType<ParallaxCamera>())
        //{
        //    ModLog.Warn("Hiding " + comp.name);
        //    comp.gameObject.SetActive(false);
        //}

        //foreach (var cam in Object.FindObjectsOfType<Camera>())
        //{
        //    ModLog.Info(cam.name + cam.pixelWidth + " - " + cam.pixelHeight);
        //}

        Camera cam = Object.FindObjectsOfType<Camera>().First(x => x.name == "scene composition camera"); // 1920x1080
        //Camera cam = Camera.main; //640x360
        //Camera.main.backgroundColor = new Color(0, 0, 0, 0);

        //var parallax = Object.FindObjectOfType<ParallaxComponent>();
        //if (parallax != null)
        //{
        //    GameObject obj = parallax.layers[0].layer;

        //    ModLog.Warn("Changing parallax: " + obj.name);
        //    obj.SetActive(false);
        //    parallax.influenceX = 0;
        //    parallax.influenceY = 0;
        //}

        if (count == 0)
        {
            ModLog.Warn("Creating big tex");
            bigTexture = new Texture2D(WIDTH * 4, HEIGHT * 4, TextureFormat.ARGB32, false);
            firstPosition = cam.transform.position;
        }

        cam.backgroundColor = Color.magenta;
        cam.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        var tex = new Texture2D(WIDTH, HEIGHT, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, WIDTH, HEIGHT), 0, 0);
        tex.Apply();

        // Copy tex onto bigTexture
        Vector3 positionChange = cam.transform.position - firstPosition;
        Vector2Int newpos = new Vector2Int((int)(positionChange.x * 32 * 3), (int)(positionChange.y * 32 * 3));

        Graphics.CopyTexture(tex, 0, 0, 0, 0, WIDTH, HEIGHT, bigTexture, 0, 0, newpos.x, newpos.y);

        byte[] bytes = bigTexture.EncodeToPNG();
        string path = Path.Combine(FileHandler.ModdingFolder, $"{CoreCache.Room.CurrentRoom.Name}.png");

        File.WriteAllBytes(path, bytes);

        RenderTexture.active = null;
        Object.Destroy(tex);
        count++;
    }

    private int count = 0;
    private Vector3 firstPosition = Vector3.zero;

    private RenderTexture renderTexture;
    private Texture2D bigTexture;

    private const int WIDTH = 1920;
    private const int HEIGHT = 1080;
}
