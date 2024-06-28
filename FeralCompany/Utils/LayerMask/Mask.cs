using System;
using System.Collections.Generic;
using System.Linq;

namespace FeralCompany.Utils.LayerMask;

public static class Mask
{
    public static int Negate(int mask)
    {
        return ~mask;
    }

    public static int Combine(params Masks[] masks)
    {
        return masks.Aggregate(0, (current, mask) => current | (int)mask);
    }

    public static int Add(int mask, params Masks[] masks)
    {
        return mask | Combine(masks);
    }

    public static int Remove(int mask, params Masks[] masks)
    {
        return mask & Negate(Combine(masks));
    }

    public static bool Contains(int mask, Masks check)
    {
        return (mask & (int)check) != 0;
    }

    public static bool ContainsAll(int mask, params Masks[] masks)
    {
        return masks.All(m => Contains(mask, m));
    }

    public static bool ContainsAny(int mask, params Masks[] masks)
    {
        return masks.Any(m => Contains(mask, m));
    }

    public static Masks[] Extract(int mask)
    {
        var all = (Masks[])Enum.GetValues(typeof(Masks));
        return all.Where(check => Contains(mask, check)).ToArray();
    }

    public static int Compress(Masks[] masks)
    {
        return Combine(masks);
    }

    public static Masks[] Extract(string str)
    {
        List<Masks> masks = [];
        foreach (var raw in str.Split(','))
        {
            if (Enum.TryParse(raw, true, out Masks mask))
            {
                masks.Add(mask);
            }
            else
            {
                Feral.IO.Warn($"Ignoring unexpected, invalid mask: {raw}");
                Feral.IO.Warn($"Entirety: {str}");
            }
        }
        return masks.ToArray();
    }

    public static string ToConfigString(this Masks[] masks)
    {
        var str = "";
        var delimiter = "";
        foreach (var mask in masks)
        {
            str += delimiter;
            str += mask.ToString();
            delimiter = ",";
        }
        return str;
    }
}
