using System.Windows.Controls;
using ClientApplication.Utils;
using ClientApplication.ViewModels;

namespace ClientApplication.Views;

public partial class MacroTaskView : UserControl
{
    public MacroTaskView()
    {
        InitializeComponent();
        NavigationService navigationService = Utils.NavigationService.GetInstance();
        MacroTaskViewModel viewModel = new MacroTaskViewModel(navigationService);
        DataContext = viewModel;
    }
}