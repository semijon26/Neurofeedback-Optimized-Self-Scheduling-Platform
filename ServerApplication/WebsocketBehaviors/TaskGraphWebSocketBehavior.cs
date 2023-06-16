using ServerApplication.Model;
using ServerApplication.modules;
using Shared;
using Shared.GameState;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerApplication.WebsocketBehaviors;
/// <summary>
/// Socket, der alle Nachrichte, die den Taskgraphen betreffen, regelt.
/// </summary>
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
        var payload = SocketMessageHelper.DeserializeFromByteArray<DataPayload>(e.RawData);

        // Zugriff auf die übertragenen Werte
        bool updateTask = payload.SetDone;
        bool changeWoker = payload.ChangeWorker;
        int intValue = payload.IntValue;
        ClientObject? workerWithPulledTask = payload.WorkerWithPulledTask;
        ClientObject? workerRemovesPulledTask = payload.WorkerRemovesPulledTask;

        var taskId = intValue;

        // Hier kannst du die Logik für beide Werte implementieren
        if (updateTask)
        {
            _provider.TaskGraph.GetTaskById(taskId)?.SetDone();
            SocketServerService.WebSocketServer.WebSocketServices["/taskgraph"].Sessions
                .Broadcast(_provider.GetSerializedTaskGraph());
        }
        else
        {
            if (!changeWoker && workerWithPulledTask != null)
            {
                _provider.TaskGraph.GetTaskById(taskId)?.ChangeWoker(workerWithPulledTask);
                SocketServerService.WebSocketServer.WebSocketServices["/taskgraph"].Sessions
                    .Broadcast(_provider.GetSerializedTaskGraph());
            }

            if (changeWoker && workerWithPulledTask != null && workerRemovesPulledTask != null)
            {
                AbstractGameState? currentGameState = null;
                foreach (var keyValuePair in SocketServerService.Clients)
                {
                    if (keyValuePair.Value.UniqueId == workerRemovesPulledTask.UniqueId)
                    {
                        var gameStateHolder = workerRemovesPulledTask.ActiveGames[taskId].GameStateHolder;
                        if (gameStateHolder.BricketBreakerGameState != null)
                        {
                            currentGameState = gameStateHolder.BricketBreakerGameState;
                        }
                        else if (gameStateHolder.BackTrackGameState != null)
                        {
                            currentGameState = gameStateHolder.BackTrackGameState;
                        }
                        else if (gameStateHolder.TextGameGameState != null)
                        {
                            currentGameState = gameStateHolder.TextGameGameState;
                        }
                        else if (gameStateHolder.MemoMasterGameState != null)
                        {
                            currentGameState = gameStateHolder.MemoMasterGameState;
                        }
                        else if (gameStateHolder.RoadRacerGameState != null)
                        {
                            currentGameState = gameStateHolder.RoadRacerGameState;
                        }

                        keyValuePair.Value.ActiveGames.Remove(taskId);
                    }
                }

                foreach (var keyValuePair in SocketServerService.Clients)
                {
                    if (keyValuePair.Value.UniqueId == workerWithPulledTask.UniqueId)
                    {
                        keyValuePair.Value.AddNewActiveGame(taskId, _provider.TaskGraph.GetTaskById(taskId).GameType,
                            currentGameState);
                    }
                }

                _provider.TaskGraph.GetTaskById(taskId).ChangeWoker(workerWithPulledTask);
                var clients = GetClientsList();
                SocketServerService.WebSocketServer.WebSocketServices["/taskgraph"].Sessions
                    .Broadcast(_provider.GetSerializedTaskGraph());
                SocketServerService.WebSocketServer.WebSocketServices["/clients"].Sessions
                    .Broadcast(SocketMessageHelper.SerializeToByteArray(clients));
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

    private void SetCurrentGameState(GameStateHolder gameStateHolder, AbstractGameState? currentGameState)
    {
        if (gameStateHolder.BricketBreakerGameState != null)
        {
            currentGameState = gameStateHolder.BricketBreakerGameState;
        }
        else if (gameStateHolder.BackTrackGameState != null)
        {
            currentGameState = gameStateHolder.BackTrackGameState;
        }
        else if (gameStateHolder.TextGameGameState != null)
        {
            currentGameState = gameStateHolder.TextGameGameState;
        }
        else if (gameStateHolder.MemoMasterGameState != null)
        {
            currentGameState = gameStateHolder.MemoMasterGameState;
        }
        else if (gameStateHolder.RoadRacerGameState != null)
        {
            currentGameState = gameStateHolder.RoadRacerGameState;
        }
    }
}