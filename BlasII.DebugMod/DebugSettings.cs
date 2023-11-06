using BlasII.ModdingAPI.Config;
using UnityEngine;

namespace BlasII.DebugMod
{
    internal class DebugSettings
    {
        public readonly float noClipSpeed;
        public readonly float freeCamSpeed;
        public readonly float hitboxUpdateDelay;

        public readonly bool inactiveShow;
        public readonly Color inactiveColor;
        public readonly bool geometryShow;
        public readonly Color geometryColor;
        public readonly bool playerShow;
        public readonly Color playerColor;
        public readonly bool enemyShow;
        public readonly Color enemyColor;
        public readonly bool hazardShow;
        public readonly Color hazardColor;
        public readonly bool triggerShow;
        public readonly Color triggerColor;
        public readonly bool otherShow;
        public readonly Color otherColor;

        public DebugSettings(ConfigHandler config)
        {
            Color color;
            noClipSpeed = config.GetProperty<float>("No_Clip_Speed");
            freeCamSpeed = config.GetProperty<float>("Free_Cam_Speed");
            hitboxUpdateDelay = config.GetProperty<float>("Hitbox_Update_Delay");

            inactiveShow = config.GetProperty<bool>("Show_Inactive");
            inactiveColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Inactive"), out color)
                ? color : DEFAULT_COLOR;

            geometryShow = config.GetProperty<bool>("Show_Geometry");
            geometryColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Geometry"), out color)
                ? color : DEFAULT_COLOR;

            playerShow = config.GetProperty<bool>("Show_Player");
            playerColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Player"), out color)
                ? color : DEFAULT_COLOR;

            enemyShow = config.GetProperty<bool>("Show_Enemy");
            enemyColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Enemy"), out color)
                ? color : DEFAULT_COLOR;

            hazardShow = config.GetProperty<bool>("Show_Hazard");
            hazardColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Hazard"), out color)
                ? color : DEFAULT_COLOR;

            triggerShow = config.GetProperty<bool>("Show_Trigger");
            triggerColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Trigger"), out color)
                ? color : DEFAULT_COLOR;

            otherShow = config.GetProperty<bool>("Show_Other");
            otherColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Other"), out color)
                ? color : DEFAULT_COLOR;
        }

        private static readonly Color DEFAULT_COLOR = Color.white;
    }
}
