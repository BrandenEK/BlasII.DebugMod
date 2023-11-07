using UnityEngine;

namespace BlasII.DebugMod.Hitboxes
{
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
            if (hitboxType == HitboxType.Geometry && !Main.DebugMod.DebugSettings.geometryShow ||
                hitboxType == HitboxType.Player && !Main.DebugMod.DebugSettings.playerShow ||
                hitboxType == HitboxType.Enemy && !Main.DebugMod.DebugSettings.enemyShow ||
                hitboxType == HitboxType.Hazard && !Main.DebugMod.DebugSettings.hazardShow ||
                hitboxType == HitboxType.Trigger && !Main.DebugMod.DebugSettings.triggerShow ||
                hitboxType == HitboxType.Other && !Main.DebugMod.DebugSettings.otherShow)
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
            Color color;
            int order;
            switch (hitboxType)
            {
                case HitboxType.Hazard:
                    color = ColorUtility.TryParseHtmlString("#FF007F", out Color c8) ? c8 : Color.white;
                    order = 420;
                    break;
                case HitboxType.Damageable:
                    color = ColorUtility.TryParseHtmlString("#FFA500", out Color c1) ? c1 : Color.white;
                    order = 400;
                    break;
                case HitboxType.Player:
                    color = ColorUtility.TryParseHtmlString("#00CCCC", out Color c9) ? c9 : Color.white;
                    order = 380;
                    break;
                case HitboxType.Sensor:
                    color = ColorUtility.TryParseHtmlString("#660066", out Color c2) ? c2 : Color.white;
                    order = 365;
                    break;
                case HitboxType.Enemy:
                    color = ColorUtility.TryParseHtmlString("#DD0000", out Color c7) ? c7 : Color.white;
                    order = 360;
                    break;
                case HitboxType.Interactable:
                    color = ColorUtility.TryParseHtmlString("#FFFF33", out Color c6) ? c6 : Color.white;
                    order = 340;
                    break;
                case HitboxType.Trigger:
                    color = ColorUtility.TryParseHtmlString("#0066CC", out Color c4) ? c4 : Color.white;
                    order = 300;
                    break;
                case HitboxType.Geometry:
                    color = ColorUtility.TryParseHtmlString("#00CC00", out Color c5) ? c5 : Color.white;
                    order = 280;
                    break;
                case HitboxType.Other:
                    color = ColorUtility.TryParseHtmlString("#000099", out Color c3) ? c3 : Color.white;
                    order = 260;
                    break;
                default:
                    throw new System.Exception("A valid type should be calculated before now!");
            }
            _line.SetColors(color, color);
            _line.sortingOrder = order;
        }

        public void DestroyHitbox()
        {
            if (_line != null && _line.gameObject != null)
                Object.Destroy(_line.gameObject);
        }

        private const float LINE_WIDTH = 0.04f;
    }
}
