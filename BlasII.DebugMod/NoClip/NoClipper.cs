using Il2CppInterop.Runtime.InteropTypes;
using Il2CppLightbug.Kinematic2D.Core;
using Il2CppLightbug.Kinematic2D.Implementation;
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
            if (Input.GetKeyDown(KeyCode.F3) && Main.DebugMod.LoadStatus.GameSceneLoaded)
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
                float playerSpeed = _config.movementSpeed;
                if (Input.GetKey(KeyCode.RightControl))
                    playerSpeed *= _config.movementModifier;

                if (Input.GetKey(KeyCode.A)) _playerPosition += Vector3.left * playerSpeed;
                if (Input.GetKey(KeyCode.D)) _playerPosition += Vector3.right * playerSpeed;
                if (Input.GetKey(KeyCode.S)) _playerPosition += Vector3.down * playerSpeed;
                if (Input.GetKey(KeyCode.W)) _playerPosition += Vector3.up * playerSpeed;

                CharacterBody2DImpl c = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterBody2DImpl>();
                var t = new BodyTransform
                {
                    position = _playerPosition
                };
                c.bodyTransform = t;
                CoreCache.PlayerSpawn.PlayerInstance.transform.position = _playerPosition;
            }
            else
            {
                _playerPosition = CoreCache.PlayerSpawn.PlayerInstance.transform.position;
            }
        }

        private void SetComponentStatus(bool enabled)
        {
            CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterMotor2DComponent>().enabled = enabled;
            //_characterMotor2.Component.enabled = enabled;
            //CoreCache.PlayerSpawn.PlayerInstance.GetComponent<BoxCollider2D>().enabled = enabled;
            //_characterCollision.Component.enabled = enabled;
            Hurtbox.SetActive(enabled);
        }

        //private readonly PlayerComponent<CharacterMotor2DComponent> _characterMotor = new();
        private readonly PlayerComponent<CharacterMotor2D> _characterMotor2 = new();
        private readonly PlayerComponent<CharacterCollisionComponent> _characterCollision = new();
        private readonly PlayerComponent<CharacterCollisions2D> _characterCollision2 = new();
        private readonly PlayerComponent<CharacterBody2DComponent> _characterBody = new();
        private readonly PlayerComponent<CharacterController2D> _characterController = new();
        private readonly PlayerComponent<CharacterBody2DImpl> _characterImplementation = new();
        private readonly PlayerComponent<BoxCollider2D> _boxCollider = new();
        private readonly PlayerComponent<PlayerControlled> _player = new();
        private readonly PlayerComponent<SafePositionComponent> _safePosition = new();

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

        private BoxCollider2D _collision;
        private BoxCollider2D Collision
        {
            get
            {
                if (_collision == null)
                    _collision = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<BoxCollider2D>();
                return _collision;
            }
        }

        private Transform _armor;
        private GameObject Hurtbox
        {
            get
            {
                //if (_armor == null)
                //{
                    _armor = CoreCache.PlayerSpawn.PlayerInstance.transform.Find("TPO/graphic/armor");
                //}
                Main.DebugMod.Log("Retrieving hurtbox");
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

    class PlayerComponent<T> where T : Il2CppObjectBase
    {
        private T _component;

        public T Component
        {
            get
            {
                if (_component == null)
                    _component = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<T>();
                return _component;
            }
        }
    }
}
