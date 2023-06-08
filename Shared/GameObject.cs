using Shared.GameState;

namespace Shared;

[Serializable]
public class GameObject
{
    public GameType GameType{ get; set; }

    public GameStateHolder GameStateHolder = new();
    public int Row { get; set; }
    public int Column { get; set; }

    public GameObject(GameType gameType, AbstractGameState? gameState, int row, int column)
    {
        if (gameState is BackTrackGameState backTrackGameState)
        {
            GameStateHolder.BackTrackGameState = backTrackGameState;
        }else if (gameState is BricketBreakerGameState bricketBreakerGameState)
        {
            GameStateHolder.BricketBreakerGameState = bricketBreakerGameState;
        }else if (gameState is MemoMasterGameState memoMasterGameState)
        {
            GameStateHolder.MemoMasterGameState = memoMasterGameState;
        }else if (gameState is PathPilotGameState pathPilotGameState)
        {
            GameStateHolder.PathPilotGameState = pathPilotGameState;
        }else if (gameState is TextGameGameState textGameGameState)
        {
            GameStateHolder.TextGameGameState = textGameGameState;
        }
        GameType = gameType;
        Row = row;
        Column = column;
    }
}