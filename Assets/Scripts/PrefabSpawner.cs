using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject prefab;
        public int count = 1;
    }

    [Header("Spawn Area (centered on this object)")]
    public Vector2 areaSize = new Vector2(20f, 12f);

    [Header("Objects To Spawn")]
    public SpawnEntry[] objectsToSpawn;

    [Header("Timing")]
    public bool spawnOnStart = true;

    [Tooltip("If > 0, respawn this pattern every X seconds.")]
    public float respawnInterval = 0f;

    private float timer = 0f;

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnAll();
        }
    }

    void Update()
    {
        if (respawnInterval > 0f)
        {
            timer += Time.deltaTime;
            if (timer >= respawnInterval)
            {
                timer = 0f;
                SpawnAll();
            }
        }
    }

    [ContextMenu("Spawn Now")]
    public void SpawnAll()
    {
        if (objectsToSpawn == null)
            return;

        foreach (var entry in objectsToSpawn)
        {
            if (entry.prefab == null || entry.count <= 0)
                continue;

            for (int i = 0; i < entry.count; i++)
            {
                Vector2 pos = GetRandomPointInArea();
                Instantiate(entry.prefab, pos, Quaternion.identity);
            }
        }
    }

    private Vector2 GetRandomPointInArea()
    {
        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);

        Vector3 center = transform.position;
        return new Vector2(center.x + x, center.y + y);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, areaSize.y, 0f));
    }
#endif
}
