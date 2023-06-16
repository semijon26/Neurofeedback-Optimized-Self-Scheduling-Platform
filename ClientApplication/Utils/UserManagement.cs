using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace ClientApplication.Utils;
/// <summary>
///  Klasse für das Management der verbundenen Clients.
/// </summary>
public class UserManagement
{
    private static WebSocket? _webSocket;

    // Action die ausgeführt wird, wenn eine Nachricht eintrifft
    public static Action<string> OnUserStringReceived;

    public static void InitUserWebSocket(string ip, int port)
    {
        try
        {
            _webSocket = new WebSocket($"ws://{ip}:{port}/users");
            _webSocket.OnOpen += OnOpen;
            _webSocket.OnMessage += OnMessage;
            _webSocket.OnClose += OnClose;

            _webSocket.Connect();
        }
        catch (Exception ex)
        {
            Logging.LogError(ex);
        }
    }

    public static async Task ReceiveUsers()
    {
        if (_webSocket != null && _webSocket.ReadyState == WebSocketState.Open)
        {
            _webSocket.Send("Initial User Request.");
        }
    }
    
    public static async Task Close()
    {
        if (_webSocket != null && _webSocket.ReadyState == WebSocketState.Open)
        {
            _webSocket.Close();
        }
    }

    private static void OnOpen(object sender, EventArgs e)
    {
        Console.WriteLine("Connected to user socket.");
        Logging.LogInformation("Connected to Users Socket");
    }

    private static void OnMessage(object sender, MessageEventArgs e)
    {
        // OnMessagereceived - Delegate aufrufn, um den Text im Mainwindow anzuzeigen
        OnUserStringReceived?.Invoke(e.Data);
        Console.WriteLine(e.Data);
        Logging.LogInformation($"Following user list received: {e.Data}");
    }

    private static void OnClose(object sender, CloseEventArgs e)
    {
        Console.WriteLine("Disconnected from user socket.");
        Logging.LogInformation("Disconnected from user socket");
    }

    public static Uri? GetWsUri()
    {
        return _webSocket?.Url;
    }
}