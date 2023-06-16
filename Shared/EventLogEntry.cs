namespace Shared;


/// <summary>
/// Diese Klasse dient zur Übertragung von Logs von Client zu Server
/// </summary>
[Serializable]
public class EventLogEntry
{
    public EventLogType Type { get; set; }
    public string Message { get; set; }
    public string User { get; set; }
}