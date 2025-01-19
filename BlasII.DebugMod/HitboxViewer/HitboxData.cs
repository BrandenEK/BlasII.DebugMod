using BlasII.ModdingAPI;
using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

internal class HitboxData
{
    private readonly LineRenderer _line;
    private readonly Collider2D _collider;

    public HitboxData(Collider2D collider, HitboxViewerSettings settings)
    {
        ColliderType colliderType = collider.GetColliderType();
        HitboxType hitboxType = collider.GetHitboxType(settings);

        // Add line renderer component
        _line = collider.gameObject.AddComponent<LineRenderer>();
        _line.material = Main.DebugMod.HitboxModule.HitboxMaterial;
        _line.sortingLayerName = "Foreground Parallax 2";
        _line.useWorldSpace = false;
        _line.SetWidth(LINE_WIDTH, LINE_WIDTH);
        _collider = collider;

        // Debug info
        //ModLog.Info($"{collider.name} (collider={colliderType}, hitbox={hitboxType})");
        //ModLog.Warn($"position: {collider.transform.position}");
        //ModLog.Warn($"localRotation: {collider.transform.localRotation}");
        //ModLog.Warn($"lossyScale: {collider.transform.lossyScale}");

        UpdateHitbox(settings);
    }

    public void UpdateHitbox(HitboxViewerSettings settings)
    {
        // Verify that the collider is a valid size
        if (_collider.bounds.extents.x * 2 < settings.MinSize || _collider.bounds.extents.x * 2 > settings.MaxSize ||
            _collider.bounds.extents.y * 2 < settings.MinSize || _collider.bounds.extents.y * 2 > settings.MaxSize)
        {
            _line.positionCount = 0;
            return;
        }

        ColliderType colliderType = _collider.GetColliderType();
        HitboxType hitboxType = _collider.GetHitboxType(settings);

        // Verify that the collider type should be shown
        if (colliderType == ColliderType.Invalid)
        {
            _line.positionCount = 0;
            return;
        }

        // Verify that the hitbox type should be shown
        if (!Main.DebugMod.HitboxModule.ToggledHitboxes[hitboxType])
        {
            _line.positionCount = 0;
            return;
        }

        SetLines(colliderType);
        SetColor(hitboxType, settings);
    }

    public void DestroyHitbox()
    {
        if (_line != null)
            Object.Destroy(_line);
    }

    private void SetLines(ColliderType colliderType)
    {
        // Set up drawing based on collider type
        switch (colliderType)
        {
            case ColliderType.Box:
                _line.DisplayBox(_collider.Cast<BoxCollider2D>());
                break;
            case ColliderType.Circle:
                _line.DisplayCircle(_collider.Cast<CircleCollider2D>());
                break;
            case ColliderType.Capsule:
                _line.DisplayCapsule(_collider.Cast<CapsuleCollider2D>());
                break;
            case ColliderType.Polygon:
                _line.DisplayPolygon(_collider.Cast<PolygonCollider2D>());
                break;
            default:
                throw new System.Exception("A valid type should be calculated before now!");
        }
    }

    private void SetColor(HitboxType hitboxType, HitboxViewerSettings settings)
    {
        // Change color and order based on hitbox type
        string color;
        int order;
        switch (hitboxType)
        {
            case HitboxType.Inactive:
                color = settings.InactiveColor;
                order = 180;
                break;
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
                order = 200;
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

    private const float LINE_WIDTH = 0.04f;
}
