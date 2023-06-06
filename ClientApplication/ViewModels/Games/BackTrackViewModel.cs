using System;
using System.Collections.Generic;
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
    private readonly List<int> _predictableNumbers = new();
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
    private int _showNextFieldCounter;
    private const int GameDurationSeconds = 60;
    private int _correctlyRecognizedNumbers;
    private int _lives = 5;
    private int _requiredNumbersToWin = 5;
    private const int TimeSpanToShowNumber = 3;
    private const int NBacks = 10;
    private const int MaxNumberToShow = NBacks*2-2;
    private bool _isNumberInserted = true;
    
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
        _showNextFieldCounter = 0;
        _correctlyRecognizedNumbers = 0;
        _lives = 5;
        _isNumberInserted = true;
        _timeLeft = GameDurationSeconds;
        _gameTimer?.Stop();
        _showNumberTimer?.Stop();
        NextNumber = "";
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
        if (!_isNumberInserted)
        {
            NumberInsertedEventHandler?.Invoke(null, -1);
        }
        if (_showNextFieldCounter % 2 == 1 && _showNextFieldCounter < MaxNumberToShow)
        {
            ShowNextNumber();
        }
        else
        {
            _showNextNumberTimeLeft = TimeSpanToShowNumber;
            _showNextFieldCounter++;
            NextNumber = "";
            _isNumberInserted = false;
            TypingEnabledEventHandler?.Invoke(null, true);
        }
    }

    private void ShowNextNumber()
    {
        NextNumber = "";
        _showNextNumberTimeLeft = TimeSpanToShowNumber;
        TypingEnabledEventHandler?.Invoke(null, false);
        var nextNumberAsInt = _random.Next(1, 10);
        NextNumber = nextNumberAsInt.ToString();
        _showNextFieldCounter++;
        _predictableNumbers.Add(nextNumberAsInt);
        Logging.LogGameEvent($"BackTrack show next Number: {nextNumberAsInt}");
    }

    private void CheckIfAlreadyWinOrLose()
    {
        bool? win = null;
        if (TimeLeft == 0 || _lives == -1)
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
                _isNumberInserted = true;
                var predictableNumber = _predictableNumbers[^2];
                if (_showNextFieldCounter == NBacks * 2)
                {
                    predictableNumber = _predictableNumbers[^1];
                }
                if (insertedNumber == predictableNumber)
                {
                    Logging.LogGameEvent($"BackTrack insert correct number: {insertedNumber}");
                    _correctlyRecognizedNumbers++;
                }
                else
                {
                    Logging.LogGameEvent($"BackTrack insert false number: {insertedNumber}");
                    _lives--;
                    if(_lives >= 0) Hearts.RemoveAt(_lives);
                }
            };
        }
    }

    public void InvokeNumberInsertedEventHandler(int number)
    {
        NumberInsertedEventHandler?.Invoke(null, number);
    }
    
}