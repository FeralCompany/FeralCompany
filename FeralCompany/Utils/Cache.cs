using System;

namespace FeralCompany.Utils;

public class Cache<T>(Func<T> supplier)
{
    internal T Value => GetOrLoad();

    internal bool IsLoaded { get; private set; }

    private T? _value;

    internal bool GetIfLoaded(out T? value)
    {
        if (IsLoaded)
        {
            value = _value;
            return true;
        }

        value = default;
        return false;
    }

    private T GetOrLoad()
    {
        if (IsLoaded)
            return _value!;

        _value = supplier.Invoke();
        IsLoaded = true;
        return _value;
    }

    public static implicit operator T(Cache<T> cache)
    {
        return cache.Value;
    }
}
