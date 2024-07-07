using System;
using GameNetcodeStuff;
using UnityEngine;

namespace FeralCompany.Modules.Map.Targets;

public sealed class PlayerTarget(PlayerControllerB player) : MapTarget, IComparable<PlayerTarget>
{
    private readonly PlayerControllerB _player = player;

    internal override bool ValidateTarget()
    {
        if (!_player || _player is { isPlayerControlled: false, isPlayerDead: false })
        {
            return false;
        }

        Name = _player.playerUsername;
        IsDead = _player.isPlayerDead;
        IsInElevator = _player.isInElevator;
        IsInShip = _player.isInHangarShipRoom;
        IsInFacility = _player.isInsideFactory;

        var localTransform = IsDead ? _player.deadBody.transform : _player.transform;
        Position = localTransform.position;
        Forward = localTransform.forward;
        Quaternion = localTransform.rotation;

        CameraRotation = IsDead ? new Vector3(90f, 90f, 0f) : new Vector3(90f, Rotation.y, 0f);
        return _player.isPlayerDead || _player.isPlayerControlled;
    }

    public int CompareTo(PlayerTarget? other)
    {
        if (other is null)
            return -1;

        return _player.playerClientId.CompareTo(other._player.playerClientId);
    }
}
