using BlasII.ModdingAPI.Config;

namespace BlasII.DebugMod
{
    internal class DebugSettings
    {
        public readonly float noClipSpeed;
        public readonly float freeCamSpeed;

        public DebugSettings(ConfigHandler config)
        {
            noClipSpeed = config.GetProperty<float>("No_Clip_Speed");
            freeCamSpeed = config.GetProperty<float>("Free_Cam_Speed");
        }
    }
}
