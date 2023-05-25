using ServerApplication.modules;
using Shared;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerApplication.WebsocketBehaviors;

public class LoggingWebSocketBehavior : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        var logEntry = SocketMessageHelper.DeserializeFromByteArray<EventLogEntry>(e.RawData);
        Logging.LogEvent(logEntry);
    }
}