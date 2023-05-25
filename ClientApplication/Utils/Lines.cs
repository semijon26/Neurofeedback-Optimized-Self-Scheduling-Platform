using Shared;

namespace ClientApplication.Utils;

public class Line
{
    public int X1 { get; set; }
    public int X2 { get; set; }
    public int Y1 { get; set; }
    public int Y2 { get; set; }
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