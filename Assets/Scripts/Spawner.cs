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
    [SerializeField] float bonusRadius = 3f;

    bool redCoinSpawned = false;

    void Start() => InvokeRepeating(nameof(SpawnCoin), 1f, interval);

    void SpawnCoin()
    {
        if (transform.childCount >= maxCoins) return;
        float x = Random.Range(-area, area);
        float z = Random.Range(-area, area);
        Vector3 pos = new Vector3(x, 0.5f, z);
        if (Vector3.Distance(pos, player.position) < 2f) return;
        Instantiate(coinPrefab, pos, Quaternion.identity, transform);
    }

    public void SpawnRedCoin()
    {
        if (redCoinSpawned) return;

        Vector3 pos;
        do
        {
            float x = Random.Range(-area, area);
            float z = Random.Range(-area, area);

            pos = new Vector3(x, 0.5f, z);
        }
        while (Vector3.Distance(pos, player.position) < 2f);
        
        Instantiate(redCoinPrefab, pos, Quaternion.identity);
        redCoinSpawned = true;
    }

    public void SpawnBlueCoins(Vector3 playerPosition)
    {
        int rings = 5;
        int coinsPerRing = bonusCoinCount / rings;
        float minRadius = 1f;
        
        for (int ring = 0; ring < rings; ring++)
        {
            float radius = Mathf.Lerp(minRadius, bonusRadius, ring / (float)(rings - 1));

            for (int i = 0; i <  coinsPerRing; i++)
            {
                float angle = i * Mathf.PI * 2f / coinsPerRing;

                // offset alternate rings
                if (ring % 2 == 2)
                {
                    angle += Mathf.PI / coinsPerRing;
                }

                Vector3 pos = new Vector3(playerPosition.x + Mathf.Cos(angle) * radius,
                    0.5f, playerPosition.z + Mathf.Sin(angle) * radius);
                Instantiate(blueCoinPrefab, pos, Quaternion.identity);
            }
        }
    }
}
