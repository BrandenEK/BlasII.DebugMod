using System.Collections.Generic;
using UnityEngine;

namespace BlasII.DebugMod.Hitboxes
{
    public class HitboxViewer
    {
        private const float LINE_WIDTH = 0.04f;

        private readonly Dictionary<int, LineRenderer> _activeHitboxes = new();
        private readonly HitboxConfig _config;

        private bool _showHitboxes = false;
        private float _currentDelay = 0f;

        public HitboxColors Colors { get; private set; }

        public HitboxViewer(HitboxConfig config)
        {
            _config = config;
            Colors = new HitboxColors(config.inactiveColor, config.geometryColor, config.playerColor, config.enemyColor, config.hazardColor, config.triggerColor, config.otherColor);
        }

        private void AddHitboxes()
        {
            float newColliders = 0;

            // Foreach collider in the scene, either add it or update it
            var foundColliders = new List<int>();
            foreach (Collider2D collider in Object.FindObjectsOfType<Collider2D>(true))
            {
                // If not showing geometry, skip
                if (!_config.showGeometry && collider.name.StartsWith("GEO_"))
                    continue;

                // If collider type is not valid, skip
                string colliderType = collider.GetIl2CppType().Name;
                if (!validColliders.Contains(colliderType))
                    continue;

                // If collider is too large, skip

                if (_activeHitboxes.TryGetValue(collider.gameObject.GetInstanceID(), out LineRenderer line))
                {
                    // If the collider is already stored, just update the colors
                    line.UpdateColors(collider);
                }
                else
                {
                    // Move this out into helper function
                    var obj = new GameObject("Hitbox");
                    obj.transform.parent = collider.transform;
                    obj.transform.localPosition = Vector3.zero;

                    line = obj.AddComponent<LineRenderer>();
                    line.material = HitboxMaterial;
                    line.useWorldSpace = false;
                    line.SetWidth(LINE_WIDTH, LINE_WIDTH);

                    switch (colliderType)
                    {
                        case "BoxCollider2D":
                            line.DisplayBox(collider.Cast<BoxCollider2D>());
                            break;
                        case "CircleCollider2D":
                            line.DisplayCircle(collider.Cast<CircleCollider2D>());
                            break;
                        case "CapsuleCollider2D":
                            line.DisplayCapsule(collider.Cast<CapsuleCollider2D>());
                            break;
                        case "PolygonCollider2D":
                            line.DisplayPolygon(collider.Cast<PolygonCollider2D>());
                            break;
                    }
                    line.UpdateColors(collider);

                    _activeHitboxes.Add(collider.gameObject.GetInstanceID(), line);
                    newColliders++;
                }

                foundColliders.Add(collider.gameObject.GetInstanceID());
            }

            // Foreach collider in the list that wasn't found, remove it
            var destroyedColliders = new List<int>();
            foreach (int colliderId in _activeHitboxes.Keys)
            {
                if (!foundColliders.Contains(colliderId))
                    destroyedColliders.Add(colliderId);
            }
            foreach (int colliderId in destroyedColliders)
            {
                _activeHitboxes.Remove(colliderId);
            }

            // Log amounts and reset timer
            if (destroyedColliders.Count > 0)
                Main.DebugMod.Log($"Removing {destroyedColliders.Count} old colliders");
            if (newColliders > 0)
                Main.DebugMod.Log($"Adding {newColliders} new colliders");
            _currentDelay = 0;
        }

        private void RemoveHitboxes()
        {
            foreach (LineRenderer hitbox in _activeHitboxes.Values)
            {
                if (hitbox != null && hitbox.gameObject != null)
                    Object.Destroy(hitbox.gameObject);
            }

            _activeHitboxes.Clear();
        }

        public void SceneLoaded()
        {
            StoreHitboxImage();

            if (_showHitboxes)
                AddHitboxes();
        }

        public void SceneUnloaded()
        {
            RemoveHitboxes();
        }

        public void Update()
        {
            if (_showHitboxes && Main.DebugMod.LoadStatus.GameSceneLoaded)
            {
                _currentDelay += Time.deltaTime;
                if (_currentDelay >= _config.updateDelay)
                {
                    AddHitboxes();
                }
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                _showHitboxes = !_showHitboxes;
                if (_showHitboxes)
                    AddHitboxes();
                else
                    RemoveHitboxes();
            }
        }

        private Sprite _hitboxImage;
        public Sprite HitboxImage => _hitboxImage;

        private void StoreHitboxImage()
        {
            var tex = new Texture2D(1, 1, TextureFormat.RGB24, false);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            _hitboxImage = Sprite.Create(tex, new Rect(0, 0, 1, 1), Vector2.zero, 1, 0, SpriteMeshType.FullRect);
        }

        private Material _hitboxMaterial;
        public Material HitboxMaterial
        {
            get
            {
                if (_hitboxMaterial == null)
                {
                    var obj = new GameObject("Temp");
                    _hitboxMaterial = obj.AddComponent<SpriteRenderer>().material;
                    Object.Destroy(obj);
                }
                return _hitboxMaterial;
            }
        }

        private readonly List<string> validColliders = new()
        {
            "BoxCollider2D",
            "CircleCollider2D",
            "CapsuleCollider2D",
            "PolygonCollider2D",
        };
    }
}
