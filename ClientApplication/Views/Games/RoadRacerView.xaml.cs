using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ClientApplication.Models;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;

namespace ClientApplication.Views.Games;

public partial class RoadRacerView : UserControl
{
    private readonly RoadRacerViewModel _viewModel;
    private bool _isGameUiInit = false;
    private CancellationTokenSource? _moveCircleUpCancellationTokenSource;
    private CancellationTokenSource? _moveCircleDownCancellationTokenSource;
    private Task? _moveCircleUpTask = null;
    private Task? _moveCircleDownTask = null;
    private Task? _pathAnimationTask = null;
    private List<double>? _allPathPoints;
    private double _canvasWidth;
    private double _canvasHeight;
    private readonly Stopwatch _stopwatch = new();
    private double _currentPathPointCollectionOffset;

    public RoadRacerView()
    {
        InitializeComponent();
        _viewModel = new RoadRacerViewModel(NavigationService.GetInstance());
        DataContext = _viewModel;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        _viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(_viewModel.IsGameRunning))
            {
                if (_viewModel.IsGameRunning && !_isGameUiInit)
                {
                    InitGameUi();
                } else if (!_viewModel.IsGameRunning && _isGameUiInit)
                {
                    ClearGameUi();
                }
            }
        };
        
        // in case IsGameRunning was set before the PropertyChanged listener above was initialized
        if (_viewModel.IsGameRunning)
        {
            InitGameUi();
        }
    }

    private void ClearGameUi()
    {
        _moveCircleUpCancellationTokenSource?.Cancel();
        _moveCircleDownCancellationTokenSource?.Cancel();
        _allPathPoints = new List<double>();
        _stopwatch.Stop();
        _currentPathPointCollectionOffset = 0;
        _isGameUiInit = false;
    }

    private void InitGameUi()
    {
        _isGameUiInit = true;
        var parentWindow = Window.GetWindow(this);
        if (parentWindow != null)
        {
            // PreviewKeyDown on parent window ensures that the event will always arrive here
            parentWindow.PreviewKeyDown += ParentWindow_KeyDown;
            parentWindow.PreviewKeyUp += ParentWindow_KeyUp;
        }

        _canvasWidth = Canvas.ActualWidth;
        _canvasHeight = Canvas.ActualHeight;
        StartPathAnimation();
    }

    private async void StartPathAnimation()
    {
        _allPathPoints = GetCompleteRandomPathCurve();
        _pathAnimationTask = PathAnimationLoop();
        _stopwatch.Start();
        await _pathAnimationTask;
    }

    private async Task PathAnimationLoop()
    {
        while (_viewModel.IsGameRunning)
        {
            // measure time since last tick to calc how many pixels the path must be moved
            var sinceLastTickElapsedMillis = _stopwatch.Elapsed.TotalMilliseconds;
            _stopwatch.Restart();

            // ensures that game speed is independent from calculation duration
            var pixelsToMoveThisTick = sinceLastTickElapsedMillis / 50 * _viewModel.PixelsPer50Millis;
            _currentPathPointCollectionOffset += pixelsToMoveThisTick;

            var pathPointYValuesForThisTick = _allPathPoints
                .Skip((int)_currentPathPointCollectionOffset)
                .Take((int)_canvasWidth).ToList();

            var pathPointsForThisTick = new PointCollection();
            for (var i = 0; i < pathPointYValuesForThisTick.Count; i++)
            {
                var yValue = pathPointYValuesForThisTick[i];
                pathPointsForThisTick.Add(new Point(x: i, yValue));
            }

            PathPolylineForeground.Points = pathPointsForThisTick;
            PathPolylineBackground.Points = pathPointsForThisTick;
            UpdateIsCircleOnPath(pathPointsForThisTick);

            await Task.Delay(10);
        }
    }

    private void UpdateIsCircleOnPath(PointCollection pathPoints)
    {
        var circleTop = Canvas.GetTop(Circle);
        var circleBottom = circleTop + Circle.ActualHeight;

        var circleHorizontalMiddle = Canvas.GetLeft(Circle) + Circle.ActualWidth / 2;
        var rangeToTake = (int)Circle.ActualHeight / 2;
        var relevantPathPointRange = pathPoints
            .Skip((int)circleHorizontalMiddle - rangeToTake / 2)
            .Take(rangeToTake)
            .ToList();

        var isCircleOnPath = false;

        foreach (var point in relevantPathPointRange)
        {
            if (point.Y >= circleTop && point.Y <= circleBottom)
            {
                isCircleOnPath = true;
                break;
            }
        }

        var snapshot = new CircleAndPathSnapshot(_currentPathPointCollectionOffset, isCircleOnPath);
        _viewModel.UpdateCurrentMeters(snapshot);
    }

    private List<double> GetCompleteRandomPathCurve()
    {
        var random = new Random();
        var requiredPointsCount = (int)_canvasWidth;
        requiredPointsCount += RoadRacerViewModel.GameDurationSeconds * _viewModel.PixelsPer50Millis * 100;
        var requiredNumbersToInterpolateCount = requiredPointsCount / 160;
        var numbers = new List<int>();
        for (var i = 0; i < requiredNumbersToInterpolateCount; i++)
        {
            var randomNumber = random.NextDouble() * _canvasHeight;
            randomNumber = Math.Clamp(randomNumber, Circle.ActualHeight * 1.5,
                _canvasHeight - Circle.ActualHeight * 1.5);
            numbers.Add((int)randomNumber);
        }

        return InterpolationUtils.Interpolate(numbers, requiredPointsCount);
    }

    private void ParentWindow_KeyDown(object sender, KeyEventArgs e)
    {
        Logging.LogInformation("Key down event accepted");
        if (e.Key == Key.Up && _moveCircleUpTask == null)
        {
            _moveCircleUpCancellationTokenSource = new CancellationTokenSource();
            var ct = _moveCircleUpCancellationTokenSource.Token;
            _moveCircleUpTask = Task.Factory.StartNew(() => { MoveCircleLoop(ct, true); }, ct);
        }
        else if (e.Key == Key.Down && _moveCircleDownTask == null)
        {
            _moveCircleDownCancellationTokenSource = new CancellationTokenSource();
            var ct = _moveCircleDownCancellationTokenSource.Token;
            _moveCircleDownTask = Task.Factory.StartNew(() => { MoveCircleLoop(ct, false); }, ct);
        }
    }

    private void MoveCircleLoop(CancellationToken ct, bool moveUp)
    {
        var moveCircleIntervalMillis = 15;
        var moveCirclePixelsPerInterval = _viewModel.PixelsPer50Millis * 0.5;

        while (!ct.IsCancellationRequested && _viewModel.IsGameRunning)
        {
            Dispatcher.Invoke(() =>
            {
                double circleNewTop = Canvas.GetTop(Circle);
                if (moveUp)
                {
                    circleNewTop -= moveCirclePixelsPerInterval;
                }
                else
                {
                    circleNewTop += moveCirclePixelsPerInterval;
                }

                var circleNewTopClamped = Math.Clamp(circleNewTop, 0,
                    _canvasHeight - Circle.ActualHeight);
                Canvas.SetTop(Circle, circleNewTopClamped);
            });
            Thread.Sleep(moveCircleIntervalMillis);
        }
    }

    private void ParentWindow_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Up)
        {
            _moveCircleUpCancellationTokenSource?.Cancel();
            _moveCircleUpTask = null;
        }
        else if (e.Key == Key.Down)
        {
            _moveCircleDownCancellationTokenSource?.Cancel();
            _moveCircleDownTask = null;
        }
    }
}