namespace Shared.GameState;
[Serializable]
public class RoadRacerGameState : AbstractGameState
{
    public double CurrentMeters;
    public int CurrentMetersFloored;
    public int TimeLeft;

    public RoadRacerGameState(double currentMeters, int currentMetersFloored, int timeLeft)
    {
        CurrentMeters = currentMeters;
        CurrentMetersFloored = currentMetersFloored;
        TimeLeft = timeLeft;
    }
}