using BlasII.DebugMod.DebugInfo;
using BlasII.DebugMod.FreeCam;
using BlasII.DebugMod.Hitboxes;
using BlasII.DebugMod.NoClip;
using BlasII.ModdingAPI;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.DebugMod
{
    public class DebugMod : BlasIIMod
    {
        public DebugMod() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        public InfoDisplay InfoDisplay { get; private set; }
        public HitboxViewer HitboxViewer { get; private set; }
        public NoClipper NoClipper { get; private set; }
        public CameraMover CameraMover { get; private set; }

        internal DebugSettings DebugSettings { get; private set; }

        protected override void OnInitialize()
        {
            InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
            {
                { "InfoDisplay", KeyCode.F1 },
                { "HitboxViewer", KeyCode.F2 },
                { "NoClip", KeyCode.F3 },
                { "FreeCam", KeyCode.F4 },
            });
            ConfigHandler.RegisterDefaultProperties(new Dictionary<string, object>()
            {
                { "No_Clip_Speed", 0.1f },
                { "Free_Cam_Speed", 0.1f },
                { "Hitbox_Update_Delay", 0.2f },
                { "Color_Hazard", "#FF007F" },
                { "Color_Damageable", "#FFA500" },
                { "Color_Player", "#00CCCC" },
                { "Color_Sensor", "#660066" },
                { "Color_Enemy", "#DD0000" },
                { "Color_Interactable", "#FFFF33" },
                { "Color_Trigger", "#0066CC" },
                { "Color_Geometry", "#00CC00" },
                { "Color_Other", "#000099" },
            });
            DebugSettings = new DebugSettings(ConfigHandler);

            InfoDisplay = new InfoDisplay();
            HitboxViewer = new HitboxViewer();
            NoClipper = new NoClipper();
            CameraMover = new CameraMover();
        }

        protected override void OnSceneLoaded(string sceneName)
        {
            InfoDisplay.SceneLoaded();
            HitboxViewer.SceneLoaded();
            NoClipper.SceneLoaded();
            CameraMover.SceneLoaded();
        }

        protected override void OnSceneUnloaded(string sceneName)
        {
            InfoDisplay.SceneUnloaded();
            HitboxViewer.SceneUnloaded();
            NoClipper.SceneUnloaded();
            CameraMover.SceneUnloaded();
        }

        protected override void OnUpdate()
        {
            InfoDisplay.Update();
            HitboxViewer.Update();
            NoClipper.Update();
            CameraMover.Update();
        }
    }
}
