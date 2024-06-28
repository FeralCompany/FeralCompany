using System;

namespace FeralCompany.Core.Setting;

public class FuncSetting<T, R>(SettingBase<T> delegated, Func<T, R> mapper, Func<R, T> demapper) : SettingBase<R>
{
    internal override R Value
    {
        get => mapper.Invoke(delegated.Value);
        set
        {
            delegated.Value = demapper.Invoke(value);
            InvokeChangeEvent(value);
        }
    }

    internal override R DefaultValue => mapper.Invoke(delegated.Value);
}
