using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using ClientApplication.Utils;
using Shared;
using Shared.GameState;

namespace ClientApplication.ViewModels.Games;

public sealed class TextGameViewModel : AbstractGameViewModel<TextGameGameState>
{
    private string _inputText;
    private int _errorCount = 0;
    private string _inputWord;
    private readonly string _targetWord;
    private bool _isWordFullyWritten = false;

    // Declare text to randomly choose from
    List<string> texts = new()
    {
        "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus? Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus! Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. ",
        "Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue! Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante.Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. ",
        "Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla! Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Nam pretium turpis et arcu? Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Sed aliquam ultrices mauris. Integer ante arcu, accumsan a, consectetuer eget, posuere ut, mauris. Praesent adipiscing. Phasellus ullamcorper ipsum rutrum nunc. Nunc nonummy metus. Vestibulum volutpat pretium libero. Cras id dui. Aenean ut eros et nisl sagittis vestibulum. ",
        "Nullam nulla eros, ultricies sit amet, nonummy id, imperdiet feugiat, pede. Sed lectus. Donec mollis hendrerit risus. Phasellus nec sem in justo pellentesque facilisis. Etiam imperdiet imperdiet orci. Nunc nec neque. Phasellus leo dolor, tempus non, auctor et, hendrerit quis, nisi. Curabitur ligula sapien, tincidunt non, euismod vitae, posuere imperdiet, leo. Maecenas malesuada. Praesent congue erat at massa. Sed cursus turpis vitae tortor. Donec posuere vulputate arcu. Phasellus accumsan cursus velit. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed aliquam, nisi quis porttitor congue, elit erat euismod orci, ac placerat dolor lectus quis orci. ",
        "Phasellus consectetuer vestibulum elit. Aenean tellus metus, bibendum sed, posuere ac, mattis non, nunc. Vestibulum fringilla pede sit amet augue. In turpis. Pellentesque posuere. Praesent turpis. Aenean posuere, tortor sed cursus feugiat, nunc augue blandit nunc, eu sollicitudin urna dolor sagittis lacus. Donec elit libero, sodales nec, volutpat a, suscipit non, turpis? Nullam sagittis. Suspendisse pulvinar, augue ac venenatis condimentum, sem libero volutpat nibh, nec pellentesque velit pede quis nunc! Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Fusce id purus. Ut varius tincidunt libero. Phasellus dolor. Maecenas vestibulum mollis diam. "
    };

    private string _targetText;

    private int fullWordsWritten = 0;
    private int _difficulty;

    private bool _isCorrect;
    private int timeLeft;
    private int inputTextLength = 0;
    private DispatcherTimer timer;


    public TextGameViewModel(INavigationService navigationService) : base(navigationService, GameType.TextGame)
    {
        // Get a random text from the list at the beginning of the game
        _targetText = GetRandomText(); 
    }

    public override void StartGame(TaskDifficulty taskDifficulty, TextGameGameState? state)
    {
        if (taskDifficulty == TaskDifficulty.Hard)
        {
            _difficulty = 30;
        }
        else
        {
            _difficulty = 15;
        }

        if (state != null)
        {
            TimeLeft = state.TimeLeft;
            ErrorCount = state.ErrorCount;
            TargetText = state.TargetText;
            FullWordsWritten = state.FullWordsWritten;
        }

        //var currentClientPlaying = GetClientInstanceLogging();
        Logging.LogGameEvent("TextGame started");
        // Create a new DispatcherTimer with a 10-second interval
        timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        // Subscribe to the Tick event
        timer.Tick += Timer_Tick;

        Logging.LogGameEvent("Timer starts: 60 seconds");
        // Start the timer
        timer.Start();

        // Set the time left to 60 seconds
        TimeLeft = 60;
        IsGameRunning = true;
    }

    public override TextGameGameState GetGameState()
    {
        // Get the current state of the game
        return new TextGameGameState(
            timeLeft: TimeLeft,
            errorCount: ErrorCount,
            targetText: TargetText,
            fullWordsWritten: FullWordsWritten
            ); 
    }

    public override void StopGame()
    {
        // Reset game values for the next game
        IsGameRunning = false;
        InputText = "";
        ErrorCount = 0;
        TargetText = GetRandomText();
        FullWordsWritten = 0;
    }

    public int TimeLeft
    {
        get { return timeLeft; }
        set
        {
            timeLeft = value;
            Logging.LogGameEvent($"TextGame time left: {timeLeft}");
            OnPropertyChanged(nameof(TimeLeft));
        }
    }

    public string InputText // Text which the user has written
    {
        get { return _inputText; }
        set
        {
            if (_inputText != value)
            {
                _inputText = value;
                Logging.LogUserInteraction($"TextGame user input text: [{InputText}]");
                OnPropertyChanged(nameof(InputText));
                CheckText();
                GetFullWordsWritten(InputText);
            }
        }
    }

    public string TargetText // Text which the user has to write
    {
        get { return _targetText; }
        set
        {
            if (_targetText != value)
            {
                _targetText = value;
                OnPropertyChanged(nameof(TargetText));
                CheckText();
            }
        }
    }

    public bool IsCorrect // Is the user input correct
    {
        get { return _isCorrect; }
        set
        {
            if (_isCorrect != value)
            {
                _isCorrect = value;
                OnPropertyChanged(nameof(IsCorrect));
            }
        }
    }

