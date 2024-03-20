using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/* HHaving a host singleton ensures that there is only one instance of HostSingleton across the entire game application, 
providing a centralized point for managing functionality and avoiding the potential problems, such as multiple instances holding
differing states. This can cause issues with authentication, and issues with multiple instances behaviors 
conflicting with one anothe*/
public class HostSingleton : MonoBehaviour
{
    // GameManager has a public getter and a private setter, meaning it can be accessed from outside the class but can only be set within the class.
    public HostGameManager GameManager { get; private set; }
    private static HostSingleton instance;

    // Propety accessor that ensures only one instance of HostSingleton exist in game either by getting it from the scene, or finding it and returning it 
    public static HostSingleton Instance
    {
        get
        {
            // If instance already exists in scene, just return it
            if (instance != null) { return instance; }

            // Searches scene for an object with the HostSingleton component attatched and stores in instance to be return after search
            instance = FindObjectOfType<HostSingleton>();

            // Error handling for in the event no instance is found or created
            if (instance == null)
            {
                Debug.LogError("No host singleton in the scene!");
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

    // Creates new instance of the ClientGameManager class
    public void CreateHost()
    {
        GameManager = new HostGameManager();
    }
}
