using System.Collections.Generic;

namespace FeralCompany.Core.Locale;

public class Locale(string key, string name, string? fallback, Dictionary<string, string> entries)
{
    internal static Locale Current { get; set; } = null!;

    internal string Key { get; } = key;
    internal string Name { get; } = name;
    internal string? Fallback { get; } = fallback;

    internal Dictionary<string, string> Entries { get; } = entries;

    internal string GetEntry(string key)
    {
        if (!TryGetEntry(key, out var entry))
            Feral.IO.Warn($"Locale '{key}' does not contain entry: {key}");

        return entry;
    }

    internal string GetEntry(string key, params object[] data)
    {
        if (!TryGetEntry(key, out var entry))
            Feral.IO.Warn($"Locale '{key}' does not contain entry: {key}");

        return string.Format(entry, data);
    }

    internal bool TryGetEntry(string key, out string entry)
    {
        if (Entries.TryGetValue(key, out entry))
            return true;

        if (Fallback != null && Feral.Locales.Locales.TryGetValue(Fallback, out var fallback))
            return fallback.TryGetEntry(key, out entry);

        entry = key;
        return false;
    }

    internal bool TryGetEntry(string key, out string entry, params object[] data)
    {
        if (!TryGetEntry(key, out entry))
            return false;

        entry = string.Format(entry, data);
        return true;
    }
}
