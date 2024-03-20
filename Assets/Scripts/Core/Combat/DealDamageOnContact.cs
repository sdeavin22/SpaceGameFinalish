using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    private ulong ownerClientID;
    // Set owner's client ID
    public void SetOwner(ulong ownerClientID)
    {
        this.ownerClientID = ownerClientID;
    }

    // If two colliders interact. If there's no rigidbody on an object (an object we don't want projectiles to have an affect on)
    // or if projectil hits the player object that launched it, have no affect. Otherwise, use it to diminish enemy player's health bar
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody == null) { return; }
        if (col.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if (ownerClientID == netObj.OwnerClientId) { return; }
        }
        if (col.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}
