using System.IO;
using System.Reflection;

namespace FeralCompany.Utils;

internal static class Files
{
    private static readonly string RootPath = Path.GetDirectoryName(
        Assembly.GetExecutingAssembly().Location
    )!;
    public static readonly string LocalePath = Path.Combine(RootPath, "Locales");
}
