using System;
using ClientApplication.Models;
using ClientApplication.Utils;
using ClientApplication.ViewModels;

namespace ClientApplication.Commands
{
    public class SubmitCommand : CommandBase
    {
        private readonly ConnectToServerViewModel _viewModel;
        public SubmitCommand(ConnectToServerViewModel connectToServerViewModel)
        {
            _viewModel = connectToServerViewModel;
        }

        public override void Execute(object parameter)
        {
            SocketClientService.ConnectionClosed += (_, e) =>
            {
                _viewModel.IsConnectedMessage = "connection stopped. Please Check the Input and try again.";
                Logging.LogInformation(
                    $"WebSocket client not connected to {SocketClientService.GetWsUri()}. Error {e}");
                ConnectedToServerData.GetInstance().IsConnected = false;
            };
            SocketClientService.ConnectionEstablished += (_, _) =>
            {
                Logging.LogInformation($"WebSocket client connected to {SocketClientService.GetWsUri()}");
                _viewModel.IsConnectedMessage = "Connected";
                _viewModel.NavigationService.NavigateTo("MainView");
            };
            if (_viewModel is { ServerIp: not null, UserName: not null }) SocketClientService.Connect(_viewModel.ServerIp, _viewModel.ServerPort, _viewModel.UserName).WaitAsync(TimeSpan.FromMilliseconds(1000));
        }
    }
}
