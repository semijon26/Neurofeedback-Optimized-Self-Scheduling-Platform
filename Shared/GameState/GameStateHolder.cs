namespace Shared.GameState;
[Serializable]
public class GameStateHolder
{
    public BackTrackGameState? BackTrackGameState { get; set; }
    public BricketBreakerGameState? BricketBreakerGameState { get; set; }
    public MemoMasterGameState? MemoMasterGameState { get; set; }
    public RoadRacerGameState? RoadRacerGameState { get; set; }
    public TextGameGameState? TextGameGameState { get; set; }
}