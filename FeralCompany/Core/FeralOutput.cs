using System.IO;
using System.Runtime.CompilerServices;
using BepInEx.Logging;

namespace FeralCompany.Core;

public class FeralOutput(ManualLogSource source)
{
    private const string LogTemplate = "[{0}#{1}:L{2}] {3}";

    internal void Status(string msg)
    {
        Feral.HUD.DisplayStatusEffect(msg);
    }

    internal void Debug(string msg)
    {
        Feral.HUD.SetDebugText(msg);
    }

    internal void Info(string msg,
        [CallerFilePath] string caller = "",
        [CallerMemberName] string callerMethod = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log(LogLevel.Info, msg, caller, callerMethod, lineNumber);
    }

    internal void Warn(string msg,
        [CallerFilePath] string caller = "",
        [CallerMemberName] string callerMethod = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log(LogLevel.Warning, msg, caller, callerMethod, lineNumber);
    }

    internal void Error(string msg,
        [CallerFilePath] string caller = "",
        [CallerMemberName] string callerMethod = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log(LogLevel.Error, msg, caller, callerMethod, lineNumber);
    }

    private void Log(LogLevel level, string msg, string caller, string callerMethod, int lineNumber)
    {
        var fileName = Path.GetFileName(caller);
        source.Log(level, string.Format(LogTemplate, fileName, callerMethod, lineNumber, msg));
    }
}
