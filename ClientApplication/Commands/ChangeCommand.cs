using System;
using System.Windows;
using ClientApplication.Models;
using ClientApplication.Utils;
using ClientApplication.ViewModels;
using Shared;

namespace ClientApplication.Commands;

public class ChangeCommand : CommandBase
{
    // variable for specific ViewModel
    private readonly TasktreeOverviewViewModel _viewModel;
    
    public ChangeCommand(TasktreeOverviewViewModel viewModel)
    {
        // Übergebenes ViewModel zuweisen
        _viewModel = viewModel;
    }

    
    // Funktion, die ausgeführt wird, wenn der Command ausgeführt wird
    public override void Execute(object parameter)
    {
        var taskId = Convert.ToInt32(parameter);
        // Graphschichten laden
        var layers = _viewModel.TaskLayers;
        
        // jeweilige Task, die die übergebene ID hat, suchen
        // und für das weitere Vorgehen auswählen
        foreach (var group in layers)
        {
            foreach (var taskGroup in group.Value)
            {
                foreach (var availTask in taskGroup.Tasks)
                {
                    if (availTask.Id == taskId)
                    {
                        var isTaskOfSomeoneElse = TaskManager.TakeTaskFromOtherUser(availTask.Id);
                        if (!isTaskOfSomeoneElse)
                        {
                            MessageBox.Show("Du kannst das Spiel nicht selbst abnehmen.");
                        }
                    }
                }
            }
        }
    }
}