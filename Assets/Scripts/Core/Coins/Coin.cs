using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public abstract class Coin : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected int coinValue = 10;
    // Flag if coin is collected already
    protected bool alreadyCollected;
    public abstract int Collect();

    public void SetValue(int value)
    {
        coinValue = value;
    }

    // Shows the coin sprite depending on bool value
    protected void Show(bool show)
    {
        spriteRenderer.enabled = show;
    }
}
