using System;
using System.Windows;
using ClientApplication.Utils;

namespace ClientApplication.Commands;


public class TakeOtherUsersTaskCommand : CommandBase
{
    
    public override void Execute(object parameter)
    {
        var taskId = Convert.ToInt32(parameter);

        var isTaskOfSomeoneElse = TaskManager.TakeTaskFromOtherUser(taskId);
        if (!isTaskOfSomeoneElse)
        {
            MessageBox.Show("Du kannst das Spiel nicht selbst abnehmen.");
        }
    }
}