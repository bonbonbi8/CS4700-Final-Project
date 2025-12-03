using UnityEngine;

public class ObjectSpawner2D : MonoBehaviour
{
    [Header("Map size in tiles (same as ground)")]
    public int mapWidth = 50;
    public int mapHeight = 50;

    [Header("Grid reference")]
    public Grid grid;  // drag your Grid object here in the Inspector

    [Header("Prefabs")]
    public GameObject housePrefab;
    public GameObject treePrefab;
    public GameObject vendorPrefab;

    [Header("Spawn chances per tile")]
    [Range(0f, 1f)] public float treeChance = 0.1f;
    [Range(0f, 1f)] public float houseChance = 0.03f;
    [Range(0f, 1f)] public float vendorChance = 0.01f;

    [Header("Keep center clear (e.g. for player start)")]
    public bool keepCenterClear = true;

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        if (grid == null)
        {
            Debug.LogError("Grid reference is missing on ObjectSpawner2D!");
            return;
        }

        int xStart = -mapWidth / 2;
        int yStart = -mapHeight / 2;

        Vector3Int centerCell = Vector3Int.zero;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                int tileX = xStart + x;
                int tileY = yStart + y;
                Vector3Int cellPos = new Vector3Int(tileX, tileY, 0);

                // Don’t place anything on the center tile if you want it free
                if (keepCenterClear && cellPos == centerCell)
                    continue;

                // One random roll per tile
                float r = Random.value;

                // Decide what to spawn (at most one per tile)
                if (r < treeChance && treePrefab != null)
                {
                    SpawnAtCell(treePrefab, cellPos);
                }
                else if (r < treeChance + houseChance && housePrefab != null)
                {
                    SpawnAtCell(housePrefab, cellPos);
                }
                else if (r < treeChance + houseChance + vendorChance && vendorPrefab != null)
                {
                    SpawnAtCell(vendorPrefab, cellPos);
                }
            }
        }
    }

    void SpawnAtCell(GameObject prefab, Vector3Int cellPos)
    {
        // Get the world position at the center of the cell
        Vector3 worldPos = grid.GetCellCenterWorld(cellPos);

        Instantiate(prefab, worldPos, Quaternion.identity, transform);
    }
}
