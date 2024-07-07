using System;
using FeralCompany.Modules.Map.Targets;
using UnityEngine;

namespace FeralCompany.Modules.Map.Pointers;

public abstract class MapPointer(GameObject pointerObject, PointerController controller, Vector3 destination, int entranceId, bool entrance) : IComparable<MapPointer>
{
    internal GameObject PointerObject { get; } = pointerObject;
    internal PointerController Controller { get; } = controller;

    protected bool IsEntrance { get; } = entrance;

    private readonly int _entranceId = entranceId;
    private float _lastDistance;

    internal void UpdateFor(MapTarget target)
    {
        _lastDistance = Vector3.Distance(target.Position, destination);
        Controller.Distance = _lastDistance;

        var direction = (destination - target.Position).normalized;
        var angle = Mathf.Atan2(direction.x, direction.z) - Mathf.Atan2(target.Forward.x, target.Forward.z);
        angle *= Mathf.Rad2Deg;
        angle = (angle + 360) % 360;

        Controller.Pointer = -angle;

        var visible = VisibleFor(target);
        PointerObject.SetActive(visible);
    }

    protected internal abstract Sprite GetIcon();
    protected abstract bool VisibleFor(MapTarget target);

    public int CompareTo(MapPointer? other)
    {
        if (other is null)
            return -1;

        if (this is ShipPointer || other is ShipPointer)
            return this is ShipPointer ? -1 : 1;

        if (this is MainEntrancePointer && other is MainEntrancePointer)
            return IsEntrance ? -1 : 1;

        if (this is MainEntrancePointer || other is MainEntrancePointer)
            return this is MainEntrancePointer ? -1 : 1;

        return _entranceId.CompareTo(other._entranceId);
    }
}
