namespace ClientApplication.Models;

 
    /// <summary>
    /// Diese Klasse speichert immer einen x-Wert der Kugel im RoadRacer und ob die Kugel an der Stelle auf dem Pfad war
    /// </summary>
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