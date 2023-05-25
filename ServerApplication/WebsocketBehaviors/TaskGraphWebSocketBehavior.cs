using Newtonsoft.Json;
using ServerApplication.Model;
using ServerApplication.modules;
using Shared;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerApplication.WebsocketBehaviors;

public class TaskGraphWebSocketBehavior : WebSocketBehavior
{
    private readonly TaskGraphProvider _provider;
    
    public TaskGraphWebSocketBehavior(TaskGraph taskGraph)
    {
        _provider = new TaskGraphProvider(initialTaskGraph: taskGraph);
    }
    
    protected override void OnOpen()
    {
        // Initially, the task graph is sent to each client that connects to the server.
        Send(_provider.GetSerializedTaskGraph());
    }
    
    protected override void OnMessage(MessageEventArgs e)
    {
        
        if (e.Data.StartsWith("{") && e.Data.EndsWith("}"))
        {
            var payload = JsonConvert.DeserializeObject<DataPayload>(e.Data);

            // Zugriff auf die übertragenen Werte
            bool updateTask = payload.SetDone;
            bool changeWoker = payload.ChangeWorker;
            int intValue = payload.IntValue;
            ClientObject worker = payload.Woker;
            
            var taskId = intValue;

            // Hier kannst du die Logik für beide Werte implementieren
            if (updateTask)
            {
                _provider.TaskGraph.GetTaskById(taskId)?.SetDone();
                SocketServerService.WebSocketServer.WebSocketServices["/taskgraph"].Sessions.Broadcast(_provider.GetSerializedTaskGraph());
            }
            else
            {
                if (!changeWoker)
                {
                    _provider.TaskGraph.GetTaskById(taskId)?.ChangeWoker(worker);
                    SocketServerService.WebSocketServer.WebSocketServices["/taskgraph"].Sessions.Broadcast(_provider.GetSerializedTaskGraph());
                }

                if (changeWoker)
                {
                    foreach (var keyValuePair in SocketServerService.Clients)
                    {
                        if (keyValuePair.Value.Label == worker.Label)
                        {
                            SocketServerService.Clients[keyValuePair.Key].AddNewActiveGame(taskId, _provider.TaskGraph.GetTaskById(taskId).GameType);
                        }

                        if (keyValuePair.Value.Label == _provider.TaskGraph.GetTaskById(taskId).Worker.Label)
                        {
                            SocketServerService.Clients[keyValuePair.Key].ActiveGames.Remove(taskId);
                        }
                    }
                    _provider.TaskGraph.GetTaskById(taskId).ChangeWoker(worker);
                    var clients = GetClientsList();
                    SocketServerService.WebSocketServer.WebSocketServices["/taskgraph"].Sessions.Broadcast(_provider.GetSerializedTaskGraph());
                    SocketServerService.WebSocketServer.WebSocketServices["/clients"].Sessions.Broadcast(SocketMessageHelper.SerializeToByteArray(clients));
                }
            }
        }
    }
    
    private List<ClientObject> GetClientsList()
    {
        List<ClientObject> clientsList = new List<ClientObject>();
        foreach (var keyValuePair in SocketServerService.Clients)
        {
            clientsList.Add(keyValuePair.Value);
        }

        return clientsList;
    }

}