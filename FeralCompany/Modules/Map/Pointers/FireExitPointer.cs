using FeralCompany.Modules.Map.Targets;
using UnityEngine;

namespace FeralCompany.Modules.Map.Pointers;

public sealed class FireExitPointer(GameObject pointerObject, PointerController controller, Vector3 destination, int entranceId, bool entrance) : MapPointer(pointerObject, controller, destination, entranceId, entrance)
{
    protected internal override Sprite GetIcon()
    {
        return Feral.Assets.SpriteExtinguisher;
    }

    protected override bool VisibleFor(MapTarget target)
    {
        return IsEntrance switch
        {
            true when target is { IsOutsideFacility: true, IsOutsideShip: true } => target is RadarTarget
                ? Feral.Settings.Map.PointerExternalFireRadar
                : Feral.Settings.Map.PointerExternalFire,
            false when target.IsInFacility => target is RadarTarget
                ? Feral.Settings.Map.PointerInternalFireRadar
                : Feral.Settings.Map.PointerInternalFire,
            _ => false
        };
    }
}
