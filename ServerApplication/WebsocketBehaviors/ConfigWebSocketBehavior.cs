using ServerApplication.modules;
using Shared;
using WebSocketSharp.Server;

namespace ServerApplication.WebsocketBehaviors;

public class ConfigWebSocketBehavior : WebSocketBehavior
{
    protected override void OnOpen()
    {
        Send(SocketMessageHelper.SerializeToByteArray(SocketServerService.objectConfiguration));
    }
}