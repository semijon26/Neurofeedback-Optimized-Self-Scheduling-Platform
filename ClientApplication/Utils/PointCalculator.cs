using System;
using System.Collections.Generic;
using System.Linq;
using ClientApplication.Models;
using Shared;

namespace ClientApplication.Utils;

public static class PointCalculator
{

    public static List<double> CalculateActiveArea(Dictionary<TaskGroup, TaskPoint> TaskPointDictionary)
    {
        Dictionary<TaskGroup, TaskPoint> filteredDictionary = TaskPointDictionary
            .Where(pair => pair.Key.Executable && !pair.Key.IsDone())
            .ToDictionary(pair => pair.Key, pair => pair.Value)!;
        
        TaskPoint lowestPoint = filteredDictionary.Values.MinBy(point => point.Y);
        TaskPoint highestPoint = filteredDictionary.Values.MaxBy(point => point.Y);
        return new List<double> { lowestPoint.Y, highestPoint.Y + 10 };
    }

    public static Dictionary<TaskGroup, TaskPoint> CalculateDrawingPoints(Dictionary<int, List<TaskGroup>> Layers, double canvasWidth, double groupWidth, double groupHeight)
    {
        Dictionary<TaskGroup, TaskPoint> current = new Dictionary<TaskGroup, TaskPoint>();
        double CanvasWidth = canvasWidth;
        double initialYValue = 0;
        double taskWidth = groupWidth;
        Logging.LogInformation($"CanvasWidth: {CanvasWidth}");
        foreach (int key in Layers.Keys)
        {
            int wholeTaskNumber = Layers.TryGetValue(key, out var layer) ? layer.Sum(taskGroup => taskGroup.Tasks.Count) : 0;
            double neededArea = wholeTaskNumber*taskWidth;
            double whiteSpace = CanvasWidth - neededArea;
            double circleDistance = whiteSpace / Layers[key].Count;
            double temp = circleDistance / 2;
            
            foreach (TaskGroup group in Layers[key])
            {
                int numberOfTasks = group.Tasks.Count;
                double yValue = initialYValue;
                double xValue = temp;

                if (temp == 0.5 * circleDistance)
                {
                    temp = 0;
                }
    
                temp += numberOfTasks * taskWidth + circleDistance; // Aktualisierung von temp
    
                TaskPoint point = new TaskPoint { X = xValue, Y = yValue, Width = numberOfTasks * taskWidth };
                current[group] = point;
                Logging.LogInformation($"TaskGroup {group.Id} - y:{yValue} x:{xValue}");
            }
            initialYValue += groupHeight;
        }

        return current;
    }
    
    public static List<Line> CalculateDrawingLines(Dictionary<TaskGroup, TaskPoint> TaskPointsDictionary, double groupHeight)
    {
        double height = groupHeight;
        Dictionary<int, List<int>> _lineHelper = new Dictionary<int, List<int>>();
        List<Line> _current = new List<Line>();
        foreach (var group in TaskPointsDictionary.Keys)
        {
            _lineHelper.Add(group.Id, new List<int>());
                
            foreach (var destNode in TaskGraphProvider.GetInstance().TaskGraph.GetAdjacentGroupsOf(group.Id))
            {
                try
                {
                    double x1 = TaskPointsDictionary[getTaskGroup(group.Id, TaskPointsDictionary)].X;
                    double y1 = TaskPointsDictionary[getTaskGroup(group.Id, TaskPointsDictionary)].Y;
                    double width1 = TaskPointsDictionary[getTaskGroup(group.Id, TaskPointsDictionary)].Width;
                    double x2 = TaskPointsDictionary[getTaskGroup(destNode.Id, TaskPointsDictionary)].X;
                    double y2 = TaskPointsDictionary[getTaskGroup(destNode.Id, TaskPointsDictionary)].Y;
                    double width2 = TaskPointsDictionary[getTaskGroup(destNode.Id, TaskPointsDictionary)].Width;
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