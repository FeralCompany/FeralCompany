using System;
using GameNetcodeStuff;

namespace FeralCompany.Core;

public class FeralEvents
{
    internal event Action<PlayerControllerB>? OnPlayerAssigned;

    internal void InvokePlayerAssigned(PlayerControllerB assignedPlayer)
    {
        OnPlayerAssigned?.Invoke(assignedPlayer);
    }
}
