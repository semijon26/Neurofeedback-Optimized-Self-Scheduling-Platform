using System.Windows;
using ClientApplication.Utils;
using ClientApplication.ViewModels;

namespace ClientApplication.Views
{
    /// <summary>
    /// Interaktionslogik für UserOverview.xaml
    /// </summary>
    public partial class UserOverview
    {
        public UserOverview()
        {
            InitializeComponent();
            NavigationService navigationService = NavigationService.GetInstance();
            UserOverviewViewModel viewModel = new UserOverviewViewModel(navigationService);
            DataContext = viewModel;
        }

        private void UserOverview_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Methode im ViewModel aufrufen, wenn die View geladen ist
            Logging.LogInformation("User Overview Loader aufgerufen");
            ((UserOverviewViewModel)DataContext).Initialize();
        }
    }
}
