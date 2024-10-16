using BlasII.ModdingAPI;
using System.Linq;

namespace BlasII.DebugMod.EventLogger;

internal class LoggerModule
{
    public void LogEvent(string obj, string method, EventType type, params EventParameter[] parameters)
    {
        string header = $"{obj}.{method}: ";
        string body = string.Join(' ', parameters.Select(x => $"[{x.Name}={x.Value}]"));
        ModLog.Debug(header + body);
    }
}
