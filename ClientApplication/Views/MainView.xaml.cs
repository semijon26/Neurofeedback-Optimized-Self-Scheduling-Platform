using System.ComponentModel;
using System.Windows;
using ClientApplication.Utils;
using ClientApplication.ViewModels;

namespace ClientApplication.Views;

public partial class MainView
{
    private MainViewViewModel viewModel;
    
    public MainView()
    {
        InitializeComponent();
        NavigationService navigationService = Utils.NavigationService.GetInstance();
        viewModel = new MainViewViewModel(navigationService);
        DataContext = viewModel;

        InitWorkloadViewVisibility();
    }

    private void InitWorkloadViewVisibility()
    {
        viewModel.PropertyChanged += MainViewViewModelOnPropertyChanged;
    }

    private void MainViewViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(viewModel.IsUserOverViewAndTaskGraphsVisible))
        {
            var visibility = viewModel.IsUserOverViewAndTaskGraphsVisible ? Visibility.Hidden : Visibility.Visible;

            TaskTreeOverviewCover.Visibility = visibility;
            UserOverviewCover.Visibility = visibility;
        }
    }
}