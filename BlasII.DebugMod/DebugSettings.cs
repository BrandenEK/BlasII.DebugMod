
namespace BlasII.DebugMod;

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

    /// <inheritdoc cref="EventLoggerSettings" />
    public EventLoggerSettings EventLogger { get; set; } = new();
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

    /// <summary> Whether hitbox colors/sizes should be updated every tick </summary>
    public bool FullRefresh { get; set; } = false;

    /// <summary> The minimum size for a hitbox to be shown </summary>
    public float MinSize { get; set; } = 0.1f;
    /// <summary> The maxmimum size for a hitbox to be shown </summary>
    public float MaxSize { get; set; } = 12f;

    /// <summary> The color of inactive colliders </summary>
    public string InactiveColor { get; set; } = "#4D4E4C";
    /// <summary> The color of hazard colliders </summary>
    public string HazardColor { get; set; } = "#FF007F";
    /// <summary> The color of damageable colliders </summary>
    public string DamageableColor { get; set; } = "#FFA500";
    /// <summary> The color of player colliders </summary>
    public string PlayerColor { get; set; } = "#00CCCC";
    /// <summary> The color of sensor colliders </summary>
    public string SensorColor { get; set; } = "#660066";
    /// <summary> The color of enemy colliders </summary>
    public string EnemyColor { get; set; } = "#DD0000";
    /// <summary> The color of interactable colliders </summary>
    public string InteractableColor { get; set; } = "#FFFF33";
    /// <summary> The color of trigger colliders </summary>
    public string TriggerColor { get; set; } = "#0066CC";
    /// <summary> The color of geometry colliders </summary>
    public string GeometryColor { get; set; } = "#00CC00";
    /// <summary> The color of other colliders </summary>
    public string OtherColor { get; set; } = "#000099";
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

/// <summary>
/// Settings for the EventLogger module
/// </summary>
public class EventLoggerSettings
{
    /// <summary> Should <see cref="EventLogger.EventType.Dialog"/> events be logged </summary>
    public bool LogDialogEvents { get; set; } = true;

    /// <summary> Should <see cref="EventLogger.EventType.Interactable"/> events be logged </summary>
    public bool LogInteractableEvents { get; set; } = true;

    /// <summary> Should <see cref="EventLogger.EventType.Inventory"/> events be logged </summary>
    public bool LogInventoryEvents { get; set; } = true;

    /// <summary> Should <see cref="EventLogger.EventType.Quest"/> events be logged </summary>
    public bool LogQuestEvents { get; set; } = true;
}
