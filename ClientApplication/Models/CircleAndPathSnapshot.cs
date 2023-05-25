namespace ClientApplication.Models;

public class CircleAndPathSnapshot
{
    public double XValue;
    public bool IsCircleOnPath;

    public CircleAndPathSnapshot(double xValue, bool isCircleOnPath)
    {
        XValue = xValue;
        IsCircleOnPath = isCircleOnPath;
    }
}