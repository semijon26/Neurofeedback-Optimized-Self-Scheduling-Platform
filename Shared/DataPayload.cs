namespace Shared;

public class DataPayload
{
    public bool SetDone { get; set; }
    public bool ChangeWorker { get; set; }
    public int IntValue { get; set; }
    public ClientObject Woker { get; set; }
    public ChangeObject ChangeObject { get; set; }
}