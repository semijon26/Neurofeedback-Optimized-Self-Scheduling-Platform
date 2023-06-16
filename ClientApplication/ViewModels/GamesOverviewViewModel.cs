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
            { GameType.RoadRacer, new RoadRacerView() },
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
            var roadRacerViewModel = (RoadRacerViewModel)GameDictionary[GameType.RoadRacer].DataContext;
            var memoMasterViewModel = (MemoMasterViewModel)GameDictionary[GameType.MemoMaster].DataContext;
            var backTrackViewModel = (BackTrackViewModel)GameDictionary[GameType.BackTrack].DataContext;

            var clientObject = ClientObject.GetInstance();

            ClientManagementSocket.OnStartGamesMessageReceived += (_, _) =>
            {
                foreach (var activeGame in clientObject.ActiveGames)
                {
                    var taskDifficulty = GetTaskDifficulty(activeGame.Key) ?? TaskDifficulty.Easy;
                    var gameStateHolder = activeGame.Value.GameStateHolder;

                    switch (activeGame.Value.GameType)
                    {
                        case GameType.TextGame:
                            if (!textGameViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.TextGame);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    textGameViewModel.StartGame(taskDifficulty, gameStateHolder.TextGameGameState);
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
                                    bricketBreakerViewModel.StartGame(taskDifficulty,
                                        gameStateHolder.BricketBreakerGameState);
                                });
                                bricketBreakerViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.RoadRacer:
                            if (!roadRacerViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.RoadRacer);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    roadRacerViewModel.StartGame(taskDifficulty,
                                        gameStateHolder.RoadRacerGameState);
                                });
                                roadRacerViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.MemoMaster:
                            if (!memoMasterViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.MemoMaster);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    memoMasterViewModel.StartGame(taskDifficulty,
                                        gameStateHolder.MemoMasterGameState);
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
                                    backTrackViewModel.StartGame(taskDifficulty,
                                        gameStateHolder.BackTrackGameState);
                                });

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
                {
                    GameType.BricketBraker, GameType.RoadRacer, GameType.MemoMaster, GameType.TextGame,
                    GameType.BackTrack
                };

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

                        if (activeGame == GameType.RoadRacer)
                        {
                            Logging.LogInformation("Stopping ------------------------------------- RoadRacer");
                            roadRacerViewModel.StopGame();
                            RemoveTaskFromUiEvent?.Invoke(null, activeGame);
                            roadRacerViewModel.RemoveTaskFromUiEvent -= RemoveTaskFromUiEvent;
                        }

                        if (activeGame == GameType.BackTrack)
                        {
                            Logging.LogInformation("Stopping ------------------------------------- BackTrack");
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
                if (clientWhoPulledTask.PulledTaskId != null &&
                    clientObject.ActiveGames.ContainsKey((int)clientWhoPulledTask.PulledTaskId))
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
                        case GameType.RoadRacer:
                            pulledGame.GameStateHolder.RoadRacerGameState = roadRacerViewModel.GetGameState();
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
            gameStateHolder.RoadRacerGameState = null;
        }
    }
}