using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels.Games;

public class MemoMasterViewModel: AbstractGameViewModel
{
    public MemoMasterViewModel(INavigationService navigationService) : base(navigationService,
        GameType.MemoMaster)
    {
    }

    public override void StartGame()
    {
        IsGameRunning = true;
    }

    public override void StopGame()
    {
        IsGameRunning = false;
    }
}