using System;
using System.Collections.Generic;
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
                    var taskId = activeGame.Key;
                    switch (activeGame.Value.GameType)
                    {
                        case GameType.TextGame:
                            if (!textGameViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.TextGame);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    textGameViewModel.StartGame(GetTaskDifficulty(taskId));
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
                                    bricketBreakerViewModel.StartGame(GetTaskDifficulty(taskId));
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
                                    pathPilotViewModel.StartGame(GetTaskDifficulty(taskId));
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
                                    memoMasterViewModel.StartGame(GetTaskDifficulty(taskId));
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
                                    backTrackViewModel.StartGame(GetTaskDifficulty(taskId));
                                });
                                backTrackViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }   
                            break;
                    }
                }
                
            };
        }

        private TaskDifficulty GetTaskDifficulty(int taskId)
        {
            return TaskGraphProvider.GetInstance().TaskGraph.GetTaskById(taskId).Difficulty;
        }
    }
}
