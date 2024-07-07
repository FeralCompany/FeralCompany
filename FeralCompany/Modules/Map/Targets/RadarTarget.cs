using System;
using UnityEngine;

namespace FeralCompany.Modules.Map.Targets;

public sealed class RadarTarget(RadarBoosterItem radar) : MapTarget, IComparable<RadarTarget>
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
        return true;
    }

    public int CompareTo(RadarTarget? other)
    {
        if (other is null)
            return -1;

        var thisHasName = Name != NoRadarName;
        var otherHasName = other.Name != NoRadarName;

        if (thisHasName && otherHasName)
            return string.Compare(Name, other.Name, StringComparison.Ordinal);

        return thisHasName ? -1 : 1;
    }
}
