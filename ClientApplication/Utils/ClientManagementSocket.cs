using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApplication.Models;
using Shared;
using WebSocketSharp;

namespace ClientApplication.Utils;

public abstract class ClientManagementSocket
{
    private static WebSocket? _clientWebSocket;
    public static event EventHandler? MessageReceived;
    public static event EventHandler? OnStartGamesMessageReceived;

    public static void InitClientSocket(string ip, int port)
    {
        try
        {
            _clientWebSocket = new WebSocket($"ws://{ip}:{port}/clients");
            _clientWebSocket.OnOpen += OnOpen;
            _clientWebSocket.OnMessage += OnMessage;
            _clientWebSocket.Connect();
        }
        catch (Exception ex)
        {
            Logging.LogError(message: "Init Client Websocket failed", ex: ex);
        }
    }

    public static async Task Send(string message)
    {
        await Task.Run(() =>
        {
            if (_clientWebSocket is { ReadyState: WebSocketState.Open })
            {
                _clientWebSocket.Send(message);
            }
        });
    }

    public static async Task Close()
    {
        await Task.Run(() =>
        {
            if (_clientWebSocket is { ReadyState: WebSocketState.Open })
            {
                _clientWebSocket.Close();
            }
        });
    }

    private static void OnOpen(object? sender, EventArgs e)
    {
        SendClientObjectWhenConnectionEstablished();
    }

    private static void OnMessage(object? sender, MessageEventArgs e)
    {
        var clientList = SocketMessageHelper.DeserializeFromByteArray<List<ClientObject>>(e.RawData);
        var currentClient = ClientObject.GetInstance(); 
        var clientManagementData = ClientManagementData.GetInstance(currentClient); 
        clientManagementData.OtherClients.Clear();
        foreach (var clientObject in clientList)
        {
            if (clientObject.UniqueId == currentClient.UniqueId)
            {
                    currentClient.ActiveGames = clientObject.ActiveGames;
                    currentClient.MinClientsToStartGames = clientObject.MinClientsToStartGames;
            }
            else clientManagementData.OtherClients.Add(clientObject);
        }
        if (clientList.Count >= currentClient.MinClientsToStartGames)
        {
            OnStartGamesMessageReceived?.Invoke(null, EventArgs.Empty);
        }
        MessageReceived?.Invoke(null, EventArgs.Empty);
    }

    public Uri? GetWsUri()
    {
        return _clientWebSocket?.Url;
    }

    public static async void SendClientObjectWhenConnectionEstablished()
    {
        await Task.Run(() =>
        {
            if (_clientWebSocket is { ReadyState: WebSocketState.Open })
            {
                ClientObject clientObject = ClientObject.GetInstance();
                if (!clientObject.Label.IsNullOrEmpty())
                {
                    byte[] clientObjectByteArray = SocketMessageHelper.SerializeToByteArray(clientObject);
                    _clientWebSocket?.Send(clientObjectByteArray);
                }
            }
        });
    }

    private static void OnMessageReceived()
    {
        MessageReceived?.Invoke(null, EventArgs.Empty);
    }
}