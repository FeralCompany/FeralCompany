using System;
using FeralCompany.Modules.Map.Pointers;

namespace FeralCompany.Core;

public class FeralEvents
{
    internal event Action? OnClientConnect;
    internal event Action? OnClientDisconnect;

    internal event Action? OnEnterMoon;
    internal event Action? OnExitMoon;

    internal event Action? OnModStart;
    internal event Action? OnModShutdown;

    internal event Action<RadarBoosterItem>? OnAddRadarBooster;
    internal event Action<RadarBoosterItem>? OnRemoveRadarBooster;

    internal event Action<MapPointer[]> OnPointersCreated = null!;
    internal event Action<MapPointer[]> OnPointersDestroyed = null!;

    internal int PlayerCount { get; private set; }
    internal bool IsInGame { get; private set; }
    internal bool IsOnMoon { get; private set; }

    internal void InvokeClientConnect(ulong _)
    {
        PlayerCount += 1;
        OnClientConnect?.Invoke();
    }

    internal void InvokeClientDisconnect(ulong _)
    {
        PlayerCount -= 1;
        OnClientDisconnect?.Invoke();
    }

    internal void InvokeEnterMoon()
    {
        IsOnMoon = true;
        OnEnterMoon?.Invoke();
    }

    internal void InvokeExitMoon()
    {
        IsOnMoon = false;
        OnExitMoon?.Invoke();
    }

    internal void InvokeStartMod()
    {
        IsInGame = true;
        OnModStart?.Invoke();
    }

    internal void InvokeShutdownMod()
    {
        IsInGame = false;
        OnModShutdown?.Invoke();
    }

    internal void InvokeAddRadarBooster(RadarBoosterItem radar) => OnAddRadarBooster?.Invoke(radar);
    internal void InvokeRemoveRadarBooster(RadarBoosterItem radar) => OnRemoveRadarBooster?.Invoke(radar);

    internal void InvokePointersCreated(MapPointer[] pointers) => OnPointersCreated?.Invoke(pointers);
    internal void InvokePointersDestroyed(MapPointer[] pointers) => OnPointersDestroyed?.Invoke(pointers);
}
