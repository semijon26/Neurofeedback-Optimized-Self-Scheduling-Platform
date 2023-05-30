using System;
using System.Collections.Generic;
using ClientApplication.Models;
using Shared;

namespace ClientApplication.Utils;

public static class PointCalculator
{
    
    public static Dictionary<TaskGroup, TaskPoint> CalculateDrawingPoints(Dictionary<int, List<TaskGroup>> Layers, int canvasWidth, int groupWidth, int groupHeight)
    {
        Dictionary<TaskGroup, TaskPoint> current_ = new Dictionary<TaskGroup, TaskPoint>();
        int CanvasWidth = canvasWidth;
        int initialYValue = 0;
        int initialWidth = groupWidth;
        foreach (int key in Layers.Keys)
        {
            int circleDistance;
            int counter = 1;
            // Where(TaskGraphProvider.GetInstance().TaskGraph.GetAvailableTaskGroups().Contains)
            foreach (TaskGroup group in Layers[key])
            {
                int NumberOfNodes = Layers[key].Count;
                int NumberOfTasks = group.Tasks.Count;
                circleDistance = CanvasWidth / (NumberOfNodes+1);
                int step = circleDistance;
                int y_value = initialYValue;
                int x_value =  counter * step;
                TaskPoint point = new TaskPoint { X = x_value, Y = y_value, Width = NumberOfTasks*initialWidth};
                current_[group] = point;
                counter++;
            }
            initialYValue += groupHeight;
        }

        return current_;
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