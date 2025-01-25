using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

internal class HitboxModule(HitboxViewerSettings settings)
{
    private readonly HitboxViewerSettings _settings = settings;

    internal HitboxToggler ToggledHitboxes { get; } = new();
    private CameraLines _cameraComponent;

    private bool _showHitboxes = false;

    public void SceneLoaded()
    {
        if (Camera.main.GetComponent<CameraLines>() == null)
        {
            _cameraComponent = Camera.main.gameObject.AddComponent<CameraLines>();
            _cameraComponent.UpdateSettings(_settings);
        }


        if (_showHitboxes)
        {
            ShowHitboxes();
        }
    }

    public void SceneUnloaded()
    {
        HideHitboxes();
    }

    public void Update()
    {
        if (_showHitboxes)
        {
            ToggledHitboxes.ProcessToggles();

            //if (Time.frameCount % FRAME_SKIP == 0)
            ShowHitboxes();
        }

        if (Main.DebugMod.InputHandler.GetKeyDown("HitboxViewer"))
        {
            _showHitboxes = !_showHitboxes;
            _cameraComponent.UpdateStatus(_showHitboxes);
            if (_showHitboxes)
                ShowHitboxes();
            else
                HideHitboxes();
        }
    }

    private void ShowHitboxes()
    {
        var colliders = Object.FindObjectsOfType<Collider2D>();
        _cameraComponent.UpdateColliders(colliders);
    }

    private void HideHitboxes()
    {
        _cameraComponent.UpdateColliders(null);
    }

    private const int FRAME_SKIP = 2;
}
