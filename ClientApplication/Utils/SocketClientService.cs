using System;
using System.Threading.Tasks;
using Shared;
using WebSocketSharp;

/// <summary>
///  SocketHelper, der für die Verbindung des Clienten zu den jeweiligen Server-Sockets verantwortlich ist.
/// </summary>

namespace ClientApplication.Utils
{
    public static class SocketClientService
    {
        private static WebSocket? _webSocket;
        public static event EventHandler? ConnectionEstablished;
        public static event EventHandler? ConnectionClosed;


        // Action die ausgeführt wird, wenn eine Nachricht eintrifft
        public static Action<string> OnMessageStringReceived;
        public static Action<string> OnMessageObjectReceived;


        public static async Task Connect(string ip, int port, string name)
        {
            await Task.Run(() =>
            {
                    ClientObject.GetInstance().Label = name;
                    InitMainWebSocket(ip, port, name);
                    Logging.InitServerLogging(ip, port);
                    UserManagement.InitUserWebSocket(ip, port);
                    ClientManagementSocket.InitClientSocket(ip, port);
                    TaskGraphProvider.GetInstance().InitTaskGraphProvider(ip, port);
                    GameStateSocket.InitGameStateSocket(ip, port);
                });
        }

        // Haupt-Websockets Initialisieren, und Name übergeben
        private static void InitMainWebSocket(string ip, int port, string name)
        {
            try
            {
                _webSocket = new WebSocket($"ws://{ip}:{port}/?param={name}");
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
        
        public static async Task Send(string message)
        {
            if (_webSocket != null && _webSocket.ReadyState == WebSocketState.Open)
            {
                _webSocket.Send(message);
            }
        }

        public static async Task Close()
        {
            if (_webSocket != null && _webSocket.ReadyState == WebSocketState.Open)
            {
                _webSocket.Close();
            }
        }

        private static void OnOpen(object? sender, EventArgs e)
        {
            Console.WriteLine("Connected to server.");
            Logging.LogInformation("Connected to Server");
            OnConnectionEstablished();
        }

        private static void OnMessage(object? sender, MessageEventArgs e)
        {
            //Console.WriteLine($"Received message: {e.Data}");

            if (e.Data != null)
            {
                // OnMessagereceived - Delegate aufrufn, um den Text im Mainwindow anzuzeigen
                OnMessageStringReceived?.Invoke(e.Data);
                Console.WriteLine(e.Data);
                Logging.LogInformation($"Following message received: {e.Data}");
            }
            else
            {
                ConfigObject obj = SocketMessageHelper.DeserializeFromByteArray<ConfigObject>(e.RawData);
                string text = $"Object received --> Host: {obj.Host}, Participants: {obj.Participants}, ServerIP: {obj.ServerIP}";
                Logging.LogInformation(text);
                OnMessageObjectReceived?.Invoke(text);

            }
        }

        private static void OnClose(object? sender, CloseEventArgs e)
        {
            Console.WriteLine("Disconnected from server.");
            Logging.LogInformation("Disconnected from server");
            OnConnectionClosed();
        }

        private static void OnConnectionEstablished()
        {
            ConnectionEstablished?.Invoke(null, EventArgs.Empty);
        }

        private static void OnConnectionClosed()
        {
            ConnectionClosed?.Invoke(null, EventArgs.Empty);
        }

        public static Uri? GetWsUri()
        {
            return _webSocket?.Url;
        }
    }
}
