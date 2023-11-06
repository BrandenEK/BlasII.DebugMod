using BlasII.DebugMod.DebugInfo;
using BlasII.DebugMod.FreeCam;
using BlasII.DebugMod.Hitboxes;
using BlasII.DebugMod.NoClip;
using BlasII.ModdingAPI;
using System.Collections.Generic;

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
            InputHandler.RegisterDefaultKeybindings(new Dictionary<string, UnityEngine.KeyCode>()
            {
                { "InfoDisplay", UnityEngine.KeyCode.F1 },
                { "HitboxViewer", UnityEngine.KeyCode.F2 },
                { "NoClip", UnityEngine.KeyCode.F3 },
                { "FreeCam", UnityEngine.KeyCode.F4 },
            });
            ConfigHandler.RegisterDefaultProperties(new Dictionary<string, object>()
            {
                { "No_Clip_Speed", 0.1f },
                { "Free_Cam_Speed", 0.1f },
            });
            DebugSettings = new DebugSettings(ConfigHandler);

            InfoDisplay = new InfoDisplay();
            HitboxViewer = new HitboxViewer(new HitboxConfig());
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
