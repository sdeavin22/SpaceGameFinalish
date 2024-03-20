using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton clientPrefab;
    [SerializeField] private HostSingleton hostPrefab;

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);
        // checks if this is a dedicated server or not, no rendering needed.
        await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async Task LaunchInMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {

        }

        else
        {
            HostSingleton hostSingleton = Instantiate(hostPrefab);
            hostSingleton.CreateHost();

            ClientSingleton clientSingleton = Instantiate(clientPrefab);
            bool Authenticated = await clientSingleton.CreateClient();

            if (Authenticated)
            {
                clientSingleton.GameManager.GoToMenu();
            }

        }
    }
}
