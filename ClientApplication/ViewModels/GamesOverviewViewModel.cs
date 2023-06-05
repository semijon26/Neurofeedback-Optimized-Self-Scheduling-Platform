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
            { GameType.MemoMaster, new MemoMasterView() }
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
            
            ClientManagementSocket.OnStartGamesMessageReceived += (_, _) =>
            {
                
                foreach (var activeGame in ClientObject.GetInstance().ActiveGames)
                {
                    switch (activeGame.Value.GameType)
                    {
                        case GameType.TextGame:
                            if (!textGameViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.TextGame);
                                Application.Current.Dispatcher.Invoke(() => textGameViewModel.StartGame(null));
                                textGameViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.BricketBraker:
                            if (!bricketBreakerViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.BricketBraker);
                                Application.Current.Dispatcher.Invoke(() => bricketBreakerViewModel.StartGame(null));
                                bricketBreakerViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.PathPilot:
                            if (!pathPilotViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.PathPilot);
                                Application.Current.Dispatcher.Invoke(() => pathPilotViewModel.StartGame(null));
                                pathPilotViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
                            }

                            break;
                        case GameType.MemoMaster:
                            if (!memoMasterViewModel.IsGameRunning)
                            {
                                AddTaskToUiEvent?.Invoke(null, GameType.MemoMaster);
                                Application.Current.Dispatcher.Invoke(() => memoMasterViewModel.StartGame(null));
                                memoMasterViewModel.RemoveTaskFromUiEvent += RemoveTaskFromUiEvent;
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
                    { GameType.BricketBraker, GameType.PathPilot, GameType.MemoMaster, GameType.TextGame };
                
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
                    }
                    else
                    {
                        Logging.LogInformation($"{activeGame} active...");
                    }
                }
            };
        }
    }
}
