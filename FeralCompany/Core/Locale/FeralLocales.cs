using System.Collections.Generic;
using System.IO;
using FeralCompany.Utils;

namespace FeralCompany.Core.Locale;

internal class FeralLocales
{
    internal readonly Dictionary<string, Locale> Locales = new();

    internal bool Init()
    {
        foreach (var fileName in Directory.GetFiles(Files.LocalePath, "*.json"))
        {
            var parsed = LocaleParser.Parse(fileName);
            Locales.Add(parsed.Key, parsed);
        }

        Locale.Current = Feral.Settings.General.Locale;
        return true;
    }
}
