using BlasII.ModdingAPI;
using Il2CppTGK.Game;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

[MelonLoader.RegisterTypeInIl2Cpp]
public class CameraLines : MonoBehaviour
{
    public Color lineColor = Color.red;

    Material lineMaterial;

    private Camera _camera;

    void Awake()
    {
        // must be called before trying to draw lines..
        CreateLineMaterial();
        _camera = Object.FindObjectsOfType<Camera>().First(x => x.name == "scene composition camera"); //GetComponent<Camera>();
    }

    void CreateLineMaterial()
    {
        // Unity has a built-in shader that is useful for drawing simple colored things
        var shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        lineMaterial.SetInt("_ZWrite", 0);
    }


    // cannot call this on update, line wont be visible then.. and if used OnPostRender() thats works when attached to camera only
    void OnPostRender()
    {
        Stopwatch watch = Stopwatch.StartNew();

        ModLog.Info("ON render post");
        lineMaterial.SetPass(0);
        GL.LoadOrtho();

        //GL.PushMatrix();
        //GL.MultMatrix(transform.localToWorldMatrix);

        if (CoreCache.PlayerSpawn.PlayerInstance == null)
            return;

        Vector3 playerPos = CoreCache.PlayerSpawn.PlayerInstance.transform.position;
        Vector3 better = _camera.WorldToScreenPoint(playerPos);
        //ModLog.Warn("Player: " + better);

        GL.Begin(1);
        GL.Color(lineColor);
        //GL.Vertex(new Vector3(10, 10, 0));
        //GL.Vertex(new Vector3(10, 100, 0));

        //GL.Vertex(new Vector3(0, 1, 0));
        //GL.Vertex(new Vector3(1, 1, 0));

        //GL.Vertex(new Vector2(0, 1));
        //GL.Vertex(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));

        GL.Vertex(new Vector2(0, 1));
        GL.Vertex(new Vector2(better.x / Screen.width, better.y / Screen.height));

        GL.End();
        //GL.PopMatrix();

        foreach (var collider in Object.FindObjectsOfType<Collider2D>())
        {
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
                    //throw new System.Exception("A valid type should be calculated before now!");
            }
        }

        watch.Stop();
        ModLog.Error("Tick: " + watch.ElapsedMilliseconds + " ms");
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

    private Vector3 LocalToWorld(Collider2D collider, Vector2 localPoint)
    {
        Vector3 point = localPoint + collider.offset; // Apply offset
        point = collider.transform.rotation * point; // Apply rotation
        point = Vector2.Scale(point, collider.transform.lossyScale); // Apply scale
        return collider.transform.position + point;
    }

    private Vector2 WorldToPercent(Vector2 worldPoint)
    {
        var screenPoint = _camera.WorldToScreenPoint(worldPoint);
        return new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
    }
}
