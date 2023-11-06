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
            _line.sortingLayerName = "Before Player";
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
                case HitboxType.Geometry:
                    color = Main.DebugMod.DebugSettings.geometryColor;
                    order = 30;
                    break;
                case HitboxType.Player:
                    color = Main.DebugMod.DebugSettings.playerColor;
                    order = 100;
                    break;
                case HitboxType.Enemy:
                    color = Main.DebugMod.DebugSettings.enemyColor;
                    order = 80;
                    break;
                case HitboxType.Hazard:
                    color = Main.DebugMod.DebugSettings.hazardColor;
                    order = 50;
                    break;
                case HitboxType.Trigger:
                    color = Main.DebugMod.DebugSettings.triggerColor;
                    order = 60;
                    break;
                case HitboxType.Other:
                    color = Main.DebugMod.DebugSettings.otherColor;
                    order = 40;
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
