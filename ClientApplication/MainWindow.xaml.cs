using System.Windows;
using System.Windows.Input;
using ClientApplication.Utils;
using ClientApplication.Views;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationService navigationService = NavigationService.GetInstance();
            // Register the views that will be used for navigation
            navigationService.RegisterView("ConnectToServerView", typeof(ConnectToServerView));
            navigationService.RegisterView("MainView", typeof(MainView));

            Logging.LogInformation("MainWindow initialized");
            this.PreviewKeyDown += KeyDownEventMethod;
        }

        private void KeyDownEventMethod(object sender, KeyEventArgs e)
        {
            Logging.LogKeyEvent(e.Key);
        }
    }
}
