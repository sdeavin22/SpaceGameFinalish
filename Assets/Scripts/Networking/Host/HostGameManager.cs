using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Relay;
using System;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    // Fields to store allocation and join code
    private Allocation allocation;
    private string joinCode;
    // Create mx number of connections and creating the game scene
    private const int MaxConnections = 20;
    private const string GameSceneName = "Game";
    public async Task StartHostAsync()
    {
        try
        {
            // Create an allocation for hosting with maximum connections
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }

        try
        {
            // Get the join code for the allocation
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            /* Get the join code for the allocation -> This is the code genereated and displayed in the console upon game execution
             for users (other clients/players) to join game */
            Debug.Log(joinCode);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }

        /* Get the UnityTransport component from the NetworkManager, Create RelayServerData with the allocation and protocol type, and set the relay server data for the transport */
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        // Start hosting the network and load the game scene
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }

}
