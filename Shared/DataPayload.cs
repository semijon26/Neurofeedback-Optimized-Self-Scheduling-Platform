namespace Shared;
[Serializable]
public class DataPayload
{
    public bool SetDone { get; set; }
    public bool ChangeWorker { get; set; }
    public int IntValue { get; set; }
    public ClientObject? WorkerWithPulledTask { get; set; }
    public ClientObject? WorkerRemovesPulledTask { get; set; }
}