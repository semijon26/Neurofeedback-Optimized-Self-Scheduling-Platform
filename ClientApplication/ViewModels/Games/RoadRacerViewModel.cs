using System;
using System.Windows;
using System.Windows.Threading;
using ClientApplication.Models;
using ClientApplication.Utils;
using Shared;
using Shared.GameState;

namespace ClientApplication.ViewModels.Games;

public sealed class RoadRacerViewModel : AbstractGameViewModel<RoadRacerGameState>
{
    public int GameDurationSeconds = 60;
    private int _requiredMetersToWinInstantly = 1000;
    private int _requiredMetersToNotLose;
    private double _currentMeters = 0;
    private int _currentMetersFloored = 0;
    private int _timeLeft;
    private DispatcherTimer? _timer = null;
    private CircleOnPathDetection _circleOnPathDetection = new();
    // change how fast meters count
    private const int PixelToMeterFactor = 6;
    // use this constant to change speed of game
    public readonly int PixelsPer50Millis = 6;

    public RoadRacerViewModel(INavigationService navigationService) : base(navigationService, GameType.RoadRacer)
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

    public override void StartGame(TaskDifficulty taskDifficulty, RoadRacerGameState? state)
    {
        if (taskDifficulty == TaskDifficulty.Easy)
        {
            _requiredMetersToNotLose = (int)(_requiredMetersToWinInstantly * 0.5);
        }
        else
        {
            _requiredMetersToNotLose = (int)(_requiredMetersToWinInstantly * 0.7);
        }
        Logging.LogGameEvent("RoadRacer started");
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer?.Start();
        TimeLeft = GameDurationSeconds;
        IsGameRunning = true;
    }

    public override RoadRacerGameState GetGameState()
    {
        return new RoadRacerGameState();
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
        Logging.LogGameEvent($"RoadRacer time left: {_timeLeft}, currentMeters: {_currentMeters}");
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
            Logging.LogGameEvent($"RoadRacer {((bool)win ? "win" : "lose")}");
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