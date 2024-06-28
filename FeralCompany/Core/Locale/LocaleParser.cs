using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FeralCompany.Core.Locale;

internal static class LocaleParser
{
    internal static Locale Parse(string fileName)
    {
        string contents;
        using (var reader = new StreamReader(fileName))
        {
            contents = reader.ReadToEnd();
        }

        var rootToken = JToken.Parse(contents);

        var key = rootToken.Value<string>("key")!;
        var name = rootToken.Value<string>("name")!;
        var fallback = rootToken.Value<string>("fallback");
        var entries = new Dictionary<string, string>();

        FlattenEntries(entries, rootToken["entries"]!);

        return new Locale(key, name, fallback, entries);
    }

    private static void FlattenEntries(
        Dictionary<string, string> entries,
        JToken token,
        string prefix = ""
    )
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                foreach (var prop in token.Children<JProperty>())
                    FlattenEntries(entries, prop.Value, Join(prefix, prop.Name));
                break;
            case JTokenType.String:
                entries.Add(prefix, token.ToString());
                break;
            case JTokenType.None:
            case JTokenType.Array:
            case JTokenType.Constructor:
            case JTokenType.Property:
            case JTokenType.Comment:
            case JTokenType.Integer:
            case JTokenType.Float:
            case JTokenType.Boolean:
            case JTokenType.Null:
            case JTokenType.Undefined:
            case JTokenType.Date:
            case JTokenType.Raw:
            case JTokenType.Bytes:
            case JTokenType.Guid:
            case JTokenType.Uri:
            case JTokenType.TimeSpan:
            default:
                throw new ArgumentOutOfRangeException($"Unsupported JTokenType: {token.Type}");
        }
    }

    private static string Join(string prefix, string key)
    {
        return string.IsNullOrEmpty(prefix) ? key : prefix + "." + key;
    }
}
