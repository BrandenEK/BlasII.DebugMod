using BlasII.ModdingAPI.Input;
using Il2CppTGK.Game.Components;
using UnityEngine;

namespace BlasII.DebugMod.FreeCam
{
    public class CameraMover
    {
        private readonly CameraConfig _config;

        private bool _canMoveCamera;
        private Vector3 _cameraPosition;

        private Transform _cameraObjectRef;
        private Transform CameraObject
        {
            get
            {
                if (_cameraObjectRef == null)
                {
                    foreach (var cam in Object.FindObjectsOfType<GameCameraComponent>())
                    {
                        if (cam.name == "Main Camera")
                        {
                            Main.DebugMod.Log("Found camera object");
                            _cameraObjectRef = cam.transform;
                            break;
                        }
                    }
                }
                return _cameraObjectRef;
            }
        }

        public CameraMover(CameraConfig config)
        {
            _config = config;
        }

        public void SceneLoaded()
        {

        }

        public void SceneUnloaded()
        {
            _canMoveCamera = false;
        }

        public void Update()
        {
            if (Main.DebugMod.InputHandler.GetKeyDown("FreeCam") && Main.DebugMod.LoadStatus.GameSceneLoaded)
            {
                _canMoveCamera = !_canMoveCamera;
            }
            
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            if (CameraObject == null)
                return;

            if (!_canMoveCamera)
            {
                _cameraPosition = CameraObject.position;
                return;
            }

            float h = Main.DebugMod.InputHandler.GetAxis(AxisType.MoveRHorizontal);
            float v = Main.DebugMod.InputHandler.GetAxis(AxisType.MoveRVertical);
            var direction = new Vector3(h, v).normalized;

            _cameraPosition += direction * _config.movementSpeed * 120f * Time.deltaTime;

            CameraObject.position = _cameraPosition;
        }
    }
}
