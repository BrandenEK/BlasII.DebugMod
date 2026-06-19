using BlasII.ModdingAPI;
using System.Linq;
using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

internal class HitboxRenderer
{
    private readonly HitboxViewerSettings _settings;

    private Camera _camera;
    private Material _material;
    private Bounds _camBounds;

    private Collider2D[] _cachedColliders = null;
    private bool _isShowing = false;

    public HitboxRenderer(HitboxViewerSettings settings)
    {
        _settings = settings;
        CacheLineMaterial();
    }

    public void UpdateColliders(Collider2D[] colliders)
    {
        _cachedColliders = colliders;
    }

    public void UpdateStatus(bool isShowing)
    {
        _isShowing = isShowing;
    }

    private void CacheCameraComponent()
    {
        _camera = Object.FindObjectsOfType<Camera>().First(x => x.name == "scene composition camera");
    }

    private void CacheLineMaterial()
    {
        // Unity has a built-in shader that is useful for drawing simple colored things
        var shader = Shader.Find("Hidden/Internal-Colored");
        _material = new Material(shader);
        _material.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        _material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        _material.SetInt("_ZWrite", 0);
    }

    private void CacheCameraBounds()
    {
        float height = _camera.orthographicSize;
        float width = _camera.aspect * height;
        _camBounds = new(_camera.transform.position, new Vector3(width, height) * 2);
    }

    public void OnPostRender(Camera cam)
    {
        if (_camera == null)
            CacheCameraComponent();

        if (!_isShowing || cam != _camera || _cachedColliders == null)
            return;

        ModLog.Info("Rendering for " + cam.name);

        _material.SetPass(0);
        CacheCameraBounds();

        GL.LoadOrtho();
        GL.Begin(1);

        // ...

        GL.Color(Color.red);
        GL.Vertex(new Vector3(0, 0, 1));
        GL.Vertex(new Vector3(1, 1, 1));

        GL.End();
    }
}
