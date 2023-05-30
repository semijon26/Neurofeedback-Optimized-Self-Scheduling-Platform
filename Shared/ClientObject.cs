
namespace Shared;
[Serializable]
public class ClientObject
{
    private static ClientObject? _instance;
    private ClientObject()
    {
        UniqueId = Guid.NewGuid();
        Label = "";
        ActiveGames = new Dictionary<int, GameObject>();
        MinClientsToStartGames = 7;
        Random random = new Random();
        A = 255;
        R = (byte)random.Next(100, 256);
        G = (byte)random.Next(100, 256);
        B = (byte)random.Next(100, 256);
    }

    public static ClientObject GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ClientObject();
            return _instance;
        }
        return _instance;
    }
    public Guid UniqueId { get; private set; }
    public string Label{ get; set; }
    public Dictionary<int, GameObject> ActiveGames { get; set; }

    public int MinClientsToStartGames;

    public int? PulledTaskId = null;

    public bool ContainsGameType(GameType gameType)
    {
        return ActiveGames.Values.Any(gameObj => gameObj.GameType == gameType);
    }

    public void AddNewActiveGame(int taskId, GameType gameType)
    {
        if (!ActiveGames.Values.Any(item => item is { Row: 0, Column: 0 }))
        {
            ActiveGames.Add(taskId, new GameObject(gameType, 0, 0));
        }else if (!ActiveGames.Values.Any(item => item is { Row: 0, Column: 1 }))
        {
            ActiveGames.Add(taskId, new GameObject(gameType, 0, 1));
        }else if (!ActiveGames.Values.Any(item => item is { Row: 1, Column: 0 }))
        {
            ActiveGames.Add(taskId, new GameObject(gameType, 1, 0));
        }else if (!ActiveGames.Values.Any(item => item is { Row: 1, Column: 1 }))
        {
            ActiveGames.Add(taskId, new GameObject(gameType, 1, 1));
        }
    }
    
    public byte A { get; set; }
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
}