﻿using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.Attack;
using Il2CppTGK.Game.Components.Collisions;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Components.Persistence;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.DebugMod.Hitboxes
{
    public static class HitboxExtensions
    {
        public static bool HasComponentInParent<T>(this Transform transform)
        {
            Transform parent = transform;
            while (parent != null)
            {
                if (parent.GetComponent<T>() != null)
                    return true;
                parent = parent.parent;
            }

            return false;
        }

        // BoxCollider2D
        public static void DisplayBox(this LineRenderer renderer, BoxCollider2D collider)
        {
            // Skip colliders that are too large
            if (collider.size.x >= 15 || collider.size.y >= 15 || collider.size.x <= 0.1f || collider.size.y <= 0.1f)
                return;

            Vector2 halfSize = collider.size / 2f;
            Vector2 topLeft = new(-halfSize.x, halfSize.y);
            Vector2 topRight = halfSize;
            Vector2 bottomRight = new(halfSize.x, -halfSize.y);
            Vector2 bottomLeft = -halfSize;
            Vector2[] points = new Vector2[]
            {
                topLeft, topRight, bottomRight, bottomLeft, topLeft
            };

            renderer.positionCount = 5;
            for (int i = 0; i < points.Length; i++)
            {
                renderer.SetPosition(i, collider.offset + points[i]);
            }
        }

        // CircleCollider2D
        public static void DisplayCircle(this LineRenderer renderer, CircleCollider2D collider)
        {
            // Skip colliders that are too large
            if (collider.radius >= 5 || collider.radius <= 0.1f)
                return;

            int segments = 80;
            float radius = collider.radius;

            renderer.positionCount = segments;
            for (int currentStep = 0; currentStep < segments; currentStep++)
            {
                float circumferenceProgress = (float)currentStep / (segments - 1);

                float currentRadian = circumferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);

                var currentPosition = new Vector2(radius * xScaled, radius * yScaled);
                renderer.SetPosition(currentStep, collider.offset + currentPosition);
            }
        }

        // CapsuleCollider2D
        public static void DisplayCapsule(this LineRenderer renderer, CapsuleCollider2D collider)
        {
            int segments = 80;
            float xRadius = collider.size.x / 2;
            float yRadius = collider.size.y / 2;
            float currAngle = 20f;

            renderer.positionCount = segments + 1;
            for (int i = 0; i < (segments + 1); i++)
            {
                float x = Mathf.Sin(Mathf.Deg2Rad * currAngle) * xRadius;
                float y = Mathf.Cos(Mathf.Deg2Rad * currAngle) * yRadius;

                renderer.SetPosition(i, collider.offset + new Vector2(x, y));
                currAngle += (360f / segments);
            }
        }

        // PolygonCollider2D
        public static void DisplayPolygon(this LineRenderer renderer, PolygonCollider2D collider)
        {
            // Skip empty polygons
            if (collider.pathCount == 0)
                return;

            var points = new List<Vector2>(collider.GetPath(0));
            if (points.Count > 0)
                points.Add(points[0]);

            renderer.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                renderer.SetPosition(i, collider.offset + points[i]);
            }
        }

        public static HitboxType GetHitboxType(this Collider2D collider)
        {
            if (collider.transform.GetComponent<AttackHit>() != null)
            {
                return HitboxType.Hazard;
            }
            if (collider.transform.HasComponentInParent<CollisionsCallback>())
            {
                return HitboxType.Damageable;
            }
            if (collider.transform.HasComponentInParent<PlayerPersistentComponent>())
            {
                return HitboxType.Player;
            }
            if (collider.transform.HasComponentInParent<TriggersCallback>())
            {
                return HitboxType.Sensor;
            }
            if (collider.transform.HasComponentInParent<AliveEntity>())
            {
                return HitboxType.Enemy;
            }
            if (collider.transform.HasComponentInParent<IInteractable>())
            {
                return HitboxType.Interactable;
            }
            if (collider.isTrigger)
            {
                return HitboxType.Trigger;
            }
            if (collider.name.StartsWith("GEO_"))
            {
                return HitboxType.Geometry;
            }

            return HitboxType.Other;
        }

        public static ColliderType GetColliderType(this Collider2D collider)
        {
            return collider.GetIl2CppType().Name switch
            {
                "BoxCollider2D" => ColliderType.Box,
                "CircleCollider2D" => ColliderType.Circle,
                "CapsuleCollider2D" => ColliderType.Capsule,
                "PolygonCollider2D" => ColliderType.Polygon,
                _ => ColliderType.Invalid,
            };
        }
    }
}
