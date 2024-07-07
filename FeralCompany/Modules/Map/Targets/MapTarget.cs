using System;
using UnityEngine;

namespace FeralCompany.Modules.Map.Targets;

public abstract class MapTarget : IComparable<MapTarget>
{
    private const float DefaultNearClipPlane = -2.47f;
    private const float HangarNearClipPlane = -0.96f;
    private const float CameraHoverY = 3.636f;

    protected internal string Name { get; protected set; } = "Unnamed";
    protected internal bool IsDead { get; protected set; }
    protected internal bool IsInElevator { get; protected set; }
    protected internal bool IsInShip { get; protected set; }
    protected internal bool IsInFacility { get; protected set; }

    protected internal bool IsOutsideShip => !IsInShip;
    protected internal bool IsOutside => !IsInShip && !IsInElevator;
    protected internal bool IsOutsideFacility => !IsInFacility;

    protected internal Vector3 Position { get; protected set; }
    protected internal Vector3 Forward { get; protected set; }
    protected internal Quaternion Quaternion { get; protected set; }

    protected internal Vector3 Rotation
    {
        get => Quaternion.eulerAngles;
        set => Quaternion = Quaternion.Euler(value);
    }

    internal float NearClipPlane => IsInShip ? HangarNearClipPlane : DefaultNearClipPlane;
    internal Vector3 CameraPosition => Position + Vector3.up * CameraHoverY;
    protected internal Vector3 CameraRotation { get; protected set; }

    internal abstract bool ValidateTarget();

    public int CompareTo(MapTarget? other)
    {
        if (other is null)
            return -1;

        if (this is PlayerTarget thisPlayer && other is PlayerTarget otherPlayer)
            return thisPlayer.CompareTo(otherPlayer);

        if (this is RadarTarget thisRadar && other is RadarTarget radarTarget)
            return thisRadar.CompareTo(radarTarget);

        return this is PlayerTarget ? -1 : 1;
    }
}
