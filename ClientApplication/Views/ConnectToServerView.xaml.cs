using System.Windows.Input;
using ClientApplication.Models;
using ClientApplication.ViewModels;
using NavigationService = ClientApplication.Utils.NavigationService;

namespace ClientApplication.Views
{
    /// <summary>
    /// Interaction logic for ConnectToServerView.xaml
    /// </summary>
    public partial class ConnectToServerView
    {
        public ConnectToServerView()
        {
            InitializeComponent();
            var viewModel = new ConnectToServerViewModel(NavigationService.GetInstance());
            DataContext = viewModel;
            if (ConnectedToServerData.GetInstance().IsConnected == false)
            {
                viewModel.IsConnectedMessage =  "Not connected. Please check your input and try again.";
            }
        }

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = (ConnectToServerViewModel)DataContext;
                viewModel.SubmitCommand.Execute(viewModel);   
            }
        }
    }
}
