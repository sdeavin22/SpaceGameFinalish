using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/* Having a client singleton ensures that there is only one instance of ClientSingleton across the entire game application, 
providing a centralized point for managing functionality and avoiding the potential problems, such as multiple instances holding
differing states. This can cause issues with authentication, and issues with multiple instances behaviors 
conflicting with one another */
public class ClientSingleton : MonoBehaviour
{
    // GameManager has a public getter and a private setter, meaning it can be accessed from outside the class but can only be set within the class.
    public ClientGameManager GameManager { get; private set; }
    private static ClientSingleton instance;

    // Propety accessor that ensures only one instance of ClientSingleton exist in game either by getting it from the scene, or finding it and returning it 
    public static ClientSingleton Instance
    {
        get
        {
            // If instance already exists in scene, just return it
            if (instance != null) { return instance; }
            // Searches scene for an object with the ClientSingleton component attatched and stores in instance to be return after search
            instance = FindObjectOfType<ClientSingleton>();
            // Error handling for in the event no instance is found or created
            if (instance == null)
            {
                Debug.LogError("No client singleton in the scene!");
                return null;
            }

            return instance;
        }
    }

    // Ensures gameObject and its corresponding components are not destroyed throughout scene changes, preserving this singleton script's functionality
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public async Task<bool> CreateClient()
    {
        // Creates new instance of the ClientGameManager class
        GameManager = new ClientGameManager();
        // If InitAsynch() successfully finishes, return true and a new client will be initialized. Returning false indicates an issue and a new client will not be created
        return await GameManager.InitAsynch();
    }
}
