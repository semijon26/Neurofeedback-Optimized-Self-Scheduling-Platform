using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ClientApplication.Commands;
using ClientApplication.Models;
using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels
{
    public class TasktreeOverviewViewModel : ViewModelBase
    {
        // Constructor
        public TasktreeOverviewViewModel(INavigationService navigationService) : base(navigationService)
        {
            TaskGraphProvider.GetInstance().PropertyChanged += OnTaskGraphChanged!;
            SocketClientService.OnMessageObjectReceived = OnMessageObjectReceived;
            SocketClientService.OnMessageStringReceived = OnMessageStringReceived;
            TaskPointsDictionary = new Dictionary<TaskGroup, TaskPoint>();
            LineList = new List<Line>();
            MacroTaskPointDictionary = new Dictionary<TaskGroup, TaskPoint>();
            MacroLineList = new List<Line>();
            RelayCommand = new RelayCommand(this);
            ChangeCommand = new ChangeCommand(this);
        }
        
        private void SetOrUpdateTaskGraph(TaskGraph taskGraph)
        {
            Dictionary<int, List<TaskGroup>> layers = GraphUtils.GetGraphLayers(taskGraph);
            TaskGraphProvider.GetInstance().TaskGraph.GetAvailableTaskGroups();
            TaskLayers = new Dictionary<int, List<TaskGroup>>(layers);
            _drawingPoints = new Dictionary<TaskGroup, TaskPoint>();
            _lines = new List<Line>();
            _macroDrawingPoints = new Dictionary<TaskGroup, TaskPoint>();
            _macroLines = new List<Line>();
            CalculateDrawingPoints();
        }
        
        private void OnTaskGraphChanged(object sender, PropertyChangedEventArgs e)
        {
            var tg = TaskGraphProvider.GetInstance().TaskGraph;
            if (tg != null)
            {
                SetOrUpdateTaskGraph(tg);
                CanvasHeight = GetMaxGraphHeight();
            }
        }

        //Commmands
        public RelayCommand RelayCommand { get; set; }
        public ChangeCommand ChangeCommand { get; set; }
        

        //Variables and Objects
        private string _title = "Tasktree";
        private string _object = "";
        private string _message = "";
        private Dictionary<int, List<TaskGroup>> _layers;
        private Dictionary<TaskGroup, TaskPoint> _drawingPoints;
        private List<Line> _lines;
        private bool _initial;

        //Variables for MacroView
        private Dictionary<TaskGroup, TaskPoint> _macroDrawingPoints;
        private List<Line> _macroLines;
        
        // Rectangle Canvas Height
        private double canvasHeight;
        
        // View const 
        private const double FirstColumnWidth = 0.2 * 1480;
        private const double SecondColumnWidth = 0.8 * 1480;
        private const double RowHeight = 180;
        private const double MacroGroupHeight = 30;
        private const double GroupHeight = 100;
        

        // Properties
        public List<Line> LineList
        {
            get
            {
                return _lines;
            }
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
            get
            {
                return _drawingPoints;
            }
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

        public string StringMessage
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(StringMessage));
            }
        }

        public string ObjectMessage
        {
            get
            {
                return _object;
            }
            set
            {
                _object = value;
                OnPropertyChanged(nameof(ObjectMessage));
            }
        }
        
        public string Title { 
            get 
            { 
                return _title; 
            }
            set 
            {
                _title = value;
            }
        }
        
        // MacroView Properties
        public List<Line> MacroLineList
        {
            get
            {
                return _macroLines;
            }
            set
            {
                if (_macroLines != value)
                {
                    _macroLines = value;
                    OnPropertyChanged(nameof(MacroLineList));
                }
            }
        }

        public Dictionary<TaskGroup, TaskPoint> MacroTaskPointDictionary
        {
            get { return _macroDrawingPoints; }
            set
            {
                if (_macroDrawingPoints != value)
                {
                    _macroDrawingPoints = value;
                    OnPropertyChanged(nameof(MacroTaskPointDictionary));
                }
            }
        }

        public double CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                if (canvasHeight != value)
                {
                    canvasHeight = value;
                    OnPropertyChanged(nameof(CanvasHeight));
                }
            }
        }

        // METHODS
        private void OnMessageStringReceived(string obj)
        {
            StringMessage = obj;
        }

        private void OnMessageObjectReceived(string obj)
        {
            ObjectMessage = obj;
        }

        private void CalculateDrawingPoints()
        {
            TaskPointsDictionary = PointCalculator.CalculateDrawingPoints(_layers, SecondColumnWidth, 40, GroupHeight);
            MacroTaskPointDictionary = PointCalculator.CalculateDrawingPoints(_layers, FirstColumnWidth, 12, MacroGroupHeight);
            CalculateDrawingLines();
        }

        private void CalculateDrawingLines()
        {
            LineList = PointCalculator.CalculateDrawingLines(TaskPointsDictionary, 70);
            MacroLineList = PointCalculator.CalculateDrawingLines(MacroTaskPointDictionary, 10);
        }
        
        private double GetMaxGraphHeight()
        {
            // Höhe des Macrographen berechnen
            double maxGraphHeight = 0;
            foreach (var taskPointPair in MacroTaskPointDictionary)
            {
                var taskPoint = taskPointPair.Value;
                double taskPointHeight = taskPoint.Y + 10;
                if (taskPointHeight > maxGraphHeight)
                {
                    maxGraphHeight = taskPointHeight;
                }
            }
            return maxGraphHeight;
        }
    }
}
