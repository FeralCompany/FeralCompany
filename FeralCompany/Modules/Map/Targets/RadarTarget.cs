using UnityEngine;

namespace FeralCompany.Modules.Map.Targets;

public sealed class RadarTarget(RadarBoosterItem radar) : MapTarget
{
    private const string NoRadarName = "NoRadarName";

    internal override bool ValidateTarget()
    {
        if (!radar.radarEnabled)
            return false;

        Name = string.IsNullOrWhiteSpace(radar.radarBoosterName) ? NoRadarName : radar.radarBoosterName;
        IsInElevator = radar.isInElevator;
        IsInShip = radar.isInShipRoom;
        Position = radar.transform.position;
        Rotation = new Vector3(90f, 90f, 0f);
        IsInFacility = radar.isInFactory;
        CameraRotation = Rotation;
        return true;
    }
}
