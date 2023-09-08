using BlasII.DebugMod.FreeCam;
using BlasII.DebugMod.Hitboxes;
using BlasII.DebugMod.NoClip;
using Newtonsoft.Json;

namespace BlasII.DebugMod
{
    public class MainConfig
    {
        public readonly HitboxConfig hitboxViewer;
        public readonly NoclipConfig noClip;
        public readonly CameraConfig freeCamera;

        [JsonConstructor]
        public MainConfig(HitboxConfig hitboxViewer, NoclipConfig noClip, CameraConfig freeCamera)
        {
            this.hitboxViewer = hitboxViewer ?? new HitboxConfig();
            this.noClip = noClip ?? new NoclipConfig();
            this.freeCamera = freeCamera ?? new CameraConfig();
        }

        public MainConfig()
        {
            hitboxViewer = new HitboxConfig();
            noClip = new NoclipConfig();
            freeCamera = new CameraConfig();
        }
    }
}
