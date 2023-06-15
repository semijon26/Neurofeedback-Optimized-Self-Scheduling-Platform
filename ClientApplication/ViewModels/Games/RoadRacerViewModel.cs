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
    public const int GameDurationSeconds = 60;
    // Maximum: Hier wird das Game gestoppt, wenn man die Meter-Anzahl erreicht
    private const int RequiredMetersToWinInstantly = 1000;
    // Wenn man darunter ist und das Spiel zu ende ist, hat man verloren
    private int _requiredMetersToNotLose;
    // Aktuelle Meter
    private double _currentMeters = 0;
    private int _currentMetersFloored = 0;
    private int _timeLeft = GameDurationSeconds;
    // Timer für den Countdown
    private DispatcherTimer? _timer = null;

    private CircleOnPathDetection _circleOnPathDetection = new();

    // Hier kann man einstellen, wie schnell die Meter hochzählen
    private const int PixelToMeterFactor = 6;

    // Hier kann man einstellen, wie schnell sich der Pfad bewegt
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
            _requiredMetersToNotLose = (int)(RequiredMetersToWinInstantly * 0.5);
        }
        else
        {
            _requiredMetersToNotLose = (int)(RequiredMetersToWinInstantly * 0.7);
        }

        if (state != null)
        {
            CurrentMeters = state.CurrentMeters;
            CurrentMetersFloored = state.CurrentMetersFloored;
            TimeLeft = state.TimeLeft;
        }
        else
        {
            TimeLeft = GameDurationSeconds;
        }

        Logging.LogGameEvent("RoadRacer started");
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer?.Start();
        IsGameRunning = true;
    }

    public override RoadRacerGameState GetGameState()
    {
        return new RoadRacerGameState(
            currentMeters: CurrentMeters,
            currentMetersFloored: CurrentMetersFloored,
            timeLeft: TimeLeft
        );
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
        // Jede Sekunde wird überprüft, ob die Meter erreicht wurden oder der Timer abgelaufen ist
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
        else if (CurrentMeters >= RequiredMetersToWinInstantly)
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