using BlasII.ModdingAPI;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

[MelonLoader.RegisterTypeInIl2Cpp]
internal class CameraLines : MonoBehaviour
{
    private HitboxViewerSettings _settings;
    private Material _material;
    private Camera _camera;

    private Collider2D[] _cachedColliders = null;
    private bool _isShowing = false;

    public void UpdateSettings(HitboxViewerSettings settings)
    {
        _settings = settings;
    }

    public void UpdateColliders(Collider2D[] colliders)
    {
        _cachedColliders = colliders;
    }

    public void UpdateStatus(bool isShowing)
    {
        _isShowing = isShowing;
    }

    void Awake()
    {
        CreateLineMaterial();
        _camera = Object.FindObjectsOfType<Camera>().First(x => x.name == "scene composition camera"); //GetComponent<Camera>();
    }

    private void CreateLineMaterial()
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

    private void OnPostRender()
    {
        if (!_isShowing || _cachedColliders == null)
            return;

        Stopwatch watch = Stopwatch.StartNew();

        _material.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(1);

        foreach (var info in _cachedColliders.Select(CalculateInfo).OrderBy(x => x.Type))
        {
            if (!info.IsVisible)
                continue;

            GL.Color(TypeToColor(info.Type));

            switch (info.Collider.GetIl2CppType().Name)
            {
                case "BoxCollider2D":
                    RenderBox(info.Collider.Cast<BoxCollider2D>());
                    break;
                case "CircleCollider2D":
                    RenderCircle(info.Collider.Cast<CircleCollider2D>());
                    break;
                case "CapsuleCollider2D":
                    RenderCapsule(info.Collider.Cast<CapsuleCollider2D>());
                    break;
                case "PolygonCollider2D":
                    RenderPolygon(info.Collider.Cast<PolygonCollider2D>());
                    break;
                default:
                    break;
            }
        }

        GL.End();

        watch.Stop();
        ModLog.Error("Tick: " + watch.ElapsedTicks + " ticks");
    }

    void RenderBox(BoxCollider2D collider)
    {
        Vector2 halfSize = collider.size / 2f;
        var topLeft = CalculateViewport(collider, new Vector2(-halfSize.x, halfSize.y));
        var topRight = CalculateViewport(collider, new Vector2(halfSize.x, halfSize.y));
        var bottomRight = CalculateViewport(collider, new Vector2(halfSize.x, -halfSize.y));
        var bottomLeft = CalculateViewport(collider, new Vector2(-halfSize.x, -halfSize.y));

        GL.Vertex(topLeft);
        GL.Vertex(topRight);

        GL.Vertex(topRight);
        GL.Vertex(bottomRight);

        GL.Vertex(bottomRight);
        GL.Vertex(bottomLeft);

        GL.Vertex(bottomLeft);
        GL.Vertex(topLeft);
    }

    void RenderCircle(CircleCollider2D collider)
    {
        int segments = 40;
        float angleStep = 2 * Mathf.PI / segments;

        float radius = collider.radius;

        Vector3 start = CalculateViewport(collider, new Vector2(radius, 0));
        GL.Vertex(start);

        for (int i = 1; i < segments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector2 point = CalculateViewport(collider, new Vector2(x, y));
            GL.Vertex(point);
            GL.Vertex(point);
        }

        GL.Vertex(start);
    }

    void RenderCapsule(CapsuleCollider2D collider)
    {
        int segments = 40;
        float angleStep = 2 * Mathf.PI / segments;

        float radius = collider.size.x / 2;
        float height = collider.size.y / 2;

        Vector3 start = CalculateViewport(collider, new Vector2(0, height));
        GL.Vertex(start);

        for (int i = 1; i < segments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * height;

            Vector2 point = CalculateViewport(collider, new Vector2(x, y));
            GL.Vertex(point);
            GL.Vertex(point);
        }

        GL.Vertex(start);
    }

    void RenderPolygon(PolygonCollider2D collider)
    {
        if (collider.pathCount == 0)
            return;

        Vector2[] points = collider.GetPath(0);

        if (points.Length < 3)
            return;

        Vector3 start = CalculateViewport(collider, points[0]);
        GL.Vertex(start);

        for (int i = 1; i < points.Length; i++)
        {
            Vector3 point = CalculateViewport(collider, points[i]);

            GL.Vertex(point);
            GL.Vertex(point);
        }

        GL.Vertex(start);
    }

    private Vector2 CalculateViewport(Collider2D collider, Vector3 point)
    {
        Transform t = collider.transform;
        Vector2 offset = collider.offset;
        Vector3 position = t.position;
        Vector3 scale = t.lossyScale;

        // Apply offset
        point.x += offset.x;
        point.y += offset.y;

        // Apply rotation
        point = t.rotation * point;
        
        // Apply scale
        point.x *= scale.x;
        point.y *= scale.y;

        // Convert to world
        point.x += position.x;
        point.y += position.y;

        return _camera.WorldToViewportPoint(point);
    }

    private Color TypeToColor(HitboxType hitboxType)
    {
        string color = hitboxType switch
        {
            HitboxType.Inactive => _settings.InactiveColor,
            HitboxType.Hazard => _settings.HazardColor,
            HitboxType.Damageable => _settings.DamageableColor,
            HitboxType.Player => _settings.PlayerColor,
            HitboxType.Sensor => _settings.SensorColor,
            HitboxType.Enemy => _settings.EnemyColor,
            HitboxType.Interactable => _settings.InteractableColor,
            HitboxType.Trigger => _settings.TriggerColor,
            HitboxType.Geometry => _settings.GeometryColor,
            HitboxType.Other => _settings.OtherColor,
            _ => throw new System.Exception("A valid type should be calculated before now!"),
        };

        return ColorUtility.TryParseHtmlString(color, out Color c) ? c : Color.white;
    }

    private HitboxInfo CalculateInfo(Collider2D collider)
    {
        // Verify collider still exists
        if (collider == null)
            return new HitboxInfo(collider, HitboxType.Invalid, false);

        // Verify collider is in camera bounds
        Vector2 viewport = _camera.WorldToViewportPoint(collider.transform.position);
        if (viewport.x < -0.5 || viewport.x > 1.5 || viewport.y < -0.5 || viewport.y > 1.5)
            return new HitboxInfo(collider, HitboxType.Invalid, false);

        // Verify collider is a valid size
        Vector2 size = collider.bounds.extents * 2;
        if (collider.enabled && (size.x < _settings.MinSize || size.x > _settings.MaxSize || size.y < _settings.MinSize || size.y > _settings.MaxSize))
            return new HitboxInfo(collider, HitboxType.Invalid, false);

        HitboxType type = collider.GetHitboxType();

        // Verify collider is toggled on
        if (!Main.DebugMod.HitboxModule.ToggledHitboxes[type])
            return new HitboxInfo(collider, type, false);

        return new HitboxInfo(collider, type, true);
    }
}
