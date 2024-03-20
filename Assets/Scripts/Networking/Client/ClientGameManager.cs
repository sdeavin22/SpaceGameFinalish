using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport.Relay;

public class ClientGameManager
{
    private JoinAllocation allocation;
    private const string MenuSceneName = "Menu";


    // If InitAsynch() authState successfully authenticates in the AuthenticationWrapper class, return true.
    // Return false if not, indicating an issue came up in DoAuth()
    public async Task<bool> InitAsynch()
    {
        await UnityServices.InitializeAsync();
        AuthState authState = await AuthenticationWrapper.DoAuth();

        if (authState == AuthState.Authenticated)
        {
            return true;
        }

        return false;
    }

    // Loads menu screen
    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }


    public async Task StartClientAsync(string joinCode)
    {
        try
        {   // obtains a join allocation from the Unity Relay service using the specified join code
            allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }

        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }

        /* Configures the networking transport (UnityTransport) to use Unity's Relay service for network comms, 
        initializes the relay server data, and starts the client functionality of the networking system. 
        This enables the client to establish connections and communicate with other networked entities in the game, in our case, the host */
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
    }
}
