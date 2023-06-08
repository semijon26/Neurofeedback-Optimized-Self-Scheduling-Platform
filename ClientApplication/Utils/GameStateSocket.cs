using System;
using System.Threading.Tasks;
using Shared;
using WebSocketSharp;

namespace ClientApplication.Utils;

public abstract class GameStateSocket
{
    private static WebSocket? _gameStateWebSocket;
    public static event EventHandler<ClientObject>? GameStateMessageReceived;

    public static void InitGameStateSocket(string ip, int port)
    {
        try
        {
            _gameStateWebSocket = new WebSocket($"ws://{ip}:{port}/gamestate");
            _gameStateWebSocket.OnMessage += OnMessage;
            _gameStateWebSocket.Connect();
        }
        catch (Exception ex)
        {
            Logging.LogError(message: "Init Client Websocket failed", ex: ex);
        }
    }

    public static async Task SendTaskIdToSetGameState(ClientObject currentClient)
    {
        await Task.Run(() =>
        {
            if (_gameStateWebSocket is { ReadyState: WebSocketState.Open })
            {
                byte[] clientObjectByteArray = SocketMessageHelper.SerializeToByteArray(currentClient);
                _gameStateWebSocket.Send(clientObjectByteArray);
            }
        });
    }

    public static async Task Close()
    {
        await Task.Run(() =>
        {
            if (_gameStateWebSocket is { ReadyState: WebSocketState.Open })
            {
                _gameStateWebSocket.Close();
            }
        });
    }

    private static void OnMessage(object? sender, MessageEventArgs e)
    {
        var clientWhoPulledTask = SocketMessageHelper.DeserializeFromByteArray<ClientObject>(e.RawData);
        GameStateMessageReceived?.Invoke(null, clientWhoPulledTask);
    }
}