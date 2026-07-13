using BlasII.Framework.UI;
using BlasII.ModdingAPI.Assets;
using BlasII.ModdingAPI.Helpers;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Attack;
using Il2CppTGK.Game.Components.UI;
using Il2CppTMPro;
using System;
using System.Text;
using UnityEngine;

namespace BlasII.DebugMod.InfoDisplay;

internal class InfoModule(InfoDisplaySettings settings)
{
    private readonly InfoDisplaySettings _settings = settings;
    private readonly FpsTracker _fpsTracker = new FpsTracker();

    private bool _showInfo = false;
    private UIPixelTextWithShadow _infoText;

    private AttackData _lastOffense = null;
    private AttackData _lastDefense = null;

    public void SceneLoaded()
    {
        if (_showInfo && SceneHelper.GameSceneLoaded)
            SetTextVisibility(true);

        _lastOffense = null;
        _lastDefense = null;

        _fpsTracker.Reset();
    }

    public void SceneUnloaded()
    {
        if (_showInfo && SceneHelper.GameSceneLoaded)
            SetTextVisibility(false);

        _lastOffense = null;
        _lastDefense = null;
    }

    public void Update()
    {
        if (Main.DebugMod.InputHandler.GetKeyDown("InfoDisplay"))
        {
            _showInfo = !_showInfo;
            SetTextVisibility(_showInfo);
        }

        _fpsTracker.Update();

        if (_showInfo && CoreCache.PlayerSpawn.PlayerInstance != null)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        var sb = new StringBuilder();
        
        if (_settings.ShowGeneral)
        {
            // General
            sb.AppendLine("<color=#FFE741>General</color>");

            // Scene
            string currentScene = SceneHelper.CurrentScene;
            sb.AppendLine($"Scene: {currentScene}");

            // FPS
            float fps = _fpsTracker.CurrentFps;
            sb.AppendLine($"FPS: {fps:F0}");
        }

        if (_settings.ShowPlayer)
        {
            // Player
            sb.AppendLine();
            sb.AppendLine("<color=#FFE741>Player</color>");

            // Position
            Vector2 playerPosition = CoreCache.PlayerSpawn.PlayerInstance.transform.position;
            sb.AppendLine($"Position: {RoundToPrecision(playerPosition.x)}, {RoundToPrecision(playerPosition.y)}");

            // Health
            int currentHealth = AssetStorage.PlayerStats.GetCurrentValue(AssetStorage.RangeStats["Health"]);
            int maxHealth = AssetStorage.PlayerStats.GetMaxValue(AssetStorage.RangeStats["Health"]);
            sb.AppendLine($"Health: {currentHealth}/{maxHealth}");

            // Fervour
            int currentFervour = AssetStorage.PlayerStats.GetCurrentValue(AssetStorage.RangeStats["Fervour"]);
            int maxFervour = AssetStorage.PlayerStats.GetMaxValue(AssetStorage.RangeStats["Fervour"]);
            sb.AppendLine($"Fervour: {currentFervour}/{maxFervour}");

            // Iframes
            //var comp = CoreCache.PlayerSpawn.PlayerInstance.GetComponentInChildren<AttackReceiverComponent>();
            //if (comp != null)
            //{
            //    bool inv = comp.IsInvincible();
            //    float time = comp.currentInvincibilityTimeleft;
            //    sb.AppendLine($"Invincible: {(inv ? $"Yes for {RoundToPrecision(time)}s" : "No")}");
            //}
        }

        if (_settings.ShowFamiliar && CoreCache.PlayerFamiliarsManager.currentID != -1)
        {
            // Familiar
            sb.AppendLine();
            sb.AppendLine("<color=#FFE741>Familiar</color>");

            // ID
            var familiar = CoreCache.PlayerFamiliarsManager.familiars[CoreCache.PlayerFamiliarsManager.currentID];
            sb.AppendLine($"ID: {familiar.id.name}");

            // Level
            int level = familiar.currentLevel;
            sb.AppendLine($"Level: {level + 1}/4");

            // EXP
            int xp = familiar.currentExp;
            sb.AppendLine($"EXP: {xp}{level switch { 0 => "/100", 1 => "/400", 2 => "/900", _ => string.Empty }}");
        }

        if (_settings.ShowOffense && _lastOffense != null)
        {
            // Offense data
            sb.AppendLine();
            sb.AppendLine("<color=#FFE741>Last offense</color>");

            if (!string.IsNullOrEmpty(_lastOffense.ID))
                sb.AppendLine($"ID: {_lastOffense.ID}");
            sb.AppendLine($"Damage: {_lastOffense.Damage}");
            if (!string.IsNullOrEmpty(_lastOffense.PhysicalAttacks))
                sb.AppendLine($"Physical: {_lastOffense.PhysicalAttacks}");
            if (!string.IsNullOrEmpty(_lastOffense.ElementalAttacks))
                sb.AppendLine($"Elemental: {_lastOffense.ElementalAttacks}");
        }

        if (_settings.ShowDefense && _lastDefense != null)
        {
            // Defense data
            sb.AppendLine();
            sb.AppendLine("<color=#FFE741>Last defense</color>");

            if (!string.IsNullOrEmpty(_lastDefense.ID))
                sb.AppendLine($"ID: {_lastDefense.ID}");
            sb.AppendLine($"Damage: {_lastDefense.Damage}");
            if (!string.IsNullOrEmpty(_lastDefense.PhysicalAttacks))
                sb.AppendLine($"Physical: {_lastDefense.PhysicalAttacks}");
            if (!string.IsNullOrEmpty(_lastDefense.ElementalAttacks))
                sb.AppendLine($"Elemental: {_lastDefense.ElementalAttacks}");
        }

        try
        {
            _infoText.SetText(sb.ToString());
        }
        catch (Exception) { } // Quitting the game throws an error inside SetText()
    }

    public void OnOffense(AttackData attack)
    {
        _lastOffense = attack;
    }

    public void OnDefense(AttackData attack)
    {
        _lastDefense = attack;
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
            Color = new Color32(222, 222, 222, 255),
            FontSize = 32,
            UseRichText = true,
            WordWrap = false,
        }).AddShadow();
    }

    private string RoundToPrecision(float num)
    {
        int precision = System.Math.Max(_settings.Precision, 1);
        return num.ToString("F" + precision);
    }
}
