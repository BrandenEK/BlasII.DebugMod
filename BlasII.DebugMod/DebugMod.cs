using BlasII.DebugMod.FreeCam;
using BlasII.DebugMod.Hitboxes;
using BlasII.ModdingAPI;

namespace BlasII.DebugMod
{
    public class DebugMod : BlasIIMod
    {
        public DebugMod() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        public HitboxViewer HitboxViewer { get; private set; }
        public CameraMover CameraMover { get; private set; }

        protected override void OnInitialize()
        {
            MainConfig config = FileHandler.LoadConfig<MainConfig>();

            Log("Hitbox inactive color: " + config.hitboxViewer.inactiveColor);
            Log("Hitbox player color: " + config.hitboxViewer.playerColor);
            Log("Hitbox update delay: " + config.hitboxViewer.updateDelay);

            Log("Camera speed: " + config.freeCamera.movementSpeed);

            HitboxViewer = new HitboxViewer(config.hitboxViewer);
            CameraMover = new CameraMover(config.freeCamera);
        }

        protected override void OnSceneLoaded(string sceneName)
        {
            HitboxViewer.SceneLoaded();
            CameraMover.SceneLoaded();
        }

        protected override void OnSceneUnloaded(string sceneName)
        {
            HitboxViewer.SceneUnloaded();
            CameraMover.SceneUnloaded();
        }

        protected override void OnUpdate()
        {
            HitboxViewer.Update();
            CameraMover.Update();
        }
    }
}
