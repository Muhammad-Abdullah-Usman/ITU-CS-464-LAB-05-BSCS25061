using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject redCoinPrefab;
    [SerializeField] GameObject blueCoinPrefab;

    [SerializeField] Transform player;
    [SerializeField] float interval = 1.5f;
    [SerializeField] int maxCoins = 5;
    [SerializeField] float area = 7f;
    [SerializeField] int bonusCoinCount = 50;
    [SerializeField] float bonusRadius = 2.5f;

    bool redCoinSpawned = false;

    void Start() => InvokeRepeating(nameof(SpawnCoin), 1f, interval);

    void SpawnCoin()
    {
        if (transform.childCount >= maxCoins) return;
        float x = UnityEngine.Random.Range(-area, area);
        float z = UnityEngine.Random.Range(-area, area);
        Vector3 pos = new Vector3(x, 0.5f, z);
        if (Vector3.Distance(pos, player.position) < 2f) return;
        Instantiate(coinPrefab, pos, coinPrefab.transform.rotation, transform); // removed quaternion.identity in favour of coinprefab.transform.rotation so customised oration is evident
    }

    public void SpawnRedCoin()
    {
        if (redCoinSpawned) return;

        Vector3 pos;
        do
        {
            float x = UnityEngine.Random.Range(-area, area);
            float z = UnityEngine.Random.Range(-area, area);

            pos = new Vector3(x, 0.5f, z);
        }
        while (Vector3.Distance(pos, player.position) < 2f);
        
        Instantiate(redCoinPrefab, pos, redCoinPrefab.transform.rotation);
        redCoinSpawned = true;
    }

    public void SpawnBlueCoins(Vector3 playerPosition)
    {
        StartCoroutine(SpawnBlueCoinsRoutine(playerPosition));
    }

    IEnumerator SpawnBlueCoinsRoutine(Vector3 playerPosition)
    {
        Vector3 center = playerPosition;

        // keep bonus area inside the arena
        center.x = Mathf.Clamp(center.x, -area + bonusRadius, area - bonusRadius);
        center.z = Mathf.Clamp(center.z, -area + bonusRadius, area - bonusRadius);

        for (int i = 0; i < bonusCoinCount; i++)
        {
            float t = (i + 1f) / bonusCoinCount;
            
            // even distirbution through a circular area
            float radius = Mathf.Sqrt(t) * bonusRadius;
            float angle = i * 137.5f * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(center.x + Mathf.Cos(angle) * radius,
                0.5f, center.z + Mathf.Sin(angle) * radius);

            Instantiate(blueCoinPrefab, pos, blueCoinPrefab.transform.rotation);

            // Five coins per frame
            if ((i + 1) % 5 == 0)
            {
                yield return null;
            }
        }
    }
}