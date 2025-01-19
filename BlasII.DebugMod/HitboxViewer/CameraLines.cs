
using BlasII.ModdingAPI;
using Il2CppTGK.Framework;
using Il2CppTGK.Game;
using System.Linq;
using UnityEngine;
using static UnityEngine.Tilemaps.Tile;

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
        GL.Vertex(new Vector3(10, 10, 0));
        GL.Vertex(new Vector3(10, 100, 0));

        GL.Vertex(new Vector3(0, 1, 0));
        GL.Vertex(new Vector3(1, 1, 0));

        GL.Vertex(new Vector2(0, 1));
        GL.Vertex(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));

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
                    RenderBox(collider.Cast<CapsuleCollider2D>());
                    break;
                case ColliderType.Polygon:
                    RenderBox(collider.Cast<PolygonCollider2D>());
                    break;
                default:
                    break;
                    //throw new System.Exception("A valid type should be calculated before now!");
            }
        }
    }

    void RenderBox(BoxCollider2D collider)
    {
        Vector2 halfSize = collider.size / 2f;
        var topLeft = WorldToPercent(LocalToWorld(collider, new Vector2(-halfSize.x, halfSize.y)));
        var topRight = WorldToPercent(LocalToWorld(collider, new Vector2(halfSize.x, halfSize.y)));
        var bottomRight = WorldToPercent(LocalToWorld(collider, new Vector2(halfSize.x, -halfSize.y)));
        var bottomLeft = WorldToPercent(LocalToWorld(collider, new Vector2(-halfSize.x, -halfSize.y)));
        //ModLog.Error(box.name + ": " + topLeft);

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

    }

    private Vector3 LocalToWorld(Collider2D collider, Vector2 localPoint)
    {
        Vector3 offset = localPoint + collider.offset;
        return collider.transform.position + offset;
    }

    private Vector2 WorldToPercent(Vector2 worldPoint)
    {
        var screenPoint = _camera.WorldToScreenPoint(worldPoint);
        return new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
    }
}
