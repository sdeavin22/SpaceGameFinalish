using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// This clas manages th total number of coins collected by player, and adds up the total value in player "wallet"
public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<Coin>(out Coin coin)) { return; }

        int coinValue = coin.Collect();
        if (!IsServer) { return; }

        TotalCoins.Value += coinValue;
    }
}
