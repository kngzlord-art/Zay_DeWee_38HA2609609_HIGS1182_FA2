using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [Header("Collectible")]
    public GameObject moneyPrefab;
    public GameObject scrapPrefab;
    public GameObject healthPrefab;

    [Header("Spawning")]
    public int MoneycollectibleAmount = 5;
    public int ScrapcollectibleAmount = 5;
    public int HealthcollectibleAmount = 5;

    [Header("Spawn Area")]
    public float spawnAreaX = 30f;
    public float spawnAreaY = 10f;
    public float spawnAreaZ = 30f;

    private void Start()
    {
        SpawnMoneyCollectibles();
        SpawnScrapCollectibles();
        SpawnHealthCollectibles();
    }

    public void SpawnMoneyCollectibles()
    {
        for (int i = 0; i < MoneycollectibleAmount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            Instantiate(
                moneyPrefab,
                spawnPosition,
                Quaternion.identity
            );

            Debug.Log("Collectible Spawned");
        }
    }
    public void SpawnScrapCollectibles()
    {
        for (int i = 0; i < ScrapcollectibleAmount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            Instantiate(
                scrapPrefab,
                spawnPosition,
                Quaternion.identity
            );

            Debug.Log("Collectible Spawned");
        }
    }

    public void SpawnHealthCollectibles()
    {
        for (int i = 0; i < HealthcollectibleAmount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            Instantiate(
                healthPrefab,
                spawnPosition,
                Quaternion.identity
            );

            Debug.Log("Collectible Spawned");
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-spawnAreaX, spawnAreaX);
        float y = Random.Range(-spawnAreaY, spawnAreaY);
        float z = Random.Range(-spawnAreaZ, spawnAreaZ);

        return transform.position + new Vector3(x, y, z);
    }

}