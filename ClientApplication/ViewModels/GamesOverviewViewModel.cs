using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;
using ClientApplication.Views.Games;
using Shared;

namespace ClientApplication.ViewModels
{
    public class GamesOverviewViewModel : ViewModelBase
    {
        public readonly Dictionary<GameType, UserControl> GameDictionary = new()
        {
            { GameType.TextGame, new TextGameView() },
            { GameType.BricketBraker, new BricketBreakerGame() },
            { GameType.PathPilot, new PathPilotView() },
            { GameType.MemoMaster, new MemoMasterView() },
            { GameType.BackTrack, new BackTrackView() }
        };

        protected readonly Dictionary<GameType, AbstractGameViewModel> gameViewModels = new()
        {
            { GameType.TextGame, new TextGameViewModel(Utils.NavigationService.GetInstance()) },
            { GameType.BricketBraker, new BricketBreakerViewModel(Utils.NavigationService.GetInstance()) },
            { GameType.PathPilot, new PathPilotViewModel(Utils.NavigationService.GetInstance()) },
            { GameType.MemoMaster, new MemoMasterViewModel(Utils.NavigationService.GetInstance()) },
            { GameType.BackTrack, new BackTrackViewModel(Utils.NavigationService.GetInstance()) }
        };

        public List<KeyValuePair<GameType, AbstractGameViewModel>> ActiveGameViewModels
        {
            get { return gameViewModels.Where(vm => vm.Value.IsGameRunning).ToList(); }
        }

        public event EventHandler<GameType>? AddTaskToUiEvent;
        public event EventHandler<GameType>? RemoveTaskFromUiEvent;

        public GamesOverviewViewModel(INavigationService navigationService) : base(
            navigationService)
        {
            var textGameViewModel = (TextGameViewModel)GameDictionary[GameType.TextGame].DataContext;
            var bricketBreakerViewModel = (BricketBreakerViewModel)GameDictionary[GameType.BricketBraker].DataContext;
            var pathPilotViewModel = (PathPilotViewModel)GameDictionary[GameType.PathPilot].DataContext;
            var memoMasterViewModel = (MemoMasterViewModel)GameDictionary[GameType.MemoMaster].DataContext;
            var backTrackViewModel = (BackTrackViewModel)GameDictionary[GameType.BackTrack].DataContext;


            ClientManagementSocket.OnStartGamesMessageReceived += (_, _) =>
            {
                foreach (var activeGame in ClientObject.GetInstance().ActiveGames)
                {
                    var taskDifficulty = GetTaskDifficulty(activeGame.Key) ?? TaskDifficulty.Easy;
                    switch (activeGame.Value.GameType)
                    {
                        case GameType.TextGame:
                            if (!textGameViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.TextGame);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    textGameViewModel.StartGame(taskDifficulty);
                                });
                                textGameViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.BricketBraker:
                            if (!bricketBreakerViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.BricketBraker);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    bricketBreakerViewModel.StartGame(taskDifficulty);
                                });
                                bricketBreakerViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.PathPilot:
                            if (!pathPilotViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.PathPilot);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    pathPilotViewModel.StartGame(taskDifficulty);
                                });
                                pathPilotViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.MemoMaster:
                            if (!memoMasterViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.MemoMaster);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    memoMasterViewModel.StartGame(taskDifficulty);
                                });
                                memoMasterViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.BackTrack:
                            if (!backTrackViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.BackTrack);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    backTrackViewModel.StartGame(taskDifficulty);
                                });
                                backTrackViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }
                            break;
                    }
                }

            };

            ClientManagementSocket.MessageReceived += (_, _) =>
            {
                Logging.LogInformation("Message Received Event ausgeführt");
                var clientGames = ClientObject.GetInstance().ActiveGames.Values.Select(game => game.GameType).ToList();
                List<GameType> types = new List<GameType>
                    { GameType.BricketBraker, GameType.PathPilot, GameType.MemoMaster, GameType.TextGame, GameType.BackTrack};

                Logging.LogWarning("clientGames:");
                foreach (var gameType in clientGames)
                {
                    Logging.LogWarning($"{gameType}");
                }


                foreach (var activeGame in types)
                {
                    if (!clientGames.Contains(activeGame))
                    {
                        Logging.LogInformation($"{activeGame} not active - delete");
                        if (activeGame == GameType.TextGame)
                        {
                            textGameViewModel.StopGame();
                            RemoveTaskFromUiEvent?.Invoke(null, activeGame);
                            textGameViewModel.RemoveTaskFromUiEvent -= RemoveTaskFromUiEvent;
                        }

                        if (activeGame == GameType.BricketBraker)
                        {
                            bricketBreakerViewModel.StopGame();
                            RemoveTaskFromUiEvent?.Invoke(null, activeGame);
                            bricketBreakerViewModel.RemoveTaskFromUiEvent -= RemoveTaskFromUiEvent;
                        }

                        if (activeGame == GameType.MemoMaster)
                        {
                            memoMasterViewModel.StopGame();
                            RemoveTaskFromUiEvent?.Invoke(null, activeGame);
                            memoMasterViewModel.RemoveTaskFromUiEvent -= RemoveTaskFromUiEvent;
                        }

                        if (activeGame == GameType.PathPilot)
                        {
                            Logging.LogInformation("Stopping ------------------------------------- Path Pilot");
                            pathPilotViewModel.StopGame();
                            RemoveTaskFromUiEvent?.Invoke(null, activeGame);
                            pathPilotViewModel.RemoveTaskFromUiEvent -= RemoveTaskFromUiEvent;
                        }
                        if (activeGame == GameType.BackTrack)
                        {
                            Logging.LogInformation("Stopping ------------------------------------- Back Track");
                            backTrackViewModel.StopGame();
                            RemoveTaskFromUiEvent?.Invoke(null, activeGame);
                            backTrackViewModel.RemoveTaskFromUiEvent -= RemoveTaskFromUiEvent;
                        }
                    }
                    else
                    {
                        Logging.LogInformation($"{activeGame} active...");
                    }
                }
            };
        }

        private TaskDifficulty? GetTaskDifficulty(int taskId)
        {
            return TaskGraphProvider.GetInstance().TaskGraph?.GetTaskById(taskId)?.Difficulty;
        }
    }
}
