namespace Shared;

[Serializable]
public class WorkTask
{
    public int Id { get; }
    public GameType GameType { get; }
    public TaskDifficulty Difficulty { get; }
    public bool IsDone { get; private set; }
    public ClientObject Worker { get; private set; }

    public WorkTask(int id, GameType gameType, TaskDifficulty difficulty)
    {
        Id = id;
        Difficulty = difficulty;
        GameType = gameType;
        Worker = null;
    }

    public void SetDone()
    {
        IsDone = true;
    }

    public void ChangeWoker(ClientObject clientObject)
    {
        Worker = clientObject;
    }

    public override string ToString()
    {
        return $"Id: {Id}, GameType: {GameType}, Difficulty: {Difficulty}, IsDone: {IsDone}";
    }
}