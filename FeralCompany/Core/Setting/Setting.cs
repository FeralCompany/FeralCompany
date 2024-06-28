namespace FeralCompany.Core.Setting;

public class Setting<T>(T defaultValue) : SettingBase<T>
{
    internal override T Value
    {
        get => _value;
        set
        {
            _value = value;
            InvokeChangeEvent(value);
        }
    }

    internal override T DefaultValue { get; } = defaultValue;

    private T _value = defaultValue;
}
