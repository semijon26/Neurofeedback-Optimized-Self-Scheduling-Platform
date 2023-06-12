using System;
using System.Collections.ObjectModel;
using ClientApplication.Models;
using ClientApplication.Utils;
using Shared;
using Shared.GameState;

namespace ClientApplication.ViewModels.Games;

public abstract class AbstractGameViewModel<T> : ViewModelBase
    where T : AbstractGameState 
{
    private bool _isGameRunning;
    private readonly GameType _gameType;
    public GameType GameIcon { get; private set; }
    public ObservableCollection<Heart> Hearts { get; } = new();

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

    public abstract void StartGame(TaskDifficulty taskDifficulty, T? state);

    public abstract void StopGame();

    public abstract T GetGameState();

    protected void RemoveActiveTask()
    {
        var taskId = TaskManager.GetTaskIdByGameType(_gameType);
        if (taskId == null) return;
        StopGame();
        TaskGraphProvider.SendUpdatedTaskGraphToServer(new DataPayload{SetDone = true, ChangeWorker = false, IntValue = (int)taskId, WorkerWithPulledTask = null, WorkerRemovesPulledTask = null});
        TaskManager.RemoveActiveTaskForCurrentClient((int)taskId);
        RemoveTaskFromUiEvent?.Invoke(null, _gameType);
    }
}