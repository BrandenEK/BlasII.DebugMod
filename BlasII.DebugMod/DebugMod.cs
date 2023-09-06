using BlasII.DebugMod.Hitboxes;
using BlasII.ModdingAPI;

namespace BlasII.DebugMod
{
    public class DebugMod : BlasIIMod
    {
        public DebugMod() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        public HitboxViewer HitboxViewer { get; private set; }

        private bool _inGame = false; // Move this into the modding api
        public bool InGame => _inGame;

        protected override void OnInitialize()
        {
            HitboxViewer = new HitboxViewer(new HitboxConfig(true, false, 1f));
        }

        protected override void OnSceneLoaded(string sceneName)
        {
            _inGame = true;

            HitboxViewer.SceneLoaded();
        }

        protected override void OnSceneUnloaded(string sceneName)
        {
            HitboxViewer.SceneUnloaded();

            _inGame = false;
        }

        protected override void OnUpdate()
        {
            HitboxViewer.Update();
        }
    }
}
