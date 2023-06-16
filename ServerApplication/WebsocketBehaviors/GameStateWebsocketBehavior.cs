using ServerApplication.modules;
using Shared;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerApplication.WebsocketBehaviors;
/// <summary>
/// Socket, der die Übertragung der jeweiligen GameStates steuert.
/// </summary>
public class GameStateWebsocketBehavior : WebSocketBehavior
{
    protected override void OnOpen()
    {
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        var client = SocketMessageHelper.DeserializeFromByteArray<ClientObject>(e.RawData);
        SocketServerService.WebSocketServer.WebSocketServices["/gamestate"].Sessions.Broadcast(SocketMessageHelper.SerializeToByteArray(client));
    }
}