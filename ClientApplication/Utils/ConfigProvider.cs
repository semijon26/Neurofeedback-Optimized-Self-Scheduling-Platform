using System;
using Shared;
using WebSocketSharp;

namespace ClientApplication.Utils;

public static class ConfigProvider
{
    private static WebSocket _webSocket;
    
    public static void Init(string ip, int port)
    {
        try
        {
            _webSocket = new WebSocket($"ws://{ip}:{port}/config");
            _webSocket.OnMessage += (sender, args) =>
            {
                try
                {
                    var config = SocketMessageHelper.DeserializeFromByteArray<ConfigObject>(args.RawData);
                    WorkloadController.GetInstance().Init(config.WorkloadViewChangeMode);
                }
                catch (Exception e)
                {
                    Logging.LogError("Could not read configObject", e);
                }
            };
            _webSocket.Connect();
        }
        catch (Exception ex)
        {
            Logging.LogError(message: "Init Config Websocket failed", ex: ex);
        }
    }
}