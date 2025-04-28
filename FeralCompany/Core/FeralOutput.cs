using System.IO;
using System.Runtime.CompilerServices;
using BepInEx.Logging;

namespace FeralCompany.Core;

public class FeralOutput(ManualLogSource source)
{
    private const string LogTemplate = "[{0}#{1}:L{2}] {3}";

    internal void Status(object msg) => FeralCompany.HUD.DisplayStatusEffect(msg.ToString());
    internal void Debug(object msg) => FeralCompany.HUD.SetDebugText(msg.ToString());

    internal void Info(object data,
        [CallerFilePath] string caller = "",
        [CallerMemberName] string callerMethod = "",
        [CallerLineNumber] int lineNumber = 0)
        => Log(LogLevel.Info, data, caller, callerMethod, lineNumber);

    internal void Warn(object data,
        [CallerFilePath] string caller = "",
        [CallerMemberName] string callerMethod = "",
        [CallerLineNumber] int lineNumber = 0)
        => Log(LogLevel.Warning, data, caller, callerMethod, lineNumber);

    internal void Error(object data,
        [CallerFilePath] string caller = "",
        [CallerMemberName] string callerMethod = "",
        [CallerLineNumber] int lineNumber = 0)
        => Log(LogLevel.Error, data, caller, callerMethod, lineNumber);

    private void Log(LogLevel level, object data, string caller, string callerMethod, int lineNumber)
    {
        var fileName = Path.GetFileName(caller);
        source.Log(level, string.Format(LogTemplate, fileName, callerMethod, lineNumber, data));
    }
}
