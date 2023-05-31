using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClientApplication.Commands;
using ClientApplication.Models;
using ClientApplication.Utils;
using ClientApplication.ViewModels;
using Shared;

namespace ClientApplication.Views
{
    /// <summary>
    /// Interaktionslogik für UserOverview.xaml
    /// </summary>
    public partial class UserOverview
    {
        private UserOverviewViewModel _viewModel;
        
        public UserOverview()
        {
            InitializeComponent();
            NavigationService navigationService = NavigationService.GetInstance();
            _viewModel = new UserOverviewViewModel(navigationService);
            DataContext = _viewModel;
        }

        private void UserOverview_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Methode im ViewModel aufrufen, wenn die View geladen ist
            Logging.LogInformation("User Overview Loader aufgerufen");
            ((UserOverviewViewModel)DataContext).Initialize();
        }

        private void OnGameImageClick(object sender, int positionInActiveGamesGrid)
        {
            var circle = (sender as Image).DataContext as Circle;
            GameType? gameType = null;
            switch (positionInActiveGamesGrid)
            {
                case 1:
                    gameType = circle.FirstActiveGameType;
                    break;
                case 2:
                    gameType = circle.SecondActiveGameType;
                    break;
                case 3:
                    gameType = circle.ThirdActiveGameType;
                    break;
                case 4:
                    gameType = circle.FourthActiveGameType;
                    break;
            }

            int? taskId = null;
            foreach (var activeGame in circle.Client.ActiveGames)
            {
                if (activeGame.Value.GameType == gameType)
                {
                    taskId = activeGame.Key;
                }
            }

            if (taskId != null)
            {
                new TakeOtherUsersTaskCommand().Execute(taskId);
            }
        }

        private void Image_Top_Left_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnGameImageClick(sender, 1);
        }

        private void Image_Top_Right_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnGameImageClick(sender, 2);
        }

        private void Image_Bottom_Left_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnGameImageClick(sender, 3);
        }

        private void Image_Bottom_Right_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnGameImageClick(sender, 4);
        }
        
        // User Overview should not receive key events
        private void ListBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void ListBox_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
