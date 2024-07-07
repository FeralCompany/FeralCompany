using FeralCompany.Core.Setting;

namespace FeralCompany.Core;

public class SettingInt(int defaultValue = 0) : Setting<int>(defaultValue)
{
    internal int Increment(int add = 1)
    {
        Value += add;
        return Value;
    }

    internal int Decrement(int subtract = 1)
    {
        Value -= subtract;
        return Value;
    }
}
