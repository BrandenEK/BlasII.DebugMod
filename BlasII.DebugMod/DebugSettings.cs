using BlasII.ModdingAPI.Config;
using UnityEngine;

namespace BlasII.DebugMod;

internal class DebugSettingsLegacy
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

    public DebugSettingsLegacy(ConfigHandler config)
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

        //ConfigHandler.RegisterDefaultProperties(new Dictionary<string, object>()
        //{
        //    { "Color_Hazard", "#FF007F" },
        //    { "Color_Damageable", "#FFA500" },
        //    { "Color_Player", "#00CCCC" },
        //    { "Color_Sensor", "#660066" },
        //    { "Color_Enemy", "#DD0000" },
        //    { "Color_Interactable", "#FFFF33" },
        //    { "Color_Trigger", "#0066CC" },
        //    { "Color_Geometry", "#00CC00" },
        //    { "Color_Other", "#000099" },
        //});
    }

    private static readonly Color DEFAULT_COLOR = Color.white;
}

/// <summary>
/// General settings
/// </summary>
public class DebugSettings
{
    /// <inheritdoc cref="InfoDisplaySettings" />
    public InfoDisplaySettings InfoDisplay { get; set; } = new();

    /// <inheritdoc cref="HitboxViewerSettings" />
    public HitboxViewerSettings HitboxViewer { get; set; } = new();

    /// <inheritdoc cref="NoClipSettings" />
    public NoClipSettings NoClip { get; set; } = new();

    /// <inheritdoc cref="FreeCamSettings" />
    public FreeCamSettings FreeCam { get; set; } = new();
}

/// <summary>
/// Settings for the InfoDisplay module
/// </summary>
public class InfoDisplaySettings
{
    /// <summary> How many decimal digits to display </summary>
    public int Precision { get; set; } = 2;
}

/// <summary>
/// Settings for the HitboxViewer module
/// </summary>
public class HitboxViewerSettings
{
    /// <summary> How many seconds between each hitbox update </summary>
    public float UpdateDelay { get; set; } = 0.2f;
}

/// <summary>
/// Settings for the NoClip module
/// </summary>
public class NoClipSettings
{
    /// <summary> How fast the player movement will be </summary>
    public float Speed { get; set; } = 0.1f;
}

/// <summary>
/// Settings for the FreeCam module
/// </summary>
public class FreeCamSettings
{
    /// <summary> How fast the camera movement will be </summary>
    public float Speed { get; set; } = 0.1f;
}
