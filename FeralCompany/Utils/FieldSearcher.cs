using System;
using System.Collections.Generic;
using System.Reflection;

namespace FeralCompany.Utils;

public static class FieldSearcher
{
    private const BindingFlags StaticAttributeFilter = BindingFlags.Public | BindingFlags.Static;

    public static List<T> Search<T>(Type from)
    {
        List<T> fields = [];

        SearchInternal(from, StaticAttributeFilter, ref fields);
        foreach (var nested in from.GetNestedTypes(StaticAttributeFilter))
            SearchInternal(nested, StaticAttributeFilter, ref fields);

        return fields;
    }

    private static void SearchInternal<T>(IReflect from, BindingFlags filter, ref List<T> fields)
    {
        foreach (var field in from.GetFields(filter))
            if (field.GetValue(null) is T value)
                fields.Add(value);
    }
}
