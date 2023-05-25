using Shared;

namespace ServerApplication.Model;

[Serializable]
public class JsonWorkTask
{
    public int TaskId { get; set; }
    public GameType GameType { get; set; }
    public TaskDifficulty TaskDifficulty { get; set; }

    public JsonWorkTask(int taskId, GameType gameType, TaskDifficulty taskDifficulty)
    {
        TaskId = taskId;
        GameType = gameType;
        TaskDifficulty = taskDifficulty;
    }
}