using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{

    /* Called when object is spawned on the network. Base class behavior is executed first, then sets the CanCommitToTransform property based on whether the client owns the object. 
    This method is used to initialize object behavior when it is spawned in a networked environment.*/
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }


    protected override void Update()
    {
        // Allows client to commit changes to the associated game object's transform props
        CanCommitToTransform = IsOwner;
        /* Ensures any behavior in the base class's Update() method is executed before additional logic defined in this class is executed.*/
        base.Update();
        // Checks if client is in a networked session, and if it's "IsOwner", then client can make changes to the associated object's transform props on the server
        if (NetworkManager != null)
        {
            if (NetworkManager.IsConnectedClient || NetworkManager.IsListening)
            {
                if (CanCommitToTransform)
                {
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                }
            }
        }
    }
    // Returning false prevents client to be server authoritive. 
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
