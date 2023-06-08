using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClientApplication.Utils;
using ClientApplication.ViewModels.Games;
using ClientApplication.Views.Games;
using Shared;
using Shared.GameState;

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

            var clientObject = ClientObject.GetInstance();

            ClientManagementSocket.OnStartGamesMessageReceived += (_, _) =>
            {
                foreach (var activeGame in clientObject.ActiveGames)
                {
                    var taskDifficulty = GetTaskDifficulty(activeGame.Key) ?? TaskDifficulty.Easy;
                    switch (activeGame.Value.GameType)
                    {
                        case GameType.TextGame:
                            if (!textGameViewModel.IsGameRunning)
                            {
                                if (activeGame.Value.GameStateHolder.TextGameGameState != null)
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                        textGameViewModel.StartGame(taskDifficulty,
                                            activeGame.Value.GameStateHolder.TextGameGameState));
                                }
                                AddTaskToUiEvent?.Invoke(null, GameType.TextGame);
                                if (!textGameViewModel.IsGameRunning) Application.Current.Dispatcher.Invoke(() => textGameViewModel.StartGame(taskDifficulty, null));
                                textGameViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }
                            break;
                        case GameType.BricketBraker:
                            if (!bricketBreakerViewModel.IsGameRunning)
                            {
                                if (activeGame.Value.GameStateHolder.BricketBreakerGameState != null)
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                        bricketBreakerViewModel.StartGame(taskDifficulty,
                                            activeGame.Value.GameStateHolder.BricketBreakerGameState));
                                }
                                AddTaskToUiEvent?.Invoke(null, GameType.BricketBraker);
                                if (!bricketBreakerViewModel.IsGameRunning) Application.Current.Dispatcher.Invoke(() => bricketBreakerViewModel.StartGame(taskDifficulty,null));
                                bricketBreakerViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.PathPilot:
                            if (!pathPilotViewModel.IsGameRunning)
                            {
                                if (activeGame.Value.GameStateHolder.PathPilotGameState != null)
                                {
                                    Application.Current.Dispatcher.Invoke(() => pathPilotViewModel.StartGame(taskDifficulty, activeGame.Value.GameStateHolder.PathPilotGameState));
                                }
                                AddTaskToUiEvent?.Invoke(null, GameType.PathPilot);
                                if (!pathPilotViewModel.IsGameRunning) Application.Current.Dispatcher.Invoke(() => pathPilotViewModel.StartGame(taskDifficulty, null));
                                pathPilotViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.MemoMaster:
                            if (!memoMasterViewModel.IsGameRunning)
                            {
                                if (activeGame.Value.GameStateHolder.MemoMasterGameState != null)
                                {
                                    Application.Current.Dispatcher.Invoke(() => memoMasterViewModel.StartGame(taskDifficulty, activeGame.Value.GameStateHolder.MemoMasterGameState));
                                }
                                AddTaskToUiEvent?.Invoke(null, GameType.MemoMaster);
                                if (!memoMasterViewModel.IsGameRunning) Application.Current.Dispatcher.Invoke(() => memoMasterViewModel.StartGame(taskDifficulty, null));
                                memoMasterViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.BackTrack:
                            if (!backTrackViewModel.IsGameRunning)
                            {
                                if (activeGame.Value.GameStateHolder.BackTrackGameState != null)
                                {
                                    Application.Current.Dispatcher.Invoke(() => backTrackViewModel.StartGame(taskDifficulty, activeGame.Value.GameStateHolder.BackTrackGameState));
                                }
                                AddTaskToUiEvent?.Invoke(null, GameType.BackTrack);
                                if (!backTrackViewModel.IsGameRunning) Application.Current.Dispatcher.Invoke(() => backTrackViewModel.StartGame(taskDifficulty, null));
                                backTrackViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }
                            break;
                    }
                    ResetAllGameStateToNull(activeGame.Value.GameStateHolder);
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

            GameStateSocket.GameStateMessageReceived += (_, clientWhoPulledTask) =>
            {
                if (clientWhoPulledTask.PulledTaskId != null && clientObject.ActiveGames.ContainsKey((int)clientWhoPulledTask.PulledTaskId))
                {
                    var pulledGame = clientObject.ActiveGames[(int)clientWhoPulledTask.PulledTaskId];
                    switch (pulledGame.GameType)
                    {
                        case GameType.BackTrack:
                            pulledGame.GameStateHolder.BackTrackGameState = backTrackViewModel.GetGameState();
                            break;
                        case GameType.BricketBraker:
                            pulledGame.GameStateHolder.BricketBreakerGameState = bricketBreakerViewModel.GetGameState();
                            break;
                        case GameType.MemoMaster:
                            pulledGame.GameStateHolder.MemoMasterGameState = memoMasterViewModel.GetGameState();
                            break;
                        case GameType.PathPilot:
                            pulledGame.GameStateHolder.PathPilotGameState = pathPilotViewModel.GetGameState();
                            break;
                        case GameType.TextGame:
                            pulledGame.GameStateHolder.TextGameGameState = textGameViewModel.GetGameState();
                            break;
                    }

                    TaskGraphProvider.GetInstance().SendUpdatedTaskGraphToServer(new DataPayload
                    {
                        SetDone = false, ChangeWorker = true, IntValue = (int)clientWhoPulledTask.PulledTaskId!,
                        WorkerWithPulledTask = clientWhoPulledTask, WorkerRemovesPulledTask = clientObject
                    });
                    clientWhoPulledTask.PulledTaskId = null;
                }
            };
        }

        private TaskDifficulty? GetTaskDifficulty(int taskId)
        {
            return TaskGraphProvider.GetInstance().TaskGraph?.GetTaskById(taskId)?.Difficulty;
        }

        private void ResetAllGameStateToNull(GameStateHolder gameStateHolder)
        {
            gameStateHolder.BackTrackGameState = null;
            gameStateHolder.TextGameGameState = null;
            gameStateHolder.BackTrackGameState = null;
            gameStateHolder.MemoMasterGameState = null;
            gameStateHolder.PathPilotGameState = null;
        }
    }
}
