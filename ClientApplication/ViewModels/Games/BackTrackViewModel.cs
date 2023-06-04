using System;
using System.Windows;
using System.Windows.Threading;
using ClientApplication.Models;
using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels.Games;

public class BackTrackViewModel: AbstractGameViewModel
{
    private string _nextNumber = "";
    public string NextNumber
    {
        get => _nextNumber;
        set
        {
            _nextNumber = value;
            OnPropertyChanged(nameof(NextNumber));
        }
    }
    private readonly Random _random = new();
    private DispatcherTimer? _gameTimer;
    private DispatcherTimer? _showNumberTimer;
    private int _timeLeft;
    private int _predictableNumber;
    public int TimeLeft
    {
        get => _timeLeft;
        set
        {
            _timeLeft = value;
            OnPropertyChanged(nameof(TimeLeft));
        }
    }
    private int _showNextNumberTimeLeft = TimeSpanToShowNumber;
    public int ShowNextNumberTimeLeft
    {
        get => _showNextNumberTimeLeft;
        set
        {
            _showNextNumberTimeLeft = value;
            OnPropertyChanged(nameof(ShowNextNumberTimeLeft));
        }
    }

    private bool _showNextNumberTimeLeftIsVisible;
    public bool ShowNextNumberTimeLeftIsVisible { 
        get => _showNextNumberTimeLeftIsVisible ;
        set
        {
            _showNextNumberTimeLeftIsVisible  = value;
            OnPropertyChanged(nameof(ShowNextNumberTimeLeftIsVisible));
        } 
    }

    private int _showNextNumberCounter;
    private const int GameDurationSeconds = 60;
    private int _correctlyRecognizedNumbers;
    private int _lives = 5;
    private int _requiredNumbersToWin = 5;
    private const int TimeSpanToShowNumber = 3;
    
    public event EventHandler<bool>? TypingEnabledEventHandler;
    public event EventHandler<int>? NumberInsertedEventHandler;
    public BackTrackViewModel(INavigationService navigationService) : base(navigationService, GameType.BackTrack)
    {
        NumberInsertedEvent();
    }

    public override void StartGame(TaskDifficulty taskDifficulty)
    {
        if (taskDifficulty == TaskDifficulty.Hard)
        {
            _lives = 3;
            _requiredNumbersToWin = 7;
        }

        Application.Current.Dispatcher.Invoke(() =>
        {
            for (int i = 0; i < _lives; i++)
            {
                Hearts.Add(new Heart { IsVisible = true });
            }
        });
        ShowNextNumber();
        Logging.LogGameEvent("BackTrack started");
        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _gameTimer.Tick += GameTimerTick;
        _showNumberTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(TimeSpanToShowNumber)
        };
        _showNumberTimer.Tick += ShowNextNumberTimerTick;
        _showNumberTimer?.Start();
        _gameTimer?.Start();
        TimeLeft = GameDurationSeconds;
        IsGameRunning = true;
    }

    public override void StopGame()
    {
        _correctlyRecognizedNumbers = 0;
        _lives = 3;
        _timeLeft = GameDurationSeconds;
        _gameTimer?.Stop();
        _showNumberTimer?.Stop();
        IsGameRunning = false;
    }
    
    private void GameTimerTick(object? sender, EventArgs e)
    {
        _timeLeft--;
        _showNextNumberTimeLeft--;
        ShowNextNumberTimeLeft = _showNextNumberTimeLeft;
        TimeLeft = _timeLeft;
        Logging.LogGameEvent($"BackTrack time left: {_timeLeft}, correctlyRecognizedNumbers: {_correctlyRecognizedNumbers}");
        CheckIfAlreadyWinOrLose();
    }

    private void ShowNextNumberTimerTick(object? sender, EventArgs e)
    {
        if (_showNextNumberCounter < 2)
        {
            ShowNextNumber();
        }
        else
        {
            _showNextNumberCounter = 0;
            NextNumber = "";
            if (_showNumberTimer != null)
            {
                _showNumberTimer.Stop();
                _showNumberTimer.Tick -= ShowNextNumberTimerTick;
            }
            TypingEnabledEventHandler?.Invoke(null, true);
            ShowNextNumberTimeLeftIsVisible = false;
        }
    }

    private void ShowNextNumber()
    {
        _showNextNumberTimeLeft = TimeSpanToShowNumber;
        TypingEnabledEventHandler?.Invoke(null, false);
        var nextNumberAsInt = _random.Next(1, 10);
        NextNumber = nextNumberAsInt.ToString();
        _showNextNumberCounter++;
        if (_showNextNumberCounter == 1) _predictableNumber = nextNumberAsInt;
        Logging.LogGameEvent($"BackTrack show next Number: {nextNumberAsInt}");
        ShowNextNumberTimeLeftIsVisible = true;
    }

    private void CheckIfAlreadyWinOrLose()
    {
        bool? win = null;
        if (TimeLeft == 0 || _lives == 0)
        {
            win = false;
        }
        else if (TimeLeft == 0 && _correctlyRecognizedNumbers >= _requiredNumbersToWin)
        {
            win = true;
        }
        else if (_correctlyRecognizedNumbers >= _requiredNumbersToWin)
        {
            win = true;
        }

        if (win != null)
        {
            RemoveActiveTask();
            MessageBox.Show((bool)win ? "Congratulations, you win!" : "Sorry, you lose.");
            Logging.LogGameEvent($"BackTrack {((bool)win ? "win" : "lose")}");
        }
    }

    private void NumberInsertedEvent()
    {
        if(NumberInsertedEventHandler == null){
            NumberInsertedEventHandler += (_, insertedNumber) =>
        {
            if (insertedNumber == _predictableNumber)
            {
                Logging.LogGameEvent($"BackTrack insert correct number: {insertedNumber}");
                _correctlyRecognizedNumbers++;
            }
            else
            {
                Logging.LogGameEvent($"BackTrack insert false number: {insertedNumber}");
                _lives--;
                Hearts.RemoveAt(_lives);
            }
            ShowNextNumber();
            if (_showNumberTimer != null)
            {
                _showNumberTimer.Tick += ShowNextNumberTimerTick;
                _showNumberTimer.Start();
            }
        };
        }
    }

    public void InvokeNumberInsertedEventHandler(int number)
    {
        NumberInsertedEventHandler?.Invoke(null, number);
    }
    
}