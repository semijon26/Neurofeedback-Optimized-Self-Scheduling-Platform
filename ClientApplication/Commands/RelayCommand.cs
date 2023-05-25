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
        _viewModel = viewModel;
    }

    public override void Execute(object parameter)
    {
        var taskId = Convert.ToInt32(parameter);

        var task = _viewModel.TaskLayers;
        
        foreach (var group in task)
        {
            foreach (var taskGroup in group.Value)
            {
                foreach (var availTask in taskGroup.Tasks)
                {
                    if (availTask.Id == taskId)
                    {
                        var currentInstance = GetCurrentInstance();
                        if (currentInstance.ActiveGames.Count is >= 0 and < 4)
                        {
                            if (TaskManager.IsGameCurrentlyActive(availTask.Id, availTask.GameType))
                            {
                                MessageBox.Show("Game already aktiv!");
                                continue;
                            }

                            if (availTask.IsDone)
                            {
                                MessageBox.Show("Game already done!");
                                continue;
                            }

                            currentInstance.AddNewActiveGame(availTask.Id, availTask.GameType);
                            //availTask.ChangeWoker(currentInstance);
                            TaskGraphProvider.GetInstance().SendUpdatedTaskGraphToServer(new DataPayload{SetDone = false, ChangeWorker = false, IntValue = availTask.Id, Woker = currentInstance});
                            Logging.LogGameEvent($"{currentInstance.Label} added {availTask.GameType} to active games");
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