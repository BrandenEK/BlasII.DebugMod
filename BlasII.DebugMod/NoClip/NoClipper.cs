using Il2CppTGK.Game;
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
            _canMovePlayer = false;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3) && Main.DebugMod.LoadStatus.GameSceneLoaded)
            {
                _canMovePlayer = !_canMovePlayer;
            }

            UpdatePlayerPosition();
        }

        private void UpdatePlayerPosition()
        {
            if (CoreCache.PlayerSpawn.PlayerInstance == null)
                return;

            if (_canMovePlayer)
            {
                float playerSpeed = _config.movementSpeed;
                if (Input.GetKey(KeyCode.RightControl))
                    playerSpeed *= _config.movementModifier;

                if (Input.GetKey(KeyCode.A)) _playerPosition += Vector3.left * playerSpeed;
                if (Input.GetKey(KeyCode.D)) _playerPosition += Vector3.right * playerSpeed;
                if (Input.GetKey(KeyCode.S)) _playerPosition += Vector3.down * playerSpeed;
                if (Input.GetKey(KeyCode.W)) _playerPosition += Vector3.up * playerSpeed;

                CoreCache.PlayerSpawn.PlayerInstance.transform.position = _playerPosition;
            }
            else
            {
                _playerPosition = CoreCache.PlayerSpawn.PlayerInstance.transform.position;
            }
        }
    }
}
