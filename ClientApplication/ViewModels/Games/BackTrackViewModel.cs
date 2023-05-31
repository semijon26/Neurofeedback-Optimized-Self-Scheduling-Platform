using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels.Games;

public class BackTrackViewModel: AbstractGameViewModel
{
    public BackTrackViewModel(INavigationService navigationService) : base(navigationService, GameType.BackTrack)
    {
      
    }

    public override void StartGame()
    {
        throw new System.NotImplementedException();
    }

    public override void StopGame()
    {
        throw new System.NotImplementedException();
    }
}