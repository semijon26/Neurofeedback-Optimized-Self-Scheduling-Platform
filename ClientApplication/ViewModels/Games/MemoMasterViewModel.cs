using ClientApplication.Utils;
using Shared;
using Shared.GameState;

namespace ClientApplication.ViewModels.Games;

public class MemoMasterViewModel: AbstractGameViewModel<MemoMasterGameState>
{
    public MemoMasterViewModel(INavigationService navigationService) : base(navigationService,
        GameType.MemoMaster)
    {
    }

    public override void StartGame(TaskDifficulty taskDifficulty, MemoMasterGameState? state)
    {
        if (taskDifficulty == TaskDifficulty.Hard)
        {
            
        }
        IsGameRunning = true;
    }

    public override MemoMasterGameState GetGameState()
    {
        return new MemoMasterGameState();
    }

    public override void StopGame()
    {
        IsGameRunning = false;
    }
}