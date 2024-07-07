using GameNetcodeStuff;
using UnityEngine;

namespace FeralCompany.Modules.Map.Targets;

public sealed class PlayerTarget(PlayerControllerB player) : MapTarget
{
    internal override bool ValidateTarget()
    {
        if (!player || player is { isPlayerControlled: false, isPlayerDead: false })
            return false;

        Name = player.playerUsername;
        IsDead = player.isPlayerDead;
        IsInElevator = player.isInElevator;
        IsInShip = player.isInHangarShipRoom;
        IsInFacility = player.isInsideFactory;

        var localTransform = IsDead ? player.deadBody.transform : player.transform;
        Position = localTransform.position;
        Forward = localTransform.forward;
        Quaternion = localTransform.rotation;

        CameraRotation = IsDead ? new Vector3(90f, 90f, 0f) : new Vector3(90f, Rotation.y, 0f);
        return true;
    }
}
