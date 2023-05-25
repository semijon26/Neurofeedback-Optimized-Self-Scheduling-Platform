using System.Windows.Controls;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;

namespace ClientApplication.Views.Games;

public partial class MemoMasterView : UserControl
{
    public MemoMasterView()
    {
        InitializeComponent();
        MemoMasterViewModel memoMasterViewModel = new MemoMasterViewModel(NavigationService.GetInstance());
        DataContext = memoMasterViewModel;
    }
}