namespace ServerApplication.Model;

[Serializable]
public class JsonTaskGroup
{
    public int GroupId { get; set; }
    public List<JsonWorkTask> Tasks { get; set; }

    public JsonTaskGroup(int groupId, List<JsonWorkTask> tasks)
    {
        GroupId = groupId;
        Tasks = tasks;
    }
}