namespace ClientApplication.Models;

/// <summary>
///  Speichert zwei Snapshots, um zu berechnen, ob die Kugel im RoadRacer auf dem Pfad geblieben ist
/// </summary>
public class CircleOnPathDetection
{
    private CircleAndPathSnapshot? _previousSnapshot;
    private CircleAndPathSnapshot? _currentSnapshot;

    public void UpdateCurrentSnapshot(CircleAndPathSnapshot snapshot)
    {
        _previousSnapshot = _currentSnapshot;
        _currentSnapshot = snapshot;
    }

    // Berechnet die Strecke zwischen den zwei Snapshots
    public double GetDistanceSinceLastPointPixels()
    {
        if (_previousSnapshot == null || _currentSnapshot == null) return 0;
        if (!_previousSnapshot.IsCircleOnPath || !_currentSnapshot.IsCircleOnPath) return 0;
        return _currentSnapshot.XValue - _previousSnapshot.XValue;
    }
    
}