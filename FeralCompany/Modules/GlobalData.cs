using System.Collections.Generic;
using System.Linq;
using FeralCompany.Modules.Map.Targets;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace FeralCompany.Modules;

public sealed class GlobalData : MonoBehaviour
{
    internal IReadOnlyList<MapTarget> MapTargets => _mapTargets;
    private readonly List<MapTarget> _mapTargets = [];

    private void Awake()
    {
        foreach (var player in Resources.FindObjectsOfTypeAll<PlayerControllerB>())
        {
            if (player.gameObject.TryGetComponent(out PlayerTarget target))
            {
                if (!MapTargets.Contains(target))
                    MapTargets.AddItem(target);
                continue;
            }

            target = new PlayerTarget(player);
            _mapTargets.Add(target);
        }

        Feral.Events.OnAddRadarBooster += AddRadarBooster;
        Feral.Events.OnRemoveRadarBooster += RemoveRadarBooster;
    }

    private void OnDestroy()
    {
        Feral.Events.OnAddRadarBooster -= AddRadarBooster;
        Feral.Events.OnRemoveRadarBooster -= RemoveRadarBooster;
    }

    private void AddRadarBooster(RadarBoosterItem item)
    {
        if (!item.gameObject.TryGetComponent(out RadarTarget target))
            target = new RadarTarget(item);

        if (_mapTargets.Contains(target))
            return;

        _mapTargets.Add(target);
        _mapTargets.Sort();
    }

    private void RemoveRadarBooster(RadarBoosterItem item)
    {
        if (!item.gameObject.TryGetComponent(out RadarTarget target))
            return;

        _mapTargets.Remove(target);
        _mapTargets.Sort();
    }
}
