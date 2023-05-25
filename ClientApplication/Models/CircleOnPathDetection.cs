namespace ClientApplication.Models;

public class CircleOnPathDetection
{
    private CircleAndPathSnapshot? _previousSnapshot;
    private CircleAndPathSnapshot? _currentSnapshot;

    public void UpdateCurrentSnapshot(CircleAndPathSnapshot snapshot)
    {
        _previousSnapshot = _currentSnapshot;
        _currentSnapshot = snapshot;
    }

    public double GetDistanceSinceLastPointPixels()
    {
        if (_previousSnapshot == null || _currentSnapshot == null) return 0;
        if (!_previousSnapshot.IsCircleOnPath || !_currentSnapshot.IsCircleOnPath) return 0;
        return _currentSnapshot.XValue - _previousSnapshot.XValue;
    }
    
}