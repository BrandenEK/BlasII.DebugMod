using BlasII.ModdingAPI.Config;
using UnityEngine;

namespace BlasII.DebugMod
{
    internal class DebugSettings
    {
        public readonly float noClipSpeed;
        public readonly float freeCamSpeed;

        public readonly Color inactiveColor;
        public readonly Color geometryColor;
        public readonly Color playerColor;
        public readonly Color enemyColor;
        public readonly Color hazardColor;
        public readonly Color triggerColor;
        public readonly Color otherColor;

        public DebugSettings(ConfigHandler config)
        {
            noClipSpeed = config.GetProperty<float>("No_Clip_Speed");
            freeCamSpeed = config.GetProperty<float>("Free_Cam_Speed");

            Color color;
            inactiveColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Inactive"), out color)
                ? color : DEFAULT_COLOR;
            geometryColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Geometry"), out color)
                ? color : DEFAULT_COLOR;
            playerColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Player"), out color)
                ? color : DEFAULT_COLOR;
            enemyColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Enemy"), out color)
                ? color : DEFAULT_COLOR;
            hazardColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Hazard"), out color)
                ? color : DEFAULT_COLOR;
            triggerColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Trigger"), out color)
                ? color : DEFAULT_COLOR;
            otherColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Other"), out color)
                ? color : DEFAULT_COLOR;
        }

        private static readonly Color DEFAULT_COLOR = Color.white;
    }
}
