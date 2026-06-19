using Il2CppTGK.Game;
using System.Linq;
using UnityEngine;

namespace BlasII.DebugMod.HitboxViewer;

internal class HitboxModule
{
    private readonly HitboxToggler _toggler;
    private readonly HitboxRenderer _renderer;

    private bool _showHitboxes = false;

    public HitboxModule(HitboxViewerSettings settings)
    {
        _toggler = new HitboxToggler();
        _renderer = new HitboxRenderer(_toggler, settings);

        Camera.onPostRender += new System.Action<Camera>(_renderer.OnPostRender);
    }

    public void SceneLoaded()
    {
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
        if (Main.DebugMod.InputHandler.GetKeyDown("HitboxViewer"))
        {
            _showHitboxes = !_showHitboxes;
            _renderer.UpdateStatus(_showHitboxes);
        }

        if (_showHitboxes)
        {
            _toggler.ProcessToggles();

            //if (CoreCache.UIManager.focusedControl?.name != "InGameWindow_prefab(Clone)")
            if (BANNED_UI.Contains(CoreCache.UIManager.focusedControl?.name ?? string.Empty))
                HideHitboxes();
            else
                ShowHitboxes();
        }
    }

    private void ShowHitboxes()
    {
        var colliders = Object.FindObjectsOfType<Collider2D>();
        _renderer.UpdateColliders(colliders);
    }

    private void HideHitboxes()
    {
        _renderer.UpdateColliders(null);
    }

    private static readonly string[] BANNED_UI =
    [
        "InventoryWindow_prefab(Clone)",
        "MapWindow_prefab(Clone)",
        "AltarpieceWidget",
        "FamiliarsWindow_prefab(Clone)",
        "PrieuDieuMenu_prefab(Clone)",
        "GenericMenuWindow_prefab(Clone)",
        "ShopWindow_prefab(Clone)",
    ];
}
