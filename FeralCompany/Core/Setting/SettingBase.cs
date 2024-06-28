using System;

namespace FeralCompany.Core.Setting;

public abstract class SettingBase<T>
{
    internal abstract T Value { get; set; }
    internal abstract T DefaultValue { get; }

    internal event Action<T>? ChangeEvent;

    internal void Reset() => Value = DefaultValue;

    protected void InvokeChangeEvent(T newValue) => ChangeEvent?.Invoke(newValue);

    public static implicit operator T(SettingBase<T> settingBase) => settingBase.Value;
}
