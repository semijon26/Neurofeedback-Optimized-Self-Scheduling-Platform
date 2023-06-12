using Shared;

namespace ClientApplication.Utils;

public class Line
{
    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }
    public TaskGroup SourceGroup { get; set; }
    public TaskGroup DestGroup { get; set; }
    public bool Visibility => CheckVisibility();

    public bool CheckVisibility()
    {
        if (TaskGraphProvider.GetInstance().TaskGraph.GetAvailableTaskGroups().Contains(DestGroup))
        {
            return true;
        }
        return false;
    }
}