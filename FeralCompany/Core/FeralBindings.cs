using System;
using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;

namespace FeralCompany.Core;

#nullable disable
public class FeralBindings : LcInputActions
{
    [InputAction("<keyboard>/m", Name = "ToggleMap")]
    internal InputAction ToggleMap { get; set; }

    [InputAction("<keyboard>/.", Name = "Cycle Map Target")]
    internal InputAction CycleMapTarget { get; set; }

    internal InputAction Alt => _immutableMap["Alt"];
    internal InputAction Shift => _immutableMap["Shift"];
    private readonly InputActionMap _immutableMap;

    internal FeralBindings()
    {
        _immutableMap = new InputActionMap();
        _immutableMap.AddAction("Alt", binding: "<keyboard>/alt");
        _immutableMap.AddAction("Shift", binding: "<keyboard>/shift");
        _immutableMap.Enable();
    }

    public new void Enable()
    {
        base.Enable();
        _immutableMap.Enable();
    }

    public new void Disable()
    {
        base.Disable();
        _immutableMap.Disable();
    }
}
