using ClientApplication.Utils;
using ClientApplication.ViewModels;

namespace ClientApplication.Views;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();
        NavigationService navigationService = Utils.NavigationService.GetInstance();
        MainViewViewModel viewModel = new MainViewViewModel(navigationService);
        DataContext = viewModel;
    }
}