using System.Collections.ObjectModel;
using System.Windows;
using ClientApplication.Models;
using ClientApplication.Utils;
using Shared;

namespace ClientApplication.ViewModels
{
    public class UserOverviewViewModel : ViewModelBase
    {
        private string _title = "User";
        private string _currentUser = "Hello";
        private ObservableCollection<Circle> _circles = new();

        public UserOverviewViewModel(INavigationService navigationService) : base(navigationService)
        {
            OnClientDataReceived();
            Initialize();
        }

        public ObservableCollection<Circle> Circles
        {
            get => _circles;
            set
            {
                if (_circles != value)
                {
                    _circles = value;
                    OnPropertyChanged(nameof(Circles));
                }
            }
        }
        public string UserMessage
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(UserMessage));
            }
        }
        public string Title 
        { 
            get => _title;
            set 
            { 
                _title = value; 
            }
        }

        private void OnClientDataReceived()
        {
            ClientManagementSocket.MessageReceived += (_, _) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var clientManagementData = ClientManagementData.GetInstance(ClientObject.GetInstance());
                    AddNewClientCircle(clientManagementData);
                });
            };
        }

        private void AddNewClientCircle(ClientManagementData clientManagementData)
        {
            Circles = new ObservableCollection<Circle>();
            var circles = new ObservableCollection<Circle>();
            Circle circle = new Circle { Label = clientManagementData.CurrentClient.Label, ButtonMargin = new Thickness(0,0,0,45), Client = clientManagementData.CurrentClient};
            foreach (var activeGame in clientManagementData.CurrentClient.ActiveGames)
            {
                SetGameIconToEmptyImage(circle, activeGame.Value);
            }
            circles.Add(circle);
            foreach (var client in clientManagementData.OtherClients)
            {
                circle = new Circle { Label = client.Label, ButtonMargin = new Thickness(0,0,0,0), Client = client};
                foreach (var activeGame in client.ActiveGames)
                {
                    SetGameIconToEmptyImage(circle, activeGame.Value);
                }
                circles.Add(circle);
            }
            Circles = circles;
        }

        public void Initialize()
        {
            UserManagement.ReceiveUsers();
        }

        private void SetGameIconToEmptyImage(Circle circle, GameObject activeGame)
        {
            if (activeGame is { Row: 0, Column: 0 })
            {
                circle.FirstActiveGameType = activeGame.GameType;
            }else if (activeGame is { Row: 0, Column: 1 })
            {
                circle.SecondActiveGameType = activeGame.GameType;
            }else if (activeGame is { Row: 1, Column: 0 })
            {
                circle.ThirdActiveGameType = activeGame.GameType;
            }else if (activeGame is { Row: 1, Column: 1 })
            {
                circle.FourthActiveGameType = activeGame.GameType;
            }
        }
    }
}
