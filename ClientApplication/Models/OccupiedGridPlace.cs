using Shared;

namespace ClientApplication.Models;

public class OccupiedGridPlace
{
    public GameType GameType{ get; set; }
    public int Row { get; set; }
    public int Column { get; set; }

    public OccupiedGridPlace(GameType gameType, int row, int column)
    {
        GameType = gameType;
        Row = row;
        Column = column;
    }
}