using UnityEngine;

/// <summary>
/// Spawns items directly into the player's inventory for testing.
/// Attach to any GameObject (e.g., Objectspawner).
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [Header("Test Items")]
    public ItemData[] testItems;
    public int[] quantities;

    [Header("Auto-spawn on Start")]
    public bool autoSpawnOnStart = true;

    void Start()
    {
        if (autoSpawnOnStart)
        {
            SpawnItemsToPlayer();
        }
    }

    [ContextMenu("Spawn Items To Player")]
    public void SpawnItemsToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("ItemSpawner: No GameObject with tag 'Player' found.");
            return;
        }

        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("ItemSpawner: Player has no Inventory component.");
            return;
        }

        if (testItems == null || testItems.Length == 0)
        {
            Debug.LogWarning("ItemSpawner: No test items assigned.");
            return;
        }

        for (int i = 0; i < testItems.Length; i++)
        {
            ItemData item = testItems[i];
            if (item == null) continue;

            int qty = 1;
            if (quantities != null && i < quantities.Length && quantities[i] > 0)
                qty = quantities[i];

            bool added = inventory.AddItem(item, qty);
            if (added)
            {
                Debug.Log($"ItemSpawner: Added {qty}x {item.itemName} to inventory.");
            }
            else
            {
                Debug.Log("ItemSpawner: Failed to add " + item.itemName);
            }
        }
    }
}
