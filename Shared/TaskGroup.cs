namespace Shared;


/// <summary>
/// Es gibt TaskGruppen, die mehrere Tasks (Klasse: WorkTask) zusammenfassen, da dies das Handling der Tasks erleichtert
/// </summary>
[Serializable]
public class TaskGroup
{
    public List<WorkTask> Tasks { get; }
    public bool done { get; private set; }
    
    public int Id { get; }

    public bool Done => IsDone();
    
    public bool Executable { get; set; }

    public TaskGroup(int id, List<WorkTask> tasks)
    {
        Tasks = tasks;
        Id = id;
        done = false;
        Executable = false;
    }

    public void SetDone()
    {
        done = true;
    }

    public bool IsDone()
    {
        return Tasks.TrueForAll(task => task.IsDone);
    }

    public string PrintTasks()
    {
        var str = "[";
        for (var i = 0; i < Tasks.Count; i++)
        {
            str += Tasks[i];
            if (i <= Tasks.Count) str += ", ";
        }
        str += "]";
        return str;
    }
}