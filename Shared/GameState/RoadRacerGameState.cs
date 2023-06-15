namespace Shared.GameState;

/// <summary>
/// Diese Klasse dient dazu, den State des RoadRacers aufzubewahren
/// </summary>
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