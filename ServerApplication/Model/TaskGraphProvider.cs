using Shared;

namespace ServerApplication.Model;

public class TaskGraphProvider
{
    public TaskGraph TaskGraph { get; }


    public TaskGraphProvider(TaskGraph initialTaskGraph)
    {
        TaskGraph = initialTaskGraph;
    }

    public byte[] GetSerializedTaskGraph()
    {
        return SocketMessageHelper.SerializeToByteArray(TaskGraph);
    }
}