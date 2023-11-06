using BlasII.ModdingAPI.Input;
using Il2CppLightbug.Kinematic2D.Core;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components;
using UnityEngine;

namespace BlasII.DebugMod.NoClip
{
    public class NoClipper
    {
        private readonly NoclipConfig _config;

        private bool _canMovePlayer;
        private Vector3 _playerPosition;

        public NoClipper(NoclipConfig config)
        {
            _config = config;
        }

        public void SceneLoaded()
        {

        }

        public void SceneUnloaded()
        {
            if (_canMovePlayer)
            {
                SetComponentStatus(true);
                _canMovePlayer = false;
            }
        }

        public void Update()
        {
            if (Main.DebugMod.InputHandler.GetKeyDown("NoClip") && Main.DebugMod.LoadStatus.GameSceneLoaded)
            {
                _canMovePlayer = !_canMovePlayer;
                SetComponentStatus(!_canMovePlayer);
            }

            UpdatePlayerPosition();
        }

        private void UpdatePlayerPosition()
        {
            if (CoreCache.PlayerSpawn.PlayerInstance == null)
                return;

            if (_canMovePlayer)
            {
                float playerSpeed = _config.movementSpeed * 120f;

                _playerPosition += Vector3.right * playerSpeed * Time.deltaTime
                    * Main.DebugMod.InputHandler.GetAxis(AxisType.MoveHorizontal);
                _playerPosition += Vector3.up * playerSpeed * Time.deltaTime
                    * Main.DebugMod.InputHandler.GetAxis(AxisType.MoveVertical);

                Body.bodyTransform = new BodyTransform { position = _playerPosition };
                CoreCache.PlayerSpawn.PlayerInstance.transform.position = _playerPosition;
            }
            else
            {
                _playerPosition = CoreCache.PlayerSpawn.PlayerInstance.transform.position;
            }
        }

        private void SetComponentStatus(bool enabled)
        {
            Motor.enabled = enabled;
            Hurtbox.SetActive(enabled);
        }

        private CharacterBody2DImpl _body;
        private CharacterBody2DImpl Body
        {
            get
            {
                if (_body == null)
                    _body = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterBody2DImpl>();
                return _body;
            }
        }

        private CharacterMotor2DComponent _motor;
        private CharacterMotor2DComponent Motor
        {
            get
            {
                if (_motor == null)
                    _motor = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterMotor2DComponent>();
                return _motor;
            }
        }

        private Transform _armor;
        private GameObject Hurtbox
        {
            get
            {
                if (_armor == null)
                {
                    _armor = CoreCache.PlayerSpawn.PlayerInstance.transform.Find("TPO/graphic/armor");
                }

                foreach (var child in _armor)
                {
                    Transform t = child.Cast<Transform>();
                    if (t.name.StartsWith("armor"))
                        return t.gameObject;
                }

                return null;
            }
        }
    }
}
