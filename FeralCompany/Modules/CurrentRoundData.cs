using System;
using System.Collections.Generic;
using FeralCompany.Modules.Map.Pointers;
using UnityEngine;

namespace FeralCompany.Modules;

public sealed class CurrentRoundData : MonoBehaviour
{
    internal IReadOnlyList<MapPointer> Pointers => _pointers;

    private readonly List<MapPointer> _pointers = [];

    private void OnEnterMoon()
    {
        var obj = GameObject.Find("CatwalkShip") ?? throw new NullReferenceException("CatwalkShip not available??");
        CreatePointer(-1, obj.transform.position, false);

        foreach (var teleport in FindObjectsOfType<EntranceTeleport>())
            CreatePointer(teleport.entranceId, teleport.entrancePoint.position, teleport.isEntranceToBuilding);

        FeralCompany.Events.InvokePointersCreated(_pointers.ToArray());
    }

    private void OnExitMoon()
    {
        foreach (var pointer in _pointers)
            Destroy(pointer.PointerObject);

        var pointersCopy = _pointers.ToArray();
        _pointers.Clear();
        FeralCompany.Events.InvokePointersDestroyed(pointersCopy);
    }

    private void Awake()
    {
        FeralCompany.Events.OnEnterMoon += OnEnterMoon;
        FeralCompany.Events.OnExitMoon += OnExitMoon;
    }

    private void OnDestroy()
    {
        FeralCompany.Events.OnEnterMoon -= OnEnterMoon;
        FeralCompany.Events.OnExitMoon -= OnExitMoon;
    }

    private void CreatePointer(int entranceId, Vector3 destination, bool entrance)
    {
        var obj = Instantiate(FeralCompany.Assets.PrefabMapPointer);
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
