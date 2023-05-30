using System;
using System.Collections.Generic;
using System.ComponentModel;
using ClientApplication.Commands;
using ClientApplication.Models;
using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels;

public class MacroTaskViewModel : ViewModelBase
{
    private Dictionary<int, List<TaskGroup>>? _layers;
    private Dictionary<TaskGroup, TaskPoint>? _drawingPoints;
    private List<Line> _lines;


    // Properties
    public List<Line> LineList
    {
        get { return _lines; }
        set
        {
            if (_lines != value)
            {
                _lines = value;
                OnPropertyChanged(nameof(LineList));
            }
        }
    }

    public Dictionary<TaskGroup, TaskPoint> TaskPointsDictionary
    {
        get { return _drawingPoints; }
        set
        {
            if (_drawingPoints != value)
            {
                _drawingPoints = value;
                OnPropertyChanged(nameof(TaskPointsDictionary));
            }
        }
    }

    public Dictionary<int, List<TaskGroup>> TaskLayers
    {
        get { return _layers; }
        set
        {
            if (_layers != value)
            {
                _layers = value;
                OnPropertyChanged(nameof(TaskLayers));
            }
        }
    }


    public MacroTaskViewModel(INavigationService navigationService) : base(navigationService)
    {
        TaskGraphProvider.GetInstance().PropertyChanged += OnTaskGraphChanged!;
        TaskPointsDictionary = new Dictionary<TaskGroup, TaskPoint>();
        LineList = new List<Line>();
    }

    private void OnTaskGraphChanged(object? sender, PropertyChangedEventArgs e)
    {
        var tg = TaskGraphProvider.GetInstance().TaskGraph;
        if (tg != null)
        {
            SetOrUpdateTaskGraph(tg);
        }
    }

    private void SetOrUpdateTaskGraph(TaskGraph taskGraph)
    {
        Dictionary<int, List<TaskGroup>> layers = GraphUtils.GetGraphLayers(taskGraph);
        TaskGraphProvider.GetInstance().TaskGraph?.GetAvailableTaskGroups();
        TaskLayers = new Dictionary<int, List<TaskGroup>>(layers);
        _drawingPoints = new Dictionary<TaskGroup, TaskPoint>();
        _lines = new List<Line>();
        CalculateDrawingPoints();
    }

    private void CalculateDrawingPoints()
    {
        TaskPointsDictionary = PointCalculator.CalculateDrawingPoints(_layers, 200, 10, 200 / _layers.Count - 5);
        CalculateDrawingLines();
    }

    private void CalculateDrawingLines()
    {
        LineList = PointCalculator.CalculateDrawingLines(TaskPointsDictionary, 10);
    }

}