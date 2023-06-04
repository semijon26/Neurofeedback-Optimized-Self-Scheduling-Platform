using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels.Games;

public class BackTrackViewModel: AbstractGameViewModel
{
    private string _nextNumber = "6";
    public string NextNumber
    {
        get => _nextNumber;
        set
        {
            _nextNumber = value;
            OnPropertyChanged(nameof(NextNumber));
        }
    }
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