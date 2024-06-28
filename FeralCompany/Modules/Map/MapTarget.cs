using System;
using System.Collections.Generic;
using FeralCompany.Modules.Map.UI;
using GameNetcodeStuff;
using UnityEngine;

namespace FeralCompany.Modules.Map;

public class MapTarget : MonoBehaviour
{
    private static List<TransformAndName> Targets => Feral.Round.mapScreen.radarTargets;

    private static FeralMap Map => Feral.Map;
    private static MapUI MapUI => Map.MapUI;

    private int _index;
    private PlayerControllerB? _playerTarget;
    private RadarBoosterItem? _radarTarget;

    internal void Next()
    {
        if (_index < 0 || _index + 1 >= Targets.Count)
        {
            _index = 0;
            return;
        }

        _index++;
    }

    private void Update()
    {
        ValidateIndex();
        UpdateCamera();
        UpdateCameraUI();
    }

    private void ValidateIndex()
    {
        _playerTarget = null;
        _radarTarget = null;

        if (Targets.Count == 0)
            return;

        if (_index < 0)
            _index = 0;

        if (_index >= Targets.Count)
            _index = Targets.Count - 1;

        var target = Targets[_index];
        var startIndex = _index;
        while (!ValidateTarget(target))
        {
            _index++;
            if (_index >= Targets.Count)
                _index = 0;

            if (_index == startIndex)
                return;

            target = Targets[_index];
        }
    }

    private bool ValidateTarget(TransformAndName target)
    {
        _playerTarget = null;
        _radarTarget = null;
        if (target.isNonPlayer)
        {
            _radarTarget = target.transform.gameObject.GetComponent<RadarBoosterItem>();
            return true;
        }

        _playerTarget = target.transform.gameObject.GetComponent<PlayerControllerB>();
        return _playerTarget && (_playerTarget.isPlayerDead || _playerTarget.isPlayerControlled);
    }

    private void UpdateCamera()
    {
        Map.Camera.transform.eulerAngles = new Vector3(90f, GetEulerY(), 0f);

        if (_playerTarget is not null && _playerTarget)
        {
            var position = _playerTarget.isPlayerDead
                ? _playerTarget.deadBody.transform.position
                : _playerTarget.transform.position;

            Map.Camera.transform.position = position + Vector3.up * 3.636f;
            Map.Camera.nearClipPlane = _playerTarget.isInHangarShipRoom ? -0.96f : -2.47f;

            Map.Light.enabled = _playerTarget.isInsideFactory;
            return;
        }

        if (_radarTarget is not null && _radarTarget)
        {
            var position = _radarTarget.transform.position;
            Map.Camera.transform.position = position + Vector3.up * 3.636f;
            Map.Camera.nearClipPlane = _radarTarget.isInShipRoom ? -0.96f : -2.47f;

            Map.Light.enabled = _radarTarget.isInFactory;
            return;
        }

        Map.Camera.nearClipPlane = -2.47f;
        Map.Light.enabled = false;
    }

    private void UpdateCameraUI()
    {
        if (!_playerTarget && !_radarTarget)
        {
            MapUI.Name = "Unknown";
            return;
        }

        if (_playerTarget)
        {
            MapUI.name = _playerTarget!.playerUsername;
            return;
        }

        MapUI.name = _radarTarget!.radarBoosterName;
    }

    private float GetEulerY()
    {
        return Feral.Settings.Map.Rotation.Value switch
        {
            Rotations.Default => GetDefaultEulerY(),
            Rotations.Target => GetTargetEulerY(),
            Rotations.North => 90f,
            Rotations.East => 180f,
            Rotations.South => 270f,
            Rotations.West => 0f,
            _ => throw new ArgumentOutOfRangeException(nameof(Feral.Settings.Map.Rotation.Value), Feral.Settings.Map.Rotation.Value, null)
        };
    }

    private float GetTargetEulerY()
    {
        if (_playerTarget is null || !_playerTarget || _playerTarget.isPlayerDead || !_playerTarget.isPlayerControlled)
            return GetDefaultEulerY();
        return _playerTarget.transform.eulerAngles.y;
    }

    // Default: 315
    // However, there are various mods that change this to another value (such as North),
    // so we retrieve it this way instead of just assuming that it hasn't changed.
    private static float _defaultEulerY;
    private static bool _checkedDefaultEulerY;
    private static float GetDefaultEulerY()
    {
        if (_checkedDefaultEulerY)
            return _defaultEulerY;
        _checkedDefaultEulerY = true;

        _defaultEulerY = GameObject.Find("MapCamera").GetComponent<Camera>().transform.eulerAngles.y;
        return _defaultEulerY;
    }
}