    public int ErrorCount
    {
        get { return _errorCount; }
        set
        {
            if (_errorCount != value)
            {
                _errorCount = value;
                OnPropertyChanged(nameof(ErrorCount));
            }
        }
    }

    public int FullWordsWritten // Number of full words written by the user
    {
        get { return fullWordsWritten; }
        set
        {
            if (fullWordsWritten != value)
            {
                fullWordsWritten = value;
                OnPropertyChanged(nameof(FullWordsWritten));
            }
        }
    }

    public string InputWord // Word which the user has written
    {
        get { return _inputWord; }
        set
        {
            if (_inputWord != value)
            {
                _inputWord = value;
                _inputText += _inputWord;
                OnPropertyChanged(nameof(InputWord));
            }
        }
    }

    public string TargetWord // Word which the user has to write
    {
        get { return _targetWord; }
    }

    public bool IsWordFullyWritten // Is the word fully written by the user
    {
        get { return _isWordFullyWritten; }
    }

    private void CheckText() // Check if the user input is correct
    {
        // If nothing is written in the input field
        if (InputText == null || InputText.Length == 0)
        {
            IsCorrect = false;
            inputTextLength = 0;
            return;
        }

        int length = Math.Min(InputText.Length, TargetText.Length);
        for (int i = 0; i < length; i++)
        {
            if (InputText[i] == '\b')
            {
                inputTextLength--;
                return;
            }

            if (InputText[i] != TargetText[i])
            {
                // Check if the user did a backspace to delete an error character
                if (length < inputTextLength)
                {
                    IsCorrect = false;
                    Logging.LogGameEvent("User input is false");
                    inputTextLength--;
                    Logging.LogGameEvent($"Text after character deletion: [{InputText}]");
                    return;
                }

                if (IsWordFullyWritten)
                {
                    return;
                }

                IsCorrect = false;
                inputTextLength++;

                // Add error if the user input is not a backspace
                ErrorCount++;
                IsErrorCountBelowThree();
                Logging.LogUserInteraction($"Wrong character input. New error count: {ErrorCount}");
                return;
            }
        }

        if (length < inputTextLength)
            Logging.LogGameEvent($"Text after character deletion: [{InputText}]");

        inputTextLength = length;
        IsCorrect = true;
        Logging.LogGameEvent("User input is correct");
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        // Decrement the time left
        timeLeft--;
        TimeLeft = timeLeft;

        var fullText = IsTextFullyWritten();
        if (TimeLeft != 0 && fullText)
        {
            timer.Stop();
            Logging.LogGameEvent($"Text game won - Error Count: {ErrorCount}");
            RemoveActiveTask();
            MessageBox.Show("Congratulations, you win!");
        }

        // Check if the timer has run out
        if (TimeLeft == 0)
        {
            // Stop the timer
            timer.Stop();

            // Check if the text is fully written
            if (IsTextFullyWritten() || fullWordsWritten >= _difficulty)
            {
                // The game is won
                Logging.LogGameEvent($"Text game won - Error Count: {ErrorCount}");
                RemoveActiveTask();
                MessageBox.Show("Congratulations, you win!");
            }
            else
            {
                // The game is lost
                Logging.LogGameEvent($"Text game lost - Error Count: {ErrorCount}");
                RemoveActiveTask();
                MessageBox.Show("Sorry, you lose.");
            }
        }
    }

    private bool IsTextFullyWritten()
    {
        // Check if the user has written the whole text
        if (InputText == TargetText)
        {
            Logging.LogGameEvent($"Input Text: [{InputText}] == Target Text: [{TargetText}]");
            return true;
        }

        return false;
    }

    private void IsErrorCountBelowThree()
    {
        // Check if the error count is below 3 otherwise the game is lost and gets removed
        if (_errorCount > 3)
        {
            timer.Stop();
            RemoveActiveTask();
            MessageBox.Show("More than 3 Errors. You Loose!");
        }
    }

    private string GetRandomText()
    {
        // Gets a random text from the text list
        Random random = new();
        var randomText = texts[random.Next(0, texts.Count)];
        return randomText;
    }

    private int GetFullWordsWritten(string inputText)
    {
        // Check if the user has written a full word
        _isWordFullyWritten = false;

        // Check if the input field is empty
        if (string.IsNullOrEmpty(inputText))
        {
            return 0;
        }
        
        // Split the text into single words
        List<string> targetWords = new(TargetText.Split(new[] { ' ' }));

        // Get the first word of the list of words
        string targetWord = targetWords[0];

        // Check if the input text is equal to the target word
        if (inputText == targetWord + " ")
        {
            // Increment the number of full words written
            FullWordsWritten++;

            _isWordFullyWritten = true;

            // Remove the written word from the list of words from the target text
            targetWords.Remove(targetWord);

            // Join the list of words back to a full string
            var newTargetText = String.Join(" ", targetWords);

            // Set the new target text without the written word
            TargetText = newTargetText;

            FullWordsWritten = fullWordsWritten;
            Logging.LogGameEvent($"FULL WORDS WRITTEN: {fullWordsWritten}");
        }
        else
        {
            // If word is not fully written, set the flag to false
            _isWordFullyWritten = false;
        }

        return fullWordsWritten;
    }
}