using Shared;

namespace ClientApplication.Utils;
/// <summary>
///  Speichert die Koordinaten der Verbindungen zwischen den verschiedenen Knoten.
/// </summary>
public class Line
{
    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }
    public TaskGroup SourceGroup { get; set; }
    public TaskGroup DestGroup { get; set; }
    public bool Visibility => CheckVisibility();

    // Überprüfen, ob der DestinationNode ausführbar ist.
    // Damit kann der Linie in der UI die richtige Farbe gegeben werden.
    public bool CheckVisibility()
    {
        if (TaskGraphProvider.GetInstance().TaskGraph.GetAvailableTaskGroups().Contains(DestGroup))
        {
            return true;
        }
        return false;
    }
}