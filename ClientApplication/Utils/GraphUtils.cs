using System.Collections.Generic;
using System.Text;
using Shared;

namespace ClientApplication.Utils;

public static class GraphUtils
{
    public static Dictionary<int, List<TaskGroup>> GetGraphLayers(TaskGraph graph)
    {
        Dictionary<TaskGroup, List<TaskGroup>> _graph = graph.AdjacencyList;
        Dictionary<TaskGroup, int> inDegree = new Dictionary<TaskGroup, int>();
        Dictionary<int, List<TaskGroup>> layers = new Dictionary<int, List<TaskGroup>>();
        Queue<TaskGroup> queue = new Queue<TaskGroup>();

        // Initialisiere Indegree für alle Knoten
        foreach (TaskGroup node in _graph.Keys)
        {
            inDegree[node] = 0;
        }

        // Berechne Indegree für alle Knoten
        foreach (TaskGroup node in _graph.Keys)
        {
            foreach (TaskGroup neighbor in _graph[node])
            {
                inDegree[neighbor]++;
            }
        }

        // Füge Knoten mit Indegree 0 in Ebene 0 ein
        int layer = 0;
        List<TaskGroup> nodesInCurrentLayer = new List<TaskGroup>();
        foreach (TaskGroup node in _graph.Keys)
        {
            if (inDegree[node] == 0)
            {
                nodesInCurrentLayer.Add(node);
                queue.Enqueue(node);
            }
        }
        layers[layer] = nodesInCurrentLayer;

        // Weise den Knoten Ebenen zu, indem wir Breitensuche anwenden
        while (queue.Count > 0)
        {
            layer++;
            nodesInCurrentLayer = new List<TaskGroup>();
            int currentLayerSize = queue.Count;
            for (int i = 0; i < currentLayerSize; i++)
            {
                TaskGroup currentNode = queue.Dequeue();
                foreach (TaskGroup neighbor in _graph[currentNode])
                {
                    inDegree[neighbor]--;
                    if (inDegree[neighbor] == 0)
                    {
                        nodesInCurrentLayer.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            layers[layer] = nodesInCurrentLayer;
        }

        return layers;
    }

    public static void LogCalculatedLayers(Dictionary<int, List<TaskGroup>> layers)
    {
        foreach (var key in layers.Keys)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var taskGroup in layers[key])
            {
                sb.Append($"-{taskGroup.Id}-");
            }

            sb.Append("\n");
            Logging.LogInformation(key.ToString() + " - " + sb);
            
        }
    }
}