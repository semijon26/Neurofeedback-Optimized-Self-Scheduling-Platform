using System.Windows.Controls;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;

namespace ClientApplication.Views.Games;

public partial class BackTrackView : UserControl
{
    public BackTrackView()
    {
        InitializeComponent();
        InitializeComponent();
        var backTrackViewModel = new BackTrackViewModel(NavigationService.GetInstance());
        DataContext = backTrackViewModel;
    }
}