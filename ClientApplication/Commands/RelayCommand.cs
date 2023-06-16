using System;
using System.Windows;
using ClientApplication.Models;
using ClientApplication.Utils;
using ClientApplication.ViewModels;
using Shared;

namespace ClientApplication.Commands;

public class RelayCommand : CommandBase
{
    private readonly TasktreeOverviewViewModel _viewModel;

    public RelayCommand(TasktreeOverviewViewModel viewModel)
    {
        // übergebenes ViewModel zuweisen
        _viewModel = viewModel;
    }

    public override void Execute(object parameter)
    {
        var taskId = Convert.ToInt32(parameter);
        // Graphschichten laden
        var layers = _viewModel.TaskLayers;
        
        // jeweilige Task anhand der ID im Graphen suchen
        // um damit weiter zu arbeiten
        foreach (var group in layers)
        {
            foreach (var taskGroup in group.Value)
            {
                foreach (var availTask in taskGroup.Tasks)
                {
                    if (availTask.Id == taskId)
                    {
                        // wenn Task gefunden: 
                        var currentInstance = GetCurrentInstance();
                        // Überprüfen, ob Spieler schon 4 Aufgaben hat
                        if (currentInstance.ActiveGames.Count is >= 0 and < 4)
                        {
                            // checken, ob Task bereits aktiv ist
                            if (TaskManager.IsGameCurrentlyActive(availTask.Id, availTask.GameType))
                            {
                                MessageBox.Show("Game already aktiv!");
                                continue;
                            }
                            // checken, ob Task bereits beendet ist
                            if (availTask.IsDone)
                            {
                                MessageBox.Show("Game already done!");
                                continue;
                            }
                            
                            // Task als aktiv für den aktuellen Client festlegen
                            currentInstance.AddNewActiveGame(availTask.Id, availTask.GameType, null);
                            TaskGraphProvider.GetInstance().SendUpdatedTaskGraphToServer(new DataPayload{SetDone = false, ChangeWorker = false, IntValue = availTask.Id, WorkerWithPulledTask = currentInstance, WorkerRemovesPulledTask = null});
                            Logging.LogGameEvent($"{availTask.GameType} is active now");
                            ClientManagementSocket.SendClientObjectWhenConnectionEstablished();
                        }
                        else
                        {
                            MessageBox.Show("Maximum number of selectable games reached!");
                        }
                    }
                }
            }
        }
    }

    private static ClientObject GetCurrentInstance()
    {
        var currentClient = ClientManagementData.GetInstance(ClientObject.GetInstance()).CurrentClient;
        return currentClient;
    }
    
}