namespace Shared;

[Serializable]
public class GameObject
{
    public GameType GameType{ get; set; }
    public int Row { get; set; }
    public int Column { get; set; }

    public GameObject(GameType gameType, int row, int column)
    {
        GameType = gameType;
        Row = row;
        Column = column;
    }
}