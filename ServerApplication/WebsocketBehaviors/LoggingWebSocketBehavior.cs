using ServerApplication.modules;
using Shared;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerApplication.WebsocketBehaviors;
/// <summary>
/// WebSocket, der für das Schreiben der übertragenen Logs verantwortlich ist.
/// </summary>
public class LoggingWebSocketBehavior : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        // Jede Client-Message in diesem Websocket wird an den Logger weitergeleitet
        var logEntry = SocketMessageHelper.DeserializeFromByteArray<EventLogEntry>(e.RawData);
        Logging.LogEvent(logEntry);
    }
}