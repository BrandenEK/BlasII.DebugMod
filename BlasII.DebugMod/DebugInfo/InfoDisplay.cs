using BlasII.Framework.UI;
using BlasII.ModdingAPI.Assets;
using BlasII.ModdingAPI.Helpers;
using Il2CppTGK.Game;
using Il2CppTMPro;
using System.Text;
using UnityEngine;

namespace BlasII.DebugMod.DebugInfo;

internal class InfoDisplay
{
    private bool _showInfo = false;

    private TextMeshProUGUI _infoText;

    public void SceneLoaded()
    {
        if (_showInfo && SceneHelper.GameSceneLoaded)
            SetTextVisibility(true);
    }

    public void SceneUnloaded()
    {
        if (_showInfo && SceneHelper.GameSceneLoaded)
            SetTextVisibility(false);
    }

    public void Update()
    {
        if (Main.DebugMod.InputHandler.GetKeyDown("InfoDisplay"))
        {
            _showInfo = !_showInfo;
            SetTextVisibility(_showInfo);
        }

        if (_showInfo)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        var sb = new StringBuilder();

        // Scene
        string currentScene = SceneHelper.CurrentScene;
        sb.AppendLine($"Scene: {currentScene}");

        // Position
        Vector2 playerPosition = CoreCache.PlayerSpawn.PlayerInstance.transform.position;
        sb.AppendLine($"Position: {playerPosition.x.RoundToPrecision()}, {playerPosition.y.RoundToPrecision()}");

        // Health
        int currentHealth = AssetStorage.PlayerStats.GetCurrentValue(AssetStorage.RangeStats["Health"]);
        int maxHealth = AssetStorage.PlayerStats.GetMaxValue(AssetStorage.RangeStats["Health"]);
        sb.AppendLine($"Health: {currentHealth}/{maxHealth}");

        // Fervour
        int currentFervour = AssetStorage.PlayerStats.GetCurrentValue(AssetStorage.RangeStats["Fervour"]);
        int maxFervour = AssetStorage.PlayerStats.GetMaxValue(AssetStorage.RangeStats["Fervour"]);
        sb.AppendLine($"Fervour: {currentFervour}/{maxFervour}");

        _infoText.text = sb.ToString();
    }

    private void SetTextVisibility(bool visible)
    {
        if (_infoText == null)
            CreateText();

        _infoText.gameObject.SetActive(visible);
    }

    private void CreateText()
    {
        _infoText = UIModder.Create(new RectCreationOptions()
        {
            Name = "Info Display",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(400, 200),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(20, -235),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddText(new TextCreationOptions()
        {
            Alignment = TextAlignmentOptions.TopLeft,
            FontSize = 40,
            WordWrap = false,
        });
    }
}
