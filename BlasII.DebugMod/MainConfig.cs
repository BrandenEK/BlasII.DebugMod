using BlasII.DebugMod.FreeCam;
using BlasII.DebugMod.Hitboxes;
using Newtonsoft.Json;

namespace BlasII.DebugMod
{
    public class MainConfig
    {
        public readonly HitboxConfig hitboxViewer;
        public readonly CameraConfig freeCamera;

        [JsonConstructor]
        public MainConfig(HitboxConfig hitboxViewer, CameraConfig freeCamera)
        {
            this.hitboxViewer = hitboxViewer ?? new HitboxConfig();
            this.freeCamera = freeCamera ?? new CameraConfig();
        }

        public MainConfig()
        {
            hitboxViewer = new HitboxConfig();
            freeCamera = new CameraConfig();
        }
    }
}
