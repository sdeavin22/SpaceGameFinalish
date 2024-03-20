using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages coin object respawn
public class RespawningCoin : Coin
{
    /* When coin is collected, previous coin position is stored, checks if coin has moved in Update(), 
    and updates previous position to new position for further implementatio of coin objects operations */
    public event Action<RespawningCoin> OnCollected;

    private Vector3 previousPosition;
    private void Update()
    {
        if (previousPosition != transform.position)
        {
            Show(true);
        }

        previousPosition = transform.position;
    }

    // If coin has been collected, return value of coin 
    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }

        if (alreadyCollected) { return 0; }
        alreadyCollected = true;
        OnCollected?.Invoke(this);

        return coinValue;
    }
    public void Reset()
    {
        alreadyCollected = false;
    }

}
