using System;
using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels.Games;

public abstract class AbstractGameViewModel : ViewModelBase
{
    private bool _isGameRunning;
    private readonly GameType _gameType;
    public GameType GameIcon { get; private set; }

    protected AbstractGameViewModel(INavigationService navigationService, GameType gameType) : base(navigationService)
    {
        _gameType = gameType;
        GameIcon = gameType;
    }
    public event EventHandler<GameType>? RemoveTaskFromUiEvent;
    public bool IsGameRunning
    {
        get => _isGameRunning;
        set
        {
            _isGameRunning = value;
            OnPropertyChanged(nameof(IsGameRunning));
        } }

    public abstract void StartGame();

    public abstract void StopGame();

    protected void RemoveActiveTask()
    {
        var taskId = TaskManager.GetTaskIdByGameType(_gameType);
        if (taskId == null) return;
        StopGame();
        TaskGraphProvider.SendUpdatedTaskGraphToServer(new DataPayload{SetDone = true, ChangeWorker = false, IntValue = (int)taskId, Woker = null});
        TaskManager.RemoveActiveTaskForCurrentClient((int)taskId);
        RemoveTaskFromUiEvent?.Invoke(null, _gameType);
    }
}