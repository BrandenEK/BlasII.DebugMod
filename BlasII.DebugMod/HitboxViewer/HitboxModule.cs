using BlasII.ModdingAPI;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

internal class HitboxModule(HitboxViewerSettings settings)
{
    private readonly HitboxViewerSettings _settings = settings;
    private readonly Dictionary<int, HitboxData> _activeHitboxes = new();

    internal HitboxToggler ToggledHitboxes { get; } = new();

    private bool _showHitboxes = false;
    private float _currentDelay = 0f;

    private void AddHitboxes()
    {
        // Foreach collider in the scene, add a HitboxData if it doesnt already have one
        var foundColliders = new List<int>();
        foreach (Collider2D collider in Object.FindObjectsOfType<Collider2D>())
        {
            int id = collider.gameObject.GetInstanceID();
            foundColliders.Add(id);

            if (_activeHitboxes.TryGetValue(id, out HitboxData data))
            {
                data.UpdateHitbox(_settings);
            }
            else
            {
                _activeHitboxes.Add(id, new HitboxData(collider, _settings));
            }
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
            _activeHitboxes[colliderId].DestroyHitbox();
            _activeHitboxes.Remove(colliderId);
        }

        // Reset timer
        _currentDelay = 0;
    }

    private void RemoveHitboxes()
    {
        foreach (HitboxData hitbox in _activeHitboxes.Values)
        {
            hitbox.DestroyHitbox();
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
        if (_showHitboxes)
        {
            if (ToggledHitboxes.ProcessToggles())
            {
                RemoveHitboxes();
                AddHitboxes();
            }

            CheckRefreshHitboxes();
        }

        if (Main.DebugMod.InputHandler.GetKeyDown("HitboxViewer"))
        {
            _showHitboxes = !_showHitboxes;
            if (_showHitboxes)
                AddHitboxes();
            else
                RemoveHitboxes();
        }
    }

    private void CheckRefreshHitboxes()
    {
        _currentDelay += Time.deltaTime;
        if (_currentDelay < _settings.UpdateDelay)
            return;

        Stopwatch watch = Stopwatch.StartNew();
        AddHitboxes();
        watch.Stop();
#if DEBUG
        ModLog.Info($"Refresh tick: {watch.ElapsedMilliseconds} ms");
#endif
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
}
