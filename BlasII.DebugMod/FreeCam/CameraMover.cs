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

            if (_canMoveCamera)
            {
                float camSpeed = _config.movementSpeed * 120;

                _cameraPosition += Vector3.right * camSpeed * Time.deltaTime
                    * Main.DebugMod.InputHandler.GetAxis(AxisType.MoveRHorizontal);
                _cameraPosition += Vector3.up * camSpeed * Time.deltaTime
                    * Main.DebugMod.InputHandler.GetAxis(AxisType.MoveRVertical);

                CameraObject.position = _cameraPosition;
            }
            else
            {
                _cameraPosition = CameraObject.position;
            }
        }
    }
}
