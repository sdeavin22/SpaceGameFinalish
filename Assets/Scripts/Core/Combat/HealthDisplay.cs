using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;

    // Manipulates health on the player
    public override void OnNetworkSpawn()
    {
        if (!IsClient) { return; }
        health.CurrentHealth.OnValueChanged += HandleHealthChange;
        HandleHealthChange(0, health.CurrentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) { return; }
        health.CurrentHealth.OnValueChanged -= HandleHealthChange;
    }

    // Hanlde change in health and update the "fill amount" the health display canvas in game
    private void HandleHealthChange(int oldHealth, int newHealth)
    {
        healthBarImage.fillAmount = (float)newHealth / health.MaxHealth;
    }
}
