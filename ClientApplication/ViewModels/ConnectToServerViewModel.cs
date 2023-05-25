using System.Windows.Input;
using ClientApplication.Commands;
using ClientApplication.TestConfigs;
using ClientApplication.Utils;

namespace ClientApplication.ViewModels
{
    public class ConnectToServerViewModel : ViewModelBase
    {
        public ConnectToServerViewModel(INavigationService navigationService) : base(navigationService)
        {
            _serverIp = DebugMode.GetDebugModeIpAddress();
            if(DebugMode.GetDebugModePort() != -1)
            {
                ServerPort = DebugMode.GetDebugModePort();
                _portAsString = ServerPort.ToString();
            }
            _userName = DebugMode.GetDebugModeTestName();
            _isConnectedMessage = null;
            SubmitCommand = new SubmitCommand(this);
            NavigationService = navigationService;
        }
        private string? _serverIp;
        public string? ServerIp
        {
            get => _serverIp;
            set
            {
                _serverIp = value;
                OnPropertyChanged(nameof(ServerIp));
            }
        }
        public int ServerPort;
        private string? _portAsString;

        public string? ServerPortAsString
        {
            get => _portAsString;
            set
            {
                _portAsString = value;
                int.TryParse(_portAsString, out ServerPort);
                OnPropertyChanged(nameof(ServerPort));
            }
        }
        private string? _userName;
        public string? UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        private string? _isConnectedMessage;
        public string? IsConnectedMessage
        {
            get => _isConnectedMessage;
            set
            {
                _isConnectedMessage = value;
                OnPropertyChanged(nameof(IsConnectedMessage));
            }
        }

        public ICommand SubmitCommand { get; }
    }
}
