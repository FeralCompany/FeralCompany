using System;
using System.Reflection;

namespace FeralCompany.Utils;

internal static class Reflection
{
    internal static void Invoke<T>(T instance, string methodName, params object[] parameters)
    {
        typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(instance, parameters);
    }

    internal static R Invoke<T, R>(T instance, string methodName, params object[] parameters)
    {
        var value = typeof(T)
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)!
            .Invoke(instance, parameters);
        if (value is not R valueCasted)
            throw new ArgumentOutOfRangeException();

        return valueCasted;
    }
}
