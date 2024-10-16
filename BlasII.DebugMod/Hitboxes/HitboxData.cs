using UnityEngine;

namespace BlasII.DebugMod.Hitboxes;

internal class HitboxData
{
    private readonly LineRenderer _line;

    public HitboxData(Collider2D collider)
    {
        ColliderType colliderType = collider.GetColliderType();
        HitboxType hitboxType = collider.GetHitboxType();

        // Verify that the collider type should be shown
        if (colliderType == ColliderType.Invalid)
            return;

        // Verify that the hitbox type should be shown
        if (!Main.DebugMod.HitboxViewer.ToggledHitboxes[hitboxType])
            return;

        // Create object as child of collider
        var obj = new GameObject("Hitbox");
        obj.transform.parent = collider.transform;
        obj.transform.localPosition = Vector3.zero;

        // Add line renderer component
        _line = obj.AddComponent<LineRenderer>();
        _line.material = Main.DebugMod.HitboxViewer.HitboxMaterial;
        _line.sortingLayerName = "Foreground Parallax 2";
        _line.useWorldSpace = false;
        _line.SetWidth(LINE_WIDTH, LINE_WIDTH);

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
                color = Main.DebugMod.DebugSettings.HitboxViewer.HazardColor;
                order = 420;
                break;
            case HitboxType.Damageable:
                color = Main.DebugMod.DebugSettings.HitboxViewer.DamageableColor;
                order = 400;
                break;
            case HitboxType.Player:
                color = Main.DebugMod.DebugSettings.HitboxViewer.PlayerColor;
                order = 380;
                break;
            case HitboxType.Sensor:
                color = Main.DebugMod.DebugSettings.HitboxViewer.SensorColor;
                order = 360;
                break;
            case HitboxType.Enemy:
                color = Main.DebugMod.DebugSettings.HitboxViewer.EnemyColor;
                order = 340;
                break;
            case HitboxType.Interactable:
                color = Main.DebugMod.DebugSettings.HitboxViewer.InteractableColor;
                order = 320;
                break;
            case HitboxType.Trigger:
                color = Main.DebugMod.DebugSettings.HitboxViewer.TriggerColor;
                order = 300;
                break;
            case HitboxType.Geometry:
                color = Main.DebugMod.DebugSettings.HitboxViewer.GeometryColor;
                order = 100;
                break;
            case HitboxType.Other:
                color = Main.DebugMod.DebugSettings.HitboxViewer.OtherColor;
                order = 260;
                break;
            default:
                throw new System.Exception("A valid type should be calculated before now!");
        }

        ColorUtility.TryParseHtmlString(color, out Color c);
        _line.SetColors(c, c);
        _line.sortingOrder = order;
    }

    public void DestroyHitbox()
    {
        if (_line != null && _line.gameObject != null)
            Object.Destroy(_line.gameObject);
    }

    private const float LINE_WIDTH = 0.04f;
}
