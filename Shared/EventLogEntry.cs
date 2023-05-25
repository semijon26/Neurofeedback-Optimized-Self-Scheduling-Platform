namespace Shared;

[Serializable]
public class EventLogEntry
{
    public EventLogType Type { get; set; }
    public string Message { get; set; }
    public string User { get; set; }
}