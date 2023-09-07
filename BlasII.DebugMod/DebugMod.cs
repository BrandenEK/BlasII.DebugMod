using BlasII.DebugMod.Hitboxes;
using BlasII.ModdingAPI;

namespace BlasII.DebugMod
{
    public class DebugMod : BlasIIMod
    {
        public DebugMod() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        public HitboxViewer HitboxViewer { get; private set; }

        protected override void OnInitialize()
        {
            HitboxViewer = new HitboxViewer(new HitboxConfig(true, false, 1f));
        }

        protected override void OnSceneLoaded(string sceneName)
        {
            HitboxViewer.SceneLoaded();
        }

        protected override void OnSceneUnloaded(string sceneName)
        {
            HitboxViewer.SceneUnloaded();
        }

        protected override void OnUpdate()
        {
            HitboxViewer.Update();
        }
    }
}
