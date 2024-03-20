using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Rendering;
using System;

public class Health : NetworkBehaviour
{

    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    // The current health of the object, synced across the network
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
    private bool isDead;
    // Event triggered when the object dies
    public Action<Health> OnDie;

    // Called when the player object is spawned on the network, giving them max health
    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        CurrentHealth.Value = MaxHealth;
    }


    // Apply when player takes damage
    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
    }

    // If players dies, exit or calculat health upon taking damage or receiving health. 
    // If health is 0, call OnDie and set isDead bool to true
    private void ModifyHealth(int value)
    {
        if (isDead) { return; }
        int newHealth = CurrentHealth.Value + value;
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);

        if (CurrentHealth.Value == 0)
        {
            OnDie?.Invoke(this);
            isDead = true;
        }
    }
}



