using ClientApplication.Models;
using Shared;

namespace ClientApplication.Utils;

public static class TaskManager
{
    public static int? GetTaskIdByGameType(GameType gameType)
    {
        foreach (var keyValuePair in ClientObject.GetInstance().ActiveGames)
        {
            if (keyValuePair.Value.GameType == gameType)
            {
                return keyValuePair.Key;
            }
        }
        return null;
    }

    
    // Returns true if task is from someone else (not one of the client's own tasks), otherwise method returns false
    public static bool TakeTaskFromOtherUser(int taskId)
    {
        if (!GetClientObject().ActiveGames.ContainsKey(taskId))
        {
            UpdatePulledTaskByServer(taskId);
            return true;
        }

        return false;
    }
    
    private static ClientObject GetClientObject()
    {
        var currentClient = ClientManagementData.GetInstance(ClientObject.GetInstance()).CurrentClient;
        return currentClient;
    }

    private static void UpdatePulledTaskByServer(int pulledTaskId)
    {
        TaskGraphProvider.GetInstance().SendUpdatedTaskGraphToServer(new DataPayload{SetDone = false, ChangeWorker = true, IntValue = pulledTaskId, Woker = ClientObject.GetInstance()});
    }

    public static bool IsGameCurrentlyActive(int taskId, GameType gameType)
    {
        var clientsData = ClientManagementData.GetInstance(ClientObject.GetInstance());
        if (IsGameActiveByClient(clientsData.CurrentClient, taskId) || clientsData.CurrentClient.ContainsGameType(gameType)) return true;

        foreach (var otherClient in clientsData.OtherClients)
        {
            if (IsGameActiveByClient(otherClient, taskId)) return true;
        }

        return false;
    }

    private static bool IsGameActiveByClient(ClientObject client, int taskId)
    {
        foreach (var clientActiveGame in client.ActiveGames)
        {
            if (clientActiveGame.Key == taskId)
            {
                return true;
            }
        }
        return false;
    }

    public static void RemoveActiveTaskForCurrentClient(int taskId)
    {
        ClientObject.GetInstance().ActiveGames.Remove(taskId);
        ClientManagementSocket.SendClientObjectWhenConnectionEstablished();
    }
}