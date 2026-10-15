using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
    [Header("Asteroid")]
    public GameObject asteroidPrefab;
    public GameObject movAsteroidPrefab;
    public List<GameObject> startingAsteroids = new List<GameObject>();


    [Header("SpawnTimes")]
    public float spawnTime1 = 140;
    public float spawnTime2 = 100;
    public float spawnTime3 = 60;
    

    [Header("SpawningAmounts")]
    public int asteroidAmount = 10;
    public int SpawnAmount1 = 1;
    public int SpawnAmount2 = 2;
    public int SpawnAmount3 = 3;

    [Header("Wave Checks")]
    public bool wave1Spawned = false;
    public bool wave2Spawned = false;
    public bool wave3Spawned = false;

    [Header("Spawn Area")]
    public float spawnAreaX = 30f;
    public float spawnAreaY = 10f;
    public float spawnAreaZ = 30f;

 

    [Header("GameManager")]
    public GameManager GM;


    private void Start()
    {
        if (GM == null)
        {
            GM = FindFirstObjectByType<GameManager>();
        }

        if (GM == null)
        {
            Debug.LogError("The GameManager was not found");
        }

        SpawnAsteroids();
    }

    private void Update()
    {
        if (!wave1Spawned && GM.time <= spawnTime1)
        {
            SpawnMovAsteroid1();
            wave1Spawned = true;
        }
        if (!wave2Spawned && GM.time <= spawnTime2)
        {
            SpawnMovAsteroid2();
            wave2Spawned = true;
        }
        if (!wave3Spawned && GM.time <= spawnTime3)
        {
            SpawnMovAsteroid3();
            wave3Spawned = true;
        }

        StartingAsteroidDestroyed(asteroidPrefab);
    }

    public void SpawnAsteroids()
    {
        for (int i = 0; i < asteroidAmount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            GameObject asteroid = Instantiate(
                asteroidPrefab,
                spawnPosition,
                Random.rotation
            );

            startingAsteroids.Add(asteroid);

            Debug.Log("Starting asteroids added Remaining: " + startingAsteroids.Count);

            Debug.Log("Starting Asteroid Spawned");

            if (GM != null)
            {
                GM.RegisterAsteroids();
            }
        }
    }

    public void SpawnMovAsteroid1()
    {
        for (int i = 0; i < SpawnAmount1; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            Instantiate(
                movAsteroidPrefab,
                spawnPosition,
                Random.rotation
            );

            Debug.Log("The first Wave asteroid has spawned in.");
        }
    }
    public void SpawnMovAsteroid2()
    {
        for (int i = 0; i < SpawnAmount2; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            Instantiate(
                movAsteroidPrefab,
                spawnPosition,
                Random.rotation
            );

            Debug.Log("The second wave asteroids have spawned in.");
        }
    }
    public void SpawnMovAsteroid3()
    {
        for (int i = 0; i < SpawnAmount3; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            Instantiate(
                movAsteroidPrefab,
                spawnPosition,
                Random.rotation
            );

            Debug.Log("The Third wave asteroids have spawned in");
        }
    }

        public void StartingAsteroidDestroyed(GameObject asteroid)
        {
            if(startingAsteroids.Remove(asteroid))
            {
                Debug.Log("Starting asteroid destroyed. Remaining: " + startingAsteroids.Count);
            }

            if(startingAsteroids.Count == 0 )
            {
                GM.WinGame();
            }
        }


    public void RegisterStartingAsteroid(GameObject asteroid)
    {
        startingAsteroids.Add(asteroid);

        Debug.Log("Starting asteroid registered. Total starting asteroids: " + startingAsteroids.Count);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-spawnAreaX, spawnAreaX);
        float y = Random.Range(-spawnAreaY, spawnAreaY);
        float z = Random.Range(-spawnAreaZ, spawnAreaZ);

        return transform.position + new Vector3(x, y, z);
    }
}