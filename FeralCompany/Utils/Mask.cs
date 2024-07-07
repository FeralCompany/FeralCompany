using System;
using System.Collections.Generic;
using System.Linq;

namespace FeralCompany.Utils;

public static class Mask
{
    internal const int Default = 1 << 0;
    internal const int TransparentFX = 1 << 1;
    internal const int IgnoreRaycast = 1 << 2;
    internal const int Player = 1 << 3;
    internal const int Water = 1 << 4;
    internal const int UI = 1 << 5;
    internal const int Props = 1 << 6;
    internal const int HelmetVisor = 1 << 7;
    internal const int Room = 1 << 8;
    internal const int InteractableObject = 1 << 9;
    internal const int Foliage = 1 << 10;
    internal const int Colliders = 1 << 11;
    internal const int PhysicsObject = 1 << 12;
    internal const int Triggers = 1 << 13;
    internal const int MapRadar = 1 << 14;
    internal const int NavigationSurface = 1 << 15;
    internal const int RoomLight = 1 << 16;
    internal const int Anomaly = 1 << 17;
    internal const int LineOfSight = 1 << 18;
    internal const int Enemies = 1 << 19;
    internal const int PlayerRagdoll = 1 << 20;
    internal const int MapHazards = 1 << 21;
    internal const int ScanNode = 1 << 22;
    internal const int EnemiesNotRendered = 1 << 23;
    internal const int MiscLevelGeometry = 1 << 24;
    internal const int Terrain = 1 << 25;
    internal const int PlaceableShipObjects = 1 << 26;
    internal const int PlacementBlocker = 1 << 27;
    internal const int Railing = 1 << 28;
    internal const int DecalStickableSurface = 1 << 29;
    internal const int Unused1 = 1 << 30;
    internal const int Unused2 = 1 << 31;

    internal static string[] Names => NameToMask.Keys.ToArray();
    internal static readonly Dictionary<string, int> NameToMask = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Default", Default },
        { "TransparentFX", TransparentFX },
        { "IgnoreRaycast", IgnoreRaycast },
        { "Player", Player },
        { "Water", Water },
        { "UI", UI },
        { "Props", Props },
        { "HelmetVisor", HelmetVisor },
        { "Room", Room },
        { "InteractableObject", InteractableObject },
        { "Foliage", Foliage },
        { "Colliders", Colliders },
        { "PhysicsObject", PhysicsObject },
        { "Triggers", Triggers },
        { "MapRadar", MapRadar },
        { "NavigationSurface", NavigationSurface },
        { "RoomLight", RoomLight },
        { "Anomaly", Anomaly },
        { "LineOfSight", LineOfSight },
        { "Enemies", Enemies },
        { "PlayerRagdoll", PlayerRagdoll },
        { "MapHazards", MapHazards },
        { "ScanNode", ScanNode },
        { "EnemiesNotRendered", EnemiesNotRendered },
        { "MiscLevelGeometry", MiscLevelGeometry },
        { "Terrain", Terrain },
        { "PlaceableShipObjects", PlaceableShipObjects },
        { "PlacementBlocker", PlacementBlocker },
        { "Railing", Railing },
        { "DecalStickableSurface", DecalStickableSurface },
        { "Unused1", Unused1 },
        { "Unused2", Unused2 }
    };

    internal static int AddMask(this int mask, int other) => mask | other;
    internal static int AddMask(this int mask, params int[] others) => others.Aggregate(mask, AddMask);
    internal static int SubtractMask(this int mask, int other) => mask & ~other;
    internal static int SubtractMask(int mask, params int[] others) => others.Aggregate(mask, SubtractMask);
    internal static bool Contains(int mask, int other) => (mask & other) == other;
    internal static bool ContainsAny(int mask, params int[] others) => others.Any(other => Contains(mask, other));
    internal static bool ContainsAll(int mask, params int[] others) => others.All(other => Contains(mask, other));

    internal static int Named(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Mask name cannot be blank.", nameof(input));
        if (NameToMask.TryGetValue(input, out var value))
            return value;
        throw new ArgumentException($"Unknown or invalid mask name: {input}", nameof(input));
    }

    internal static string[] Decompress(int mask)
    {
        List<string> names = [];
        names.AddRange(from pair in NameToMask where Mask.Contains(mask, pair.Value) select pair.Key);
        return names.ToArray();
    }
}
