using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies around this object, mainly at night.
/// Requires a DayNightCycle in the scene for night-based spawning.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Area")]
    [Tooltip("Rectangle area centered on this spawner where enemies can appear.")]
    public Vector2 areaSize = new Vector2(16f, 10f);

    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    [Tooltip("Maximum number of enemies that this spawner can have alive at once.")]
    public int maxAliveEnemies = 5;

    [Tooltip("Seconds between spawn checks when conditions are met.")]
    public float spawnInterval = 5f;

    [Tooltip("Spawn only at night if true.")]
    public bool onlySpawnAtNight = true;

    [Tooltip("Minimum distance from the player for a spawn to be valid.")]
    public float minDistanceFromPlayer = 5f;

    private float spawnTimer;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private DayNightCycle dayNight;
    private Transform player;

    void Start()
    {
        dayNight = FindObjectOfType<DayNightCycle>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        // Clean up dead references
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] == null)
                aliveEnemies.RemoveAt(i);
        }

        // Check if we should spawn
        if (onlySpawnAtNight && dayNight != null && !dayNight.IsNight)
            return; // do nothing during day

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        if (aliveEnemies.Count >= maxAliveEnemies)
            return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnEnemy();
        }
    }

    void TrySpawnEnemy()
    {
        Vector2 spawnPos = GetRandomPointInArea();

        // If we have a player, enforce minimum distance
        if (player != null)
        {
            float dist = Vector2.Distance(player.position, spawnPos);
            if (dist < minDistanceFromPlayer)
            {
                // Too close, skip this attempt
                return;
            }
        }

        // Pick a random enemy prefab
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        if (prefab == null)
            return;

        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        aliveEnemies.Add(enemy);
    }

    Vector2 GetRandomPointInArea()
    {
        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);

        Vector3 center = transform.position;
        return new Vector2(center.x + x, center.y + y);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, areaSize.y, 0f));
    }
#endif
}
