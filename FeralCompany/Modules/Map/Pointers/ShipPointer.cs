using FeralCompany.Modules.Map.Targets;
using UnityEngine;

namespace FeralCompany.Modules.Map.Pointers;

public sealed class ShipPointer(GameObject pointerObject, PointerController controller, Vector3 destination, int entranceId, bool entrance) : MapPointer(pointerObject, controller, destination, entranceId, entrance)
{
    protected internal override Sprite GetIcon()
    {
        return FeralCompany.Assets.SpriteHome;
    }

    protected override bool VisibleFor(MapTarget target)
    {
        if (target is { IsOutsideFacility: true, IsOutside: true })
        {
            return target is RadarTarget
                ? FeralCompany.Settings.Map.PointerShipRadar
                : FeralCompany.Settings.Map.PointerShip;
        }
        return false;
    }
}
