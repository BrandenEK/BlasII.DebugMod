using BlasII.ModdingAPI;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

[MelonLoader.RegisterTypeInIl2Cpp]
internal class CameraLines : MonoBehaviour
{
    private Material _material;
    private Camera _camera;

    private Collider2D[] _cachedColliders = null;
    private bool _isShowing = false;

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

        Plane[] planes = new Plane[6];
        GeometryUtility.CalculateFrustumPlanes(_camera, planes);

        foreach (var collider in _cachedColliders)
        {
            if (collider == null)
                continue;

            if (!GeometryUtility.TestPlanesAABB(planes, collider.bounds))
            {
                ModLog.Warn("Skipping col: " + collider.name);
                continue;
            }

            //Vector2 viewport = _camera.WorldToViewportPoint(collider.transform.position);
            //if (viewport.x < -0.5 || viewport.x > 1.5 || viewport.y < -0.5 || viewport.y > 1.5)
            //    continue;

            ColliderType colliderType = collider.GetColliderType();
            switch (colliderType)
            {
                case ColliderType.Box:
                    RenderBox(collider.Cast<BoxCollider2D>());
                    break;
                case ColliderType.Circle:
                    RenderCircle(collider.Cast<CircleCollider2D>());
                    break;
                case ColliderType.Capsule:
                    RenderCapsule(collider.Cast<CapsuleCollider2D>());
                    break;
                case ColliderType.Polygon:
                    RenderPolygon(collider.Cast<PolygonCollider2D>());
                    break;
                default:
                    break;
            }
        }

        watch.Stop();
        ModLog.Error("Tick: " + watch.ElapsedTicks + " ticks");
    }

    void RenderBox(BoxCollider2D collider)
    {
        Vector2 halfSize = collider.size / 2f;
        var topLeft = WorldToPercent(LocalToWorld(collider, new Vector2(-halfSize.x, halfSize.y)));
        var topRight = WorldToPercent(LocalToWorld(collider, new Vector2(halfSize.x, halfSize.y)));
        var bottomRight = WorldToPercent(LocalToWorld(collider, new Vector2(halfSize.x, -halfSize.y)));
        var bottomLeft = WorldToPercent(LocalToWorld(collider, new Vector2(-halfSize.x, -halfSize.y)));

        GL.Begin(1);
        GL.Color(Color.green);

        GL.Vertex(topLeft);
        GL.Vertex(topRight);

        GL.Vertex(topRight);
        GL.Vertex(bottomRight);

        GL.Vertex(bottomRight);
        GL.Vertex(bottomLeft);

        GL.Vertex(bottomLeft);
        GL.Vertex(topLeft);

        GL.End();
    }

    void RenderCircle(CircleCollider2D collider)
    {
        int segments = 80;
        float radius = collider.radius;

        Vector3 start = WorldToPercent(LocalToWorld(collider, new Vector2(radius, 0)));
        Vector3 previous = start;
        GL.Begin(1);
        GL.Color(Color.blue);

        for (int currentStep = 1; currentStep < segments; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / (segments - 1);
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            var currentPosition = new Vector2(radius * xScaled, radius * yScaled);
            Vector3 current = WorldToPercent(LocalToWorld(collider, currentPosition));

            GL.Vertex(previous);
            GL.Vertex(current);

            previous = current;
        }

        GL.Vertex(previous);
        GL.Vertex(start);

        GL.End();
    }

    void RenderCapsule(CapsuleCollider2D collider)
    {
        int segments = 80;
        float xRadius = collider.size.x / 2;
        float yRadius = collider.size.y / 2;
        float currAngle = 20f;

        Vector3 start = Vector3.zero;
        GL.Begin(1);
        GL.Color(Color.yellow);

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * currAngle) * xRadius;
            float y = Mathf.Cos(Mathf.Deg2Rad * currAngle) * yRadius;

            Vector3 point = WorldToPercent(LocalToWorld(collider, new Vector2(x, y)));
            currAngle += (360f / segments);

            if (i == 0)
            {
                start = point;
                GL.Vertex(point);
                continue;
            }

            GL.Vertex(point);
            GL.Vertex(point);
        }

        GL.Vertex(start);
        GL.End();
    }

    void RenderPolygon(PolygonCollider2D collider)
    {
        GL.Begin(1);
        GL.Color(Color.red);

        if (collider.pathCount == 0)
            return;

        Vector2[] points = collider.GetPath(0);

        if (points.Length < 3)
            return;

        Vector3 start = WorldToPercent(LocalToWorld(collider, points[0]));
        GL.Vertex(start);

        for (int i = 1; i < points.Length; i++)
        {
            Vector3 point = WorldToPercent(LocalToWorld(collider, points[i]));

            GL.Vertex(point);
            GL.Vertex(point);
        }

        GL.Vertex(start);
        GL.End();
    }

    private Vector3 LocalToWorld(Collider2D collider, Vector3 point)
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

        return point;
    }

    private Vector2 WorldToPercent(Vector2 worldPoint)
    {
        return _camera.WorldToViewportPoint(worldPoint);
    }
}
