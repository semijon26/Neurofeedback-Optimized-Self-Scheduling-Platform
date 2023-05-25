using System.Linq;
using System.Windows.Controls;
using ClientApplication.Utils;
using ClientApplication.ViewModels;
using Shared;

namespace ClientApplication.Views
{
    /// <summary>
    /// Interaktionslogik für GamesOverview.xaml
    /// </summary>
    public partial class GamesOverview
    {

        public GamesOverview()
        {
            InitializeComponent();
            GamesOverviewViewModel viewModel = new GamesOverviewViewModel(NavigationService.GetInstance());
            DataContext = viewModel;
            AddTaskToUi(viewModel);
            RemoveTaskFromUi(viewModel, GameGrid);
        }

        private void AddTaskToUi(GamesOverviewViewModel viewModel)
        {
            viewModel.AddTaskToUiEvent += (_, gameType) =>
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (var keyValuePair in ClientObject.GetInstance().ActiveGames.Where(keyValuePair => keyValuePair.Value.GameType == gameType))
                    {
                        AddRightGameViewToUi(viewModel, gameType, keyValuePair.Value.Row, keyValuePair.Value.Column);
                    }
                });
            };
        }

        private void AddRightGameViewToUi(GamesOverviewViewModel viewModel,GameType gameType, int row, int column)
        {
            Grid.SetRow(viewModel.GameDictionary[gameType], row);
            Grid.SetColumn(viewModel.GameDictionary[gameType], column);
            GameGrid.Children.Add(viewModel.GameDictionary[gameType]);
        }

        private void RemoveTaskFromUi(GamesOverviewViewModel viewModel, Grid gameGrid)
        {
            viewModel.RemoveTaskFromUiEvent += (_, gameType) =>
            {
                foreach (var keyValuePair in ClientObject.GetInstance().ActiveGames.Where(keyValuePair => keyValuePair.Value.GameType == gameType))
                {
                    ClientObject.GetInstance().ActiveGames.Remove(keyValuePair.Key);
                }

                Dispatcher.Invoke(() => { gameGrid.Children.Remove(viewModel.GameDictionary[gameType]); });
            };
        }
    }
}
