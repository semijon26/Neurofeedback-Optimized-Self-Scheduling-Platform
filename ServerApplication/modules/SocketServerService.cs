using Newtonsoft.Json;
using ServerApplication.WebsocketBehaviors;
using Shared;
using WebSocketSharp;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp.Server;

namespace ServerApplication.modules
{
    public static class SocketServerService
    {
        // WebSocket, der als Server ausgeführt wird
        public static WebSocketServer WebSocketServer;
        // Liste aller verbundenen Clients
        private static List<WebSocketContext> connectedClients;
        // Locker, der die Threadsichere Ausführung gewährleistet.
        private static object locker;

        public static ConfigObject objectConfiguration;
        public static readonly Dictionary< WebSocketContext, ClientObject> Clients = new();

        public static void Start(int port, TaskGraph taskGraph)
        {
            connectedClients = new List<WebSocketContext>();
            locker = new object();
            WebSocketServer = new WebSocketServer($"ws://localhost:{port}");
            WebSocketServer.AddWebSocketService<MyWebSocket>("/");
            WebSocketServer.AddWebSocketService<LoggingWebSocketBehavior>("/logging");
            WebSocketServer.AddWebSocketService<UserWebSocketBehavior>("/users");
            WebSocketServer.AddWebSocketService<ClientWebsocketBehavior>("/clients");
            WebSocketServer.AddWebSocketService("/taskgraph", () => new TaskGraphWebSocketBehavior(taskGraph));
            WebSocketServer.Start();
        }

        private static void AddConnectedClient(WebSocketContext client)
        {
            lock (locker)
            {
                connectedClients.Add(client);
            }
        }

        private static void RemoveConnectedClient(WebSocketContext client)
        {
            lock (locker)
            {
                connectedClients.Remove(client);
            }
        }

        public static void Stop()
        {
            WebSocketServer?.Stop();
        }

        public static void SendAll(string message)
        {
            foreach (WebSocketContext client in connectedClients)
            {
                client.WebSocket.Send(message);
            }
        }

        public static void SendByte(byte[] data)
        {
            foreach (WebSocketContext client in connectedClients)
            {
                Logging.LogInformation("Client sent");
                client.WebSocket.Send(data);
            }
        }

        public static void SendCurrentUsers()
        {
            string data = GetUserJson();
            WebSocketServer.WebSocketServices["/users"].Sessions.Broadcast(data);
        }
        public static string GetUserJson()
        {
            List<string> clientNames = new List<string>();
            foreach (var client in connectedClients)
            {
                string name = client.QueryString["param"];
                Console.WriteLine(name);
                clientNames.Add(name);
            }
            string user_json = JsonConvert.SerializeObject(clientNames);
            //client.WebSocket.Send(user_json);
            //webSocketServer.WebSocketServices["/users"].Sessions.Broadcast("Hello, clients!");
            return user_json;
        }

        public static byte[] SerializeObjectByte(ConfigObject obj)
        {
            return SocketMessageHelper.SerializeToByteArray(obj);
        }


        private class UserWebSocketBehavior : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                Console.WriteLine(e.Data);
                SendCurrentUsers();
            }
        }
        private class MyWebSocket : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                // Handle message
                Console.WriteLine($"Received message: {e.Data}");
                if (e.Data != ".")
                {
                    Send($"Deine Nachricht ({e.Data}) kam erfolgreich an.");
                }
                else
                {
                    SendByte(SerializeObjectByte(objectConfiguration));
                }
            }

            protected override void OnOpen()
            {
                AddConnectedClient(Context);
                Console.WriteLine($"{Context.UserEndPoint.Address} hat sich mit dem Server verbunden. Username: {Context.QueryString["param"]}");
                Console.WriteLine($"Anzahl der verbundenen Clients: {connectedClients.Count}");
                // Send a message to all connected clients on the /users socket
                WebSocketServer.WebSocketServices["/users"].Sessions.Broadcast(GetUserJson());
                Console.WriteLine(GetUserJson());
            }

            protected override void OnClose(CloseEventArgs e) 
            {
                RemoveConnectedClient(Context);
                Console.WriteLine($"Verbindung zu getrennt");
                Console.WriteLine($"Anzahl der verbundenen Clients: {connectedClients.Count}");
                // Send a message to all connected clients on the /users socket
                WebSocketServer.WebSocketServices["/users"].Sessions.Broadcast(GetUserJson());
            }
        }
    }
}

