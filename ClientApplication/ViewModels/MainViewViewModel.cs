using ClientApplication.Models;
using ClientApplication.Utils;

namespace ClientApplication.ViewModels;

public class MainViewViewModel: ViewModelBase
{
    private string _test = "test";
    public string TestString
    {
        get
        {
            return _test;
        }
        set
        {
            _test = value;
            OnPropertyChanged(nameof(TestString));
        }
    }

    private bool _isUserOverViewAndTaskGraphsVisible = true;

    public bool IsUserOverViewAndTaskGraphsVisible
    {
        get => _isUserOverViewAndTaskGraphsVisible;
        set
        {
            _isUserOverViewAndTaskGraphsVisible = value;
            OnPropertyChanged(nameof(IsUserOverViewAndTaskGraphsVisible));
        }
    }
    
    public MainViewViewModel(INavigationService navigationService) : base(navigationService)
    {
        ConnectedToServerData.GetInstance().IsConnected = true;
        SocketClientService.ConnectionClosed += (_, e) =>
        {
            _test = "connection stopped";
            Logging.LogInformation(
                $"WebSocket client not connected to {SocketClientService.GetWsUri()}. Error {e}");
            ConnectedToServerData.GetInstance().IsConnected = false;
            navigationService.NavigateTo("ConnectToServerView");
        };

        WorkloadController.GetInstance().PropertyChanged += (sender, args) =>
        {
            if (WorkloadController.GetInstance().WorkloadIntensity == WorkloadIntensity.HIGH)
            {
                IsUserOverViewAndTaskGraphsVisible = false;
            }
            else
            {
                IsUserOverViewAndTaskGraphsVisible = true;
            }
            
        };
    }
}