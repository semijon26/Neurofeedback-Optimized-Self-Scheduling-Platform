using System;
using System.Windows;
using ClientApplication.Models;
using ClientApplication.Utils;
using ClientApplication.ViewModels;
using Shared;

namespace ClientApplication.Commands;

public class ChangeCommand : CommandBase
{
    private readonly TasktreeOverviewViewModel _viewModel;

    public ChangeCommand(TasktreeOverviewViewModel viewModel)
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

                        // Hier die Bedingung, wann Task abgenommen werden kann, einfügen.
                        if (!currentInstance.ActiveGames.ContainsKey(taskId))
                        {
                            // Hier den Code ausführen
                            //MessageBox.Show("Task sollte übernommen werden.");
                            TaskManager.UpdatePulledTaskByServer(availTask.Id);
                        }
                        else
                        {
                            MessageBox.Show("Du kannst das Spiel nicht selbst abnehmen.");
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