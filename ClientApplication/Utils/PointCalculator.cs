using System;
using System.Collections.Generic;
using System.Linq;
using ClientApplication.Models;
using Shared;

namespace ClientApplication.Utils;

public static class PointCalculator
{

    public static List<int> CalculateActiveArea(Dictionary<TaskGroup, TaskPoint> TaskPointDictionary)
    {
        Dictionary<TaskGroup, TaskPoint> filteredDictionary = TaskPointDictionary
            .Where(pair => pair.Key.Executable && !pair.Key.IsDone())
            .ToDictionary(pair => pair.Key, pair => pair.Value)!;
        
        TaskPoint lowestPoint = filteredDictionary.Values.MinBy(point => point.Y);
        TaskPoint highestPoint = filteredDictionary.Values.MaxBy(point => point.Y);
        return new List<int> { lowestPoint.Y, highestPoint.Y + 10 };
    }

    public static Dictionary<TaskGroup, TaskPoint> CalculateDrawingPoints(Dictionary<int, List<TaskGroup>> Layers, int canvasWidth, int groupWidth, int groupHeight)
    {
        Dictionary<TaskGroup, TaskPoint> current = new Dictionary<TaskGroup, TaskPoint>();
        int CanvasWidth = canvasWidth;
        int initialYValue = 0;
        int initialWidth = groupWidth;
        foreach (int key in Layers.Keys)
        {
            int temp = 0;
            int wholeTaskNumber = Layers.TryGetValue(key, out var layer) ? layer.Sum(taskGroup => taskGroup.Tasks.Count) : 0;
            int circleDistance = (CanvasWidth / (wholeTaskNumber+1));
            foreach (TaskGroup group in Layers[key])
            {
                int numberOfTasks = group.Tasks.Count;
                int yValue = initialYValue;
                int xValue =  circleDistance * temp;
                temp += numberOfTasks;
                TaskPoint point = new TaskPoint { X = xValue, Y = yValue, Width = numberOfTasks*initialWidth};
                current[group] = point;
                Logging.LogInformation($"TaskGroup {group.Id} - y:{yValue} x:{xValue}");
            }
            initialYValue += groupHeight;
        }

        return current;
    }
    
    public static List<Line> CalculateDrawingLines(Dictionary<TaskGroup, TaskPoint> TaskPointsDictionary, int groupHeight)
    {
        int height = groupHeight;
        Dictionary<int, List<int>> _lineHelper = new Dictionary<int, List<int>>();
        List<Line> _current = new List<Line>();
        foreach (var group in TaskPointsDictionary.Keys)
        {
            _lineHelper.Add(group.Id, new List<int>());
                
            foreach (var destNode in TaskGraphProvider.GetInstance().TaskGraph.GetAdjacentGroupsOf(group.Id))
            {
                try
                {
                    int x1 = TaskPointsDictionary[getTaskGroup(group.Id, TaskPointsDictionary)].X;
                    int y1 = TaskPointsDictionary[getTaskGroup(group.Id, TaskPointsDictionary)].Y;
                    int width1 = TaskPointsDictionary[getTaskGroup(group.Id, TaskPointsDictionary)].Width;
                    int x2 = TaskPointsDictionary[getTaskGroup(destNode.Id, TaskPointsDictionary)].X;
                    int y2 = TaskPointsDictionary[getTaskGroup(destNode.Id, TaskPointsDictionary)].Y;
                    int width2 = TaskPointsDictionary[getTaskGroup(destNode.Id, TaskPointsDictionary)].Width;
                    _current.Add(new Line {X1 = x1+width1/2, X2 = x2+width2/2, Y1 = y1+height/2, Y2 = y2+height/2, SourceGroup = group, DestGroup = destNode});
                }
                catch (Exception e)
                {
                    Logging.LogError("Thrown in Line Calculation: " + e.Message);
                }
                    
            }
        }

        return _current;
    }
    
    
    // ID muss unique sein --> keine Dopplung, sonst fehlerhafte Zuweisung
    private static TaskGroup getTaskGroup(int Id, Dictionary<TaskGroup, TaskPoint> TaskPointsDictionary)
    {
        foreach (var task in TaskPointsDictionary.Keys)
        {
            if (task.Id == Id)
            {
                return task;
            }
        }
        return null;
    }
}