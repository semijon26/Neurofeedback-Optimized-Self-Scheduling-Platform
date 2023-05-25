using System.Windows;
using System.Windows.Input;
using ClientApplication.Utils;
using ClientApplication.ViewModels;

namespace ClientApplication.Views
{
    /// <summary>
    /// Interaktionslogik für TasktreeOverview.xaml
    /// </summary>
    public partial class TasktreeOverview
    {
        public TasktreeOverview()
        {
            InitializeComponent();
            NavigationService navigationService = Utils.NavigationService.GetInstance();
            TasktreeOverviewViewModel viewModel = new TasktreeOverviewViewModel(navigationService);
            DataContext = viewModel;
        }
        
        private void MyView_Loaded(object sender, RoutedEventArgs e)
        {
            // Methode im ViewModel aufrufen, wenn die View geladen ist
            Logging.LogInformation("Loader aufgerufen");
            //(((TasktreeOverviewViewModel)DataContext).Initialize();
        }

        private void SubmitMessage(object sender, RoutedEventArgs e)
        {
            SocketClientService.Send(MessageTextBox.Text);
        }

        // Tasktree should not receive key events
        private void ScrollViewer_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void ScrollViewer_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
