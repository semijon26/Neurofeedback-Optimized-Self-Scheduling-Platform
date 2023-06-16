using System.Collections.Generic;
using Shared;

namespace ClientApplication.Models;

/// <summary>
///  Diese Klasse speichert alle Clients und kann das eigene ClientObject zurückgeben
/// </summary>
public class ClientManagementData
{
    private static ClientManagementData? _instance;
    private ClientManagementData(ClientObject currentClient)
    {
        CurrentClient = currentClient;
        OtherClients = new List<ClientObject>();
    }

    public static ClientManagementData GetInstance(ClientObject currentClient)
    {
        if (_instance == null)
        {
            _instance = new ClientManagementData(currentClient);
            return _instance;
        }
        return _instance;
    }

    public ClientObject CurrentClient { get; private set; }
    public List<ClientObject> OtherClients { get; private set; }
}