namespace FeralCompany.Core.Locale;

public class LocaleToken(string key)
{
    internal string GetEntry()
    {
        return Locale.Current.GetEntry(key);
    }

    internal string GetEntry(params object[] data)
    {
        return Locale.Current.GetEntry(key, data);
    }

    internal bool TryGetEntry(out string entry)
    {
        return Locale.Current.TryGetEntry(key, out entry);
    }

    internal bool TryGetEntry(out string entry, params object[] data)
    {
        return Locale.Current.TryGetEntry(key, out entry, data);
    }

    public static implicit operator string(LocaleToken localeToken)
    {
        return localeToken.GetEntry();
    }
}
