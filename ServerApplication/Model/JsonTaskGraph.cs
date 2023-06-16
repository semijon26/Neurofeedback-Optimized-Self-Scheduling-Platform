using Shared;

namespace ServerApplication.Model;

/// <summary>
/// Diese Klasse liest die JSON Repräsentation des Taskgraphen in der taskgraph.json File aus
/// </summary>
[Serializable]
public class JsonTaskGraph
{
    public List<JsonTaskGroup> TaskGroups { get; set; }
    public Dictionary<int, List<int>> ConnectionsFromTo { get; set; }

    public JsonTaskGraph(List<JsonTaskGroup> taskGroups, Dictionary<int, List<int>> connectionsFromTo)
    {
        TaskGroups = taskGroups;
        ConnectionsFromTo = connectionsFromTo;
    }

    // Der TaskGraph in Form dieser JSON Klasse wird in die Klasse umgewandelt, die in der Anwendung genutzt wird
    public TaskGraph ConvertToTaskGraph()
    {
        var taskGraph = new TaskGraph();
        var taskGroups = new List<TaskGroup>();
        foreach (var jtg in TaskGroups)
        {
            var workTasks = new List<WorkTask>();
            foreach (var jwt in jtg.Tasks)
            {
                workTasks.Add(new WorkTask(jwt.TaskId, jwt.GameType, jwt.TaskDifficulty));
            }

            taskGroups.Add(new TaskGroup(jtg.GroupId, workTasks));
        }

        foreach (var taskGroup in taskGroups)
        {
            taskGraph.AddTaskGroup(taskGroup);
        }

        foreach (var (taskFromId, taskToIds) in ConnectionsFromTo)
        {
            foreach (var taskToId in taskToIds)
            {
                taskGraph.AddConnection(
                    taskGraph.GetTaskGroupById(taskFromId)!,
                    taskGraph.GetTaskGroupById(taskToId)!
                );
            }
        }

        return taskGraph;
    }
}