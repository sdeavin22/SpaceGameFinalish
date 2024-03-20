using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Random = UnityEngine.Random;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private RespawningCoin coinPrefab;
    [SerializeField] private int maxCoins = 50;
    [SerializeField] private int coinValue = 10;
    // Range for spawning coins along the x-axis
    [SerializeField] private Vector2 xSpawnRange;
    // Range for spawning coins along the y-axis
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private LayerMask layerMask;

    // Used to ensure no new coin spawns where an existing one is in game
    private Collider2D[] coinBuffer = new Collider2D[1];

    private float coinRadius;

    // Called when coin object is spawned, get radius of it's collider, and spawns the max number of coins 
    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;
        for (int i = 0; i < maxCoins; i++)
        {
            SpawnCoin();
        }
    }
    // Spawns coins
    private void SpawnCoin()
    {
        RespawningCoin coinInstance = Instantiate(coinPrefab, GetSpawnPoint(), Quaternion.identity);
        coinInstance.SetValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();

        coinInstance.OnCollected += HandleCoinCollected;
    }

    /* Method to get a random, valid spawn point for the coin, generate a random location within spawn ranges, 
    creates sawpn point and checks for overlaps in layersm then sapwn in a valid location in game */
    private Vector2 GetSpawnPoint()
    {
        float x = 0;
        float y = 0;
        while (true)
        {
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            Vector2 spawnPoint = new Vector2(x, y);
            int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPoint, coinRadius, coinBuffer, layerMask);
            if (numColliders == 0) { return spawnPoint; }
        }
    }

    // Moves collected coins to a new spawn point in game, and resets coin object's state
    private void HandleCoinCollected(RespawningCoin coin)
    {
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
    }
}
