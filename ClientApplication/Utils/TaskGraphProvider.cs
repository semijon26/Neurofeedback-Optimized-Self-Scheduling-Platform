using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Shared;
using WebSocketSharp;

namespace ClientApplication.Utils;

public class TaskGraphProvider : INotifyPropertyChanged
{
    private WebSocket _webSocket;
    private static TaskGraphProvider? _instance;
    private TaskGraph? _taskGraph;
    
    private TaskGraphProvider()
    {
    }

    public static TaskGraphProvider GetInstance()
    {
        if (_instance == null)
        {
            _instance = new TaskGraphProvider();
        }
        
        return _instance;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public TaskGraph? TaskGraph
    {
        get => _taskGraph;
        set
        {
            _taskGraph = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskGraph)));
        }
    }
        
    
    public void InitTaskGraphProvider(string ip, int port)
    {
        try
        {
            _webSocket = new WebSocket($"ws://{ip}:{port}/taskgraph");
            _webSocket.OnMessage += OnMessage;
            _webSocket.Connect();
        }catch (Exception ex)
        {
            Logging.LogError(message: "Init Task Graph Websocket failed", ex: ex);
        }
    }

    private void OnMessage(object? sender, MessageEventArgs e)
    {
        Logging.LogInformation("------- Task Graph received --------");
        var taskGraph = SocketMessageHelper.DeserializeFromByteArray<TaskGraph>(e.RawData);
        TaskGraph = taskGraph;
    }

    public void SendUpdatedTaskGraphToServer(DataPayload task)
    {
        _webSocket.Send(SocketMessageHelper.SerializeToByteArray(task));
    }
    
    private byte[] GetSerializedTaskGraph()
    {
        return SocketMessageHelper.SerializeToByteArray(TaskGraph);
    }
}