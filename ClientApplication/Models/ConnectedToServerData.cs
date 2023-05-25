namespace ClientApplication.Models;

public class ConnectedToServerData
{
    public bool? IsConnected { get; set; }
    
    private static ConnectedToServerData? _instance;

    private ConnectedToServerData()
    {
    }
    public static ConnectedToServerData GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ConnectedToServerData();
        }
        return _instance;
    }
}