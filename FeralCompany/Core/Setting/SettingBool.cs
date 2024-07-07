namespace FeralCompany.Core.Setting;

public class SettingBool(bool defaultValue = false) : Setting<bool>(defaultValue)
{
    internal void SetFalse() => Value = false;
    internal void SetTrue() => Value = true;
}
