using UnityEngine;

/// <summary>
/// Spawns item pickups in the world at random positions within a rectangular area.
/// Attach this to a GameObject (e.g. "ItemSpawnZone").
/// </summary>
public class WorldItemSpawner : MonoBehaviour
{
    [Header("Spawn Area (centered on this object)")]
    public Vector2 areaSize = new Vector2(10f, 6f);

    [Header("Pickup Prefab")]
    [Tooltip("Prefab with SpriteRenderer + Collider2D (IsTrigger) + ItemPickup.")]
    public GameObject pickupPrefab;

    [Header("Items To Spawn")]
    public ItemData[] itemsToSpawn;
    public int[] amountPerItem;

    [Header("Spawn Timing")]
    public bool spawnOnStart = true;

    [Tooltip("If > 0, will respawn items every X seconds (based on the same settings).")]
    public float respawnInterval = 0f;

    private float respawnTimer = 0f;

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnAllItems();
        }
    }

    void Update()
    {
        if (respawnInterval > 0f)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= respawnInterval)
            {
                respawnTimer = 0f;
                SpawnAllItems();
            }
        }
    }

    [ContextMenu("Spawn Items Now")]
    public void SpawnAllItems()
    {
        if (pickupPrefab == null)
        {
            Debug.LogWarning("WorldItemSpawner: No pickupPrefab assigned.");
            return;
        }

        if (itemsToSpawn == null || itemsToSpawn.Length == 0)
        {
            Debug.LogWarning("WorldItemSpawner: No items to spawn.");
            return;
        }

        for (int i = 0; i < itemsToSpawn.Length; i++)
        {
            ItemData item = itemsToSpawn[i];
            if (item == null)
                continue;

            int count = 1;
            if (amountPerItem != null && i < amountPerItem.Length && amountPerItem[i] > 0)
                count = amountPerItem[i];

            for (int c = 0; c < count; c++)
            {
                Vector2 spawnPos = GetRandomPointInArea();
                GameObject pickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);

                // Configure ItemPickup
                ItemPickup ip = pickup.GetComponent<ItemPickup>();
                if (ip != null)
                {
                    ip.itemData = item;
                    ip.quantity = 1;
                }
                else
                {
                    Debug.LogWarning("WorldItemSpawner: Pickup prefab missing ItemPickup component.");
                }
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, areaSize.y, 0f));
    }
#endif
}
