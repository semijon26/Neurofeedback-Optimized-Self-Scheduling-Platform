using System;
using System.Windows;
using System.Windows.Threading;
using ClientApplication.Models;
using ClientApplication.Models.GameState;
using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels.Games;

public sealed class PathPilotViewModel : AbstractGameViewModel<PathPilotGameState>
{
    public int GameDurationSeconds = 20;
    private int _requiredMetersToWinInstantly = 50;
    private int _requiredMetersToNotLose = 30;
    private double _currentMeters = 0;
    private int _currentMetersFloored = 0;
    private const int PixelToMeterFactor = 20;
    private int _timeLeft;
    private DispatcherTimer? _timer = null;
    private CircleOnPathDetection _circleOnPathDetection = new();

    // use this constant to change speed of game
    public readonly int PixelsPer50Millis = 7;

    public PathPilotViewModel(INavigationService navigationService) : base(navigationService, GameType.PathPilot)
    {
    }

    public int TimeLeft
    {
        get => _timeLeft;
        set
        {
            _timeLeft = value;
            OnPropertyChanged(nameof(TimeLeft));
        }
    }

    public double CurrentMeters
    {
        get => _currentMeters;
        set
        {
            _currentMeters = value;
            OnPropertyChanged(nameof(CurrentMeters));
        }
    }

    public int CurrentMetersFloored
    {
        get => _currentMetersFloored;
        set
        {
            _currentMetersFloored = value;
            OnPropertyChanged(nameof(CurrentMetersFloored));
        }
    }

    public override void StartGame(TaskDifficulty taskDifficulty, PathPilotGameState? state)
    {
        if (taskDifficulty == TaskDifficulty.Hard)
        {
            
        }
        Logging.LogGameEvent("PathPilot started");
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer?.Start();
        TimeLeft = GameDurationSeconds;
        IsGameRunning = true;
    }

    public override PathPilotGameState GetGameState()
    {
        return new PathPilotGameState();
    }

    public override void StopGame()
    {
        IsGameRunning = false;
        _currentMeters = 0;
        _currentMetersFloored = 0;
        _timeLeft = GameDurationSeconds;
        _timer?.Stop();
        _circleOnPathDetection = new();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _timeLeft--;
        TimeLeft = _timeLeft;
        Logging.LogGameEvent($"PathPilot time left: {_timeLeft}, currentMeters: {_currentMeters}");
        CheckIfAlreadyWinOrLose();
    }

    private void CheckIfAlreadyWinOrLose()
    {
        bool? win = null;
        if (TimeLeft == 0 && CurrentMeters < _requiredMetersToNotLose)
        {
            win = false;
        }
        else if (TimeLeft == 0 && CurrentMeters >= _requiredMetersToNotLose)
        {
            win = true;
        }
        else if (CurrentMeters >= _requiredMetersToWinInstantly)
        {
            win = true;
        }

        if (win != null)
        {
            RemoveActiveTask();
            MessageBox.Show((bool)win ? "Congratulations, you win!" : "Sorry, you lose.");
            Logging.LogGameEvent($"PathPilot {((bool)win ? "win" : "lose")}");
        }
    }

    public void UpdateCurrentMeters(CircleAndPathSnapshot snapshot)
    {
        _circleOnPathDetection.UpdateCurrentSnapshot(snapshot);
        var distanceSinceLastPointMeters =
            _circleOnPathDetection.GetDistanceSinceLastPointPixels() / PixelToMeterFactor;
        CurrentMeters += distanceSinceLastPointMeters;
        CurrentMetersFloored = (int)CurrentMeters;
        CheckIfAlreadyWinOrLose();
    }
}