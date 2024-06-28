using BepInEx.Configuration;

namespace FeralCompany.Core.Setting;

public class Config<T> : SettingBase<T>
{
    internal override T Value
    {
        get => _entry.Value;
        set => _entry.Value = value;
    }

    internal override T DefaultValue { get; }

    private readonly ConfigEntry<T> _entry;

    internal Config(ConfigFile config, string section, string key, T defaultValue)
    {
        DefaultValue = defaultValue;
        _entry = config.Bind(section, key, defaultValue);
        _entry.SettingChanged += (_, _) => InvokeChangeEvent(_entry.Value);
    }
}
