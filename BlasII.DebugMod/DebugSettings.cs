using BlasII.ModdingAPI.Config;
using UnityEngine;

namespace BlasII.DebugMod;

internal class DebugSettings
{
    public readonly int infoDisplayPrecision;
    public readonly float noClipSpeed;
    public readonly float freeCamSpeed;
    public readonly float hitboxUpdateDelay;

    public readonly Color hazardColor;
    public readonly Color damageableColor;
    public readonly Color playerColor;
    public readonly Color sensorColor;
    public readonly Color enemyColor;
    public readonly Color interactableColor;
    public readonly Color triggerColor;
    public readonly Color geometryColor;
    public readonly Color otherColor;

    public DebugSettings(ConfigHandler config)
    {
        infoDisplayPrecision = config.GetProperty<int>("Info_Display_Precision");
        noClipSpeed = config.GetProperty<float>("No_Clip_Speed");
        freeCamSpeed = config.GetProperty<float>("Free_Cam_Speed");
        hitboxUpdateDelay = config.GetProperty<float>("Hitbox_Update_Delay");

        hazardColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Hazard"), out Color color)
            ? color : DEFAULT_COLOR;
        damageableColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Damageable"), out color)
            ? color : DEFAULT_COLOR;
        playerColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Player"), out color)
            ? color : DEFAULT_COLOR;
        sensorColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Sensor"), out color)
            ? color : DEFAULT_COLOR;
        enemyColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Enemy"), out color)
            ? color : DEFAULT_COLOR;
        interactableColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Interactable"), out color)
            ? color : DEFAULT_COLOR;
        triggerColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Trigger"), out color)
            ? color : DEFAULT_COLOR;
        geometryColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Geometry"), out color)
            ? color : DEFAULT_COLOR;
        otherColor = ColorUtility.TryParseHtmlString(config.GetProperty<string>("Color_Other"), out color)
            ? color : DEFAULT_COLOR;
    }

    private static readonly Color DEFAULT_COLOR = Color.white;
}
