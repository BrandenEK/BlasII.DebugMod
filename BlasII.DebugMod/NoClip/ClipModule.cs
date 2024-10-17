using BlasII.ModdingAPI.Input;
using Il2CppLightbug.Kinematic2D.Core;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components;
using UnityEngine;

namespace BlasII.DebugMod.NoClip;

internal class ClipModule(NoClipSettings settings)
{
    private readonly NoClipSettings _settings = settings;

    private bool _canMovePlayer;
    private Vector3 _playerPosition;

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
        if (Main.DebugMod.InputHandler.GetKeyDown("NoClip"))
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

        if (!_canMovePlayer)
        {
            _playerPosition = CoreCache.PlayerSpawn.PlayerInstance.transform.position;
            return;
        }

        float speed = _settings.Speed * 120f;
        float h = Main.DebugMod.InputHandler.GetAxis(AxisType.MoveHorizontal);
        float v = Main.DebugMod.InputHandler.GetAxis(AxisType.MoveVertical);
        var direction = new Vector3(h, v).normalized;

        _playerPosition += direction * speed * Time.deltaTime;

        Body.bodyTransform = new BodyTransform { position = _playerPosition };
        CoreCache.PlayerSpawn.PlayerInstance.transform.position = _playerPosition;
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
