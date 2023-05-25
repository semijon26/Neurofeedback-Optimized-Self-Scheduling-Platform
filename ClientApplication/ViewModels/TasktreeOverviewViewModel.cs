using System;
using System.Collections.Generic;
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
            RelayCommand = new RelayCommand(this);
        }
        
        private void SetOrUpdateTaskGraph(TaskGraph taskGraph)
        {
            Dictionary<int, List<TaskGroup>> layers = GraphUtils.GetGraphLayers(taskGraph);
            GraphUtils.LogCalculatedLayers(layers);
            TaskGraphProvider.GetInstance().TaskGraph.GetAvailableTaskGroups();
            TaskLayers = new Dictionary<int, List<TaskGroup>>(layers);
            _drawingPoints = new Dictionary<TaskGroup, TaskPoint>();
            _lines = new List<Line>();
            CalculateDrawingPoints();
        }
        
        private void OnTaskGraphChanged(object sender, PropertyChangedEventArgs e)
        {
            var tg = TaskGraphProvider.GetInstance().TaskGraph;
            if (tg != null)
            {
                SetOrUpdateTaskGraph(tg);
            }
        }

        
        //Commmands
        public RelayCommand RelayCommand { get; set; }
        

        //Variables and Objects
        private string _title = "Tasktree";
        private string _object = "";
        private string _message = "";
        private Dictionary<int, List<TaskGroup>> _layers;
        private Dictionary<TaskGroup, TaskPoint> _drawingPoints;
        private List<Line> _lines;

        
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
            Dictionary<TaskGroup, TaskPoint> current_ = new Dictionary<TaskGroup, TaskPoint>();
            int CanvasWidth = 1200;
            int initialYValue = 0;
            int initialWidth = 70;
            foreach (int key in _layers.Keys)
            {
                int circleDistance;
                int counter = 1;
                // Where(TaskGraphProvider.GetInstance().TaskGraph.GetAvailableTaskGroups().Contains)
                foreach (TaskGroup group in _layers[key])
                {
                    int NumberOfNodes = _layers[key].Count;
                    int NumberOfTasks = group.Tasks.Count;
                    circleDistance = CanvasWidth / (NumberOfNodes+1);
                    int step = circleDistance;

                    Logging.LogInformation($"Circle Distance = {step}");
                    
                    int y_value = initialYValue;
                    int x_value =  counter * step;
                    TaskPoint point = new TaskPoint { X = x_value, Y = y_value, Width = NumberOfTasks*initialWidth};
                    current_[group] = point;
                    Logging.LogInformation($"X = {x_value}; Y={y_value}");
                    counter++;
                }
                initialYValue += 90;
            }
            TaskPointsDictionary = current_;
            CalculateDrawingLines();
        }
        
        // ID muss unique sein --> keine Dopplung, sonst fehlerhafte Zuweisung
        private TaskGroup getTaskGroup(int Id)
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

        private void CalculateDrawingLines()
        {
            int height = 70;
            Dictionary<int, List<int>> _lineHelper = new Dictionary<int, List<int>>();
            List<Line> _current = new List<Line>();
            foreach (var group in TaskPointsDictionary.Keys)
            {
                _lineHelper.Add(group.Id, new List<int>());
                
                //.Where(TaskPointsDictionary.ContainsKey)
                foreach (var destNode in TaskGraphProvider.GetInstance().TaskGraph.GetAdjacentGroupsOf(group.Id))
                {
                    try
                    {
                        // Daher jetzt erst Kacke
                        int x1 = TaskPointsDictionary[getTaskGroup(group.Id)].X;
                        int y1 = TaskPointsDictionary[getTaskGroup(group.Id)].Y;
                        int width1 = TaskPointsDictionary[getTaskGroup(group.Id)].Width;
                        int x2 = TaskPointsDictionary[getTaskGroup(destNode.Id)].X;
                        int y2 = TaskPointsDictionary[getTaskGroup(destNode.Id)].Y;
                        int width2 = TaskPointsDictionary[getTaskGroup(destNode.Id)].Width;
                        _current.Add(new Line {X1 = x1+width1/2, X2 = x2+width2/2, Y1 = y1+height/2, Y2 = y2+height/2, SourceGroup = group, DestGroup = destNode});
                        
                        Logging.LogWarning($"Die Kante von ({x1}:{y1})({TaskPointsDictionary[getTaskGroup(group.Id)].X}:{TaskPointsDictionary[getTaskGroup(group.Id)].Y}) und ({x2}:{y2})({TaskPointsDictionary[getTaskGroup(destNode.Id)].X}:{TaskPointsDictionary[getTaskGroup(destNode.Id)].Y}) wurde erstellt.");
                    }
                    catch (Exception e)
                    {
                        Logging.LogError("Thrown in Line Calculation: " + e.Message);
                    }
                    
                }
            }
            LineList = _current;
        }
    }
}
