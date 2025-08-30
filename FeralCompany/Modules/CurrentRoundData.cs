using System;
using System.Collections.Generic;
using FeralCompany.Modules.Map.Pointers;
using UnityEngine;

namespace FeralCompany.Modules;

public sealed class CurrentRoundData : MonoBehaviour
{
    internal IReadOnlyList<MapPointer> Pointers => _pointers;

    public static readonly List<MapPointer> _pointers = [];

    private void OnExitMoon()
    {
        foreach (var pointer in _pointers)
            Destroy(pointer.PointerObject);

        _pointers.Clear();
    }

    private void OnEnterMoon()
    {
        var obj = GameObject.Find("Environment/HangarShip/StartGameLever") ?? throw new NullReferenceException("CatwalkShip not available??");
        CreatePointer(-1, obj.transform.position, false);
    }

    private void Awake()
    {
        Feral.Events.OnEnterMoon += OnEnterMoon;
        Feral.Events.OnExitMoon += OnExitMoon;
    }

    private void OnDestroy()
    {
        Feral.Events.OnEnterMoon -= OnEnterMoon;
        Feral.Events.OnExitMoon -= OnExitMoon;
    }

    public static void CreatePointer(int entranceId, Vector3 destination, bool entrance)
    {
        var obj = Instantiate(Feral.Assets.PrefabMapPointer);
        obj.name = $"Pointer_{entranceId}_{(entrance ? "Entrance" : "Exit")}";

        var controller = obj.AddComponent<PointerController>();

        MapPointer pointer = entranceId switch
        {
            -1 => new ShipPointer(obj, controller, destination, entranceId, entrance),
            0 => new MainEntrancePointer(obj, controller, destination, entranceId, entrance),
            _ => new FireExitPointer(obj, controller, destination, entranceId, entrance)
        };
        controller.Icon = pointer.GetIcon();

        _pointers.Add(pointer);
    }
}
