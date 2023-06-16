namespace Shared;


/// <summary>
/// Der TaskGraph ist im Kern ein azyklischer, gerichteter Graph, der mittels Adjazenzliste dargestellt wird
/// </summary>
[Serializable]
public class TaskGraph
{
    public Dictionary<TaskGroup, List<TaskGroup>> AdjacencyList = new();
    
    public Dictionary<TaskGroup, List<TaskGroup>> Predecessors = new();

    public void AddTaskGroup(TaskGroup group)
    {
        if (AdjacencyList.ContainsKey(group)) throw new Exception("Duplicate taskGroup");

        AdjacencyList[group] = new List<TaskGroup>();
        Predecessors[group] = new List<TaskGroup>();
    }

    public void AddConnection(TaskGroup from, TaskGroup to)
    {
        if (AdjacencyList[from].Contains(to) || from == to)
            throw new Exception("Invalid connection between taskGroups");

        AdjacencyList[from].Add(to);
        Predecessors[to].Add(from);
    }

    public string PrintConnections()
    {
        var str = "Taskgraph: \n";
        foreach (var taskGroup in AdjacencyList.Keys)
        {
            str += $" {taskGroup.PrintTasks()} connected to: ";
            var adjTaskGroups = AdjacencyList[taskGroup];
            str = adjTaskGroups.Aggregate(str, (current, adjTaskGroup) => current + $"{adjTaskGroup.PrintTasks()}  ");
            str += "\n";
        }

        return str;
    }

    public WorkTask? GetTaskById(int id)
    {
        WorkTask? task = null;
        foreach (var tg in AdjacencyList.Keys)
        {
            foreach (var t in tg.Tasks)
            {
                if (t.Id == id)
                {
                    task = t;
                    break;
                }
            }
        }

        return task;
    }

    public List<TaskGroup> GetAdjacentGroupsOf(int groupId)
    {
        var pair = AdjacencyList.FirstOrDefault(pair => pair.Key.Id == groupId);
        return pair.Value;
    }

    public TaskGroup? GetTaskGroupById(int id)
    {
        TaskGroup? taskGroup = null;
        foreach (var tg in AdjacencyList.Keys)
        {
            if (tg.Id == id)
            {
                taskGroup = tg;
                break;
            }
        }

        return taskGroup;
    }

    public List<TaskGroup> GetAvailableTaskGroups()
    {
        var availableTaskGroups = new List<TaskGroup>();
        foreach (var taskGroup in Predecessors.Keys)
        {
            var isAvailable = false;
            if (Predecessors[taskGroup].Count == 0)
            {
                isAvailable = true;
                taskGroup.Executable = true;
            }
            else
            {
                foreach (var predecessor in Predecessors[taskGroup])
                {
                    if (predecessor.IsDone())
                    {
                        isAvailable = true;
                        taskGroup.Executable = true;
                    }
                    else
                    {
                        isAvailable = false;
                        taskGroup.Executable = false;
                        break;
                    }
                }
            }

            if (isAvailable) availableTaskGroups.Add(taskGroup);
        }

        return availableTaskGroups;
    }

    public List<TaskGroup> GetNotAvailableTaskGroups()
    {
        return AdjacencyList.Keys.Except(GetAvailableTaskGroups()).ToList();
    }

    public void SetTaskDoneById(int id)
    {
        GetTaskById(id)?.SetDone();
    }

    public void SetWorkerById(int id, ClientObject clientObject)
    {
        GetTaskById(id)?.ChangeWoker(clientObject);
    }
}