using BlasII.ModdingAPI;
using System.Linq;

namespace BlasII.DebugMod.EventLogger;

internal class LoggerModule(EventLoggerSettings settings)
{
    private readonly EventLoggerSettings _settings = settings;

    public void LogEvent(string obj, string method, EventType type, params EventParameter[] parameters)
    {
        if (!ShouldLogEvent(type))
            return;

        string header = $"{obj}.{method}: ";
        string body = string.Join(' ', parameters.Select(x => $"[{x.Name}={x.Value}]"));
        ModLog.Debug(header + body);
    }

    private bool ShouldLogEvent(EventType type)
    {
        return type switch
        {
            EventType.Dialog => _settings.LogDialogEvents,
            EventType.Quest => _settings.LogQuestEvents,
            _ => throw new System.ArgumentException("Invalid event type", nameof(type))
        };
    }
}
