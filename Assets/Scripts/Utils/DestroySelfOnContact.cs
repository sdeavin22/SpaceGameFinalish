using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is simplu used to destroy the projectile object upon collision with a triggered collider, and plays an explosion audio clip
public class DestroySelfOnContact : MonoBehaviour
{
    [SerializeField] public AudioSource playSound;

    private void OnTriggerEnter2D(Collider2D col)
    {
        playSound.Play();
        Destroy(gameObject);
    }
}
