using BlasII.DebugMod.FreeCam;
using BlasII.DebugMod.Hitboxes;
using BlasII.DebugMod.NoClip;
using BlasII.ModdingAPI;

namespace BlasII.DebugMod
{
    public class DebugMod : BlasIIMod
    {
        public DebugMod() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        public HitboxViewer HitboxViewer { get; private set; }
        public NoClipper NoClipper { get; private set; }
        public CameraMover CameraMover { get; private set; }

        protected override void OnInitialize()
        {
            MainConfig config = FileHandler.LoadConfig<MainConfig>();

            HitboxViewer = new HitboxViewer(config.hitboxViewer);
            NoClipper = new NoClipper();
            CameraMover = new CameraMover(config.freeCamera);
        }

        protected override void OnSceneLoaded(string sceneName)
        {
            HitboxViewer.SceneLoaded();
            NoClipper.SceneLoaded();
            CameraMover.SceneLoaded();
        }

        protected override void OnSceneUnloaded(string sceneName)
        {
            HitboxViewer.SceneUnloaded();
            NoClipper.SceneUnloaded();
            CameraMover.SceneUnloaded();
        }

        protected override void OnUpdate()
        {
            HitboxViewer.Update();
            NoClipper.Update();
            CameraMover.Update();
        }
    }
}
