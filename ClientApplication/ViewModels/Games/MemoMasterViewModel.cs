using ClientApplication.Models.GameState;
using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels.Games;

public class MemoMasterViewModel: AbstractGameViewModel<MemoMasterGameState>
{
    public MemoMasterViewModel(INavigationService navigationService) : base(navigationService,
        GameType.MemoMaster)
    {
    }

    public override void StartGame(MemoMasterGameState? state)
    {
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