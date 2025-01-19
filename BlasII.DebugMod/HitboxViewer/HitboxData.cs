using BlasII.ModdingAPI;
using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

internal class HitboxData
{
    private readonly LineRenderer _line;

    public HitboxData(Collider2D collider, HitboxViewerSettings settings)
    {
        ColliderType colliderType = collider.GetColliderType();
        HitboxType hitboxType = collider.GetHitboxType();

        // Verify that the collider type should be shown
        if (colliderType == ColliderType.Invalid)
            return;

        // Verify that the hitbox type should be shown
        if (!Main.DebugMod.HitboxModule.ToggledHitboxes[hitboxType])
            return;

        // Add line renderer component
        _line = collider.gameObject.AddComponent<LineRenderer>();
        _line.material = Main.DebugMod.HitboxModule.HitboxMaterial;
        _line.sortingLayerName = "Foreground Parallax 2";
        _line.useWorldSpace = false;
        _line.SetWidth(LINE_WIDTH, LINE_WIDTH);

        // Debug info
        ModLog.Info($"{collider.name} (collider={colliderType}, hitbox={hitboxType})");
        ModLog.Warn($"position: {collider.transform.position}");
        ModLog.Warn($"localRotation: {collider.transform.localRotation}");
        ModLog.Warn($"lossyScale: {collider.transform.lossyScale}");

        // Set up drawing based on collider type
        switch (colliderType)
        {
            case ColliderType.Box:
                _line.DisplayBox(collider.Cast<BoxCollider2D>());
                break;
            case ColliderType.Circle:
                _line.DisplayCircle(collider.Cast<CircleCollider2D>());
                break;
            case ColliderType.Capsule:
                _line.DisplayCapsule(collider.Cast<CapsuleCollider2D>());
                break;
            case ColliderType.Polygon:
                _line.DisplayPolygon(collider.Cast<PolygonCollider2D>());
                break;
            default:
                throw new System.Exception("A valid type should be calculated before now!");
        }

        // Change color and order based on hitbox type
        string color;
        int order;
        switch (hitboxType)
        {
            case HitboxType.Hazard:
                color = settings.HazardColor;
                order = 420;
                break;
            case HitboxType.Damageable:
                color = settings.DamageableColor;
                order = 400;
                break;
            case HitboxType.Player:
                color = settings.PlayerColor;
                order = 380;
                break;
            case HitboxType.Sensor:
                color = settings.SensorColor;
                order = 360;
                break;
            case HitboxType.Enemy:
                color = settings.EnemyColor;
                order = 340;
                break;
            case HitboxType.Interactable:
                color = settings.InteractableColor;
                order = 320;
                break;
            case HitboxType.Trigger:
                color = settings.TriggerColor;
                order = 300;
                break;
            case HitboxType.Geometry:
                color = settings.GeometryColor;
                order = 100;
                break;
            case HitboxType.Other:
                color = settings.OtherColor;
                order = 260;
                break;
            default:
                throw new System.Exception("A valid type should be calculated before now!");
        }

        if (ColorUtility.TryParseHtmlString(color, out Color c))
            _line.SetColors(c, c);
        _line.sortingOrder = order;
    }

    public void DestroyHitbox()
    {
        if (_line != null)
            Object.Destroy(_line);
    }

    private const float LINE_WIDTH = 0.04f;
}
