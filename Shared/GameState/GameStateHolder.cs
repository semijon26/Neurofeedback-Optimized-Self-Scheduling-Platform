namespace Shared.GameState;
[Serializable]
public class GameStateHolder
{
    public BackTrackGameState? BackTrackGameState { get; set; }
    public BricketBreakerGameState? BricketBreakerGameState { get; set; }
    public MemoMasterGameState? MemoMasterGameState { get; set; }
    public PathPilotGameState? PathPilotGameState { get; set; }
    public TextGameGameState? TextGameGameState { get; set; }
}