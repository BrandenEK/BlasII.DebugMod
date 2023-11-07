using BlasII.ModdingAPI.UI;
using Il2CppTGK.Game;
using Il2CppTMPro;
using System;
using UnityEngine;

namespace BlasII.DebugMod.DebugInfo
{
    public class InfoDisplay
    {
        private bool _showInfo = false;

        private TextMeshProUGUI _infoText;

        public void SceneLoaded()
        {
            if (_showInfo && Main.DebugMod.LoadStatus.GameSceneLoaded)
                SetTextVisibility(true);
        }

        public void SceneUnloaded()
        {
            if (_showInfo && Main.DebugMod.LoadStatus.GameSceneLoaded)
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
            string currentScene = Main.DebugMod.LoadStatus.CurrentScene;
            Vector2 playerPosition = CoreCache.PlayerSpawn.PlayerInstance.transform.position;

            _infoText.text = $"Scene: {currentScene}\nPosition: {Math.Round(playerPosition.x, 1)}, {Math.Round(playerPosition.y, 1)}";
        }

        private void SetTextVisibility(bool visible)
        {
            if (_infoText == null)
                CreateText();

            _infoText.gameObject.SetActive(visible);
        }


        private void CreateText()
        {
            _infoText = UIModder.CreateRect("Info Display", UIModder.Parents.GameLogic)
                .SetXRange(0, 0).SetYRange(1, 1).SetPivot(0, 1).SetPosition(20, -235).SetSize(400, 200).AddText()
                .SetFontSize(40).SetAlignment(TextAlignmentOptions.TopLeft);

            _infoText.enableWordWrapping = false;
            //_infoText.outlineColor = new Color32(255, 255, 255, 255);
            //_infoText.outlineWidth = 0.06f;
        }
    }
}
