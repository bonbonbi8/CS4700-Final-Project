using UnityEngine;

/// <summary>
/// Helper script to spawn items for testing. Attach to a GameObject and click "Spawn Items" in the inspector.
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [Header("Test Items")]
    public ItemData[] testItems;
    public int[] quantities;

    [Header("Auto-spawn on Start")]
    public bool autoSpawnOnStart = false;

    void Start()
    {
        if (autoSpawnOnStart)
        {
            SpawnAllItems();
        }
    }

    [ContextMenu("Spawn All Items")]
    public void SpawnAllItems()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
            return;
        }

        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Player doesn't have Inventory component!");
            return;
        }

        if (testItems == null || testItems.Length == 0)
        {
            Debug.LogWarning("No test items assigned!");
            return;
        }

        for (int i = 0; i < testItems.Length; i++)
        {
            if (testItems[i] != null)
            {
                int quantity = (quantities != null && i < quantities.Length) ? quantities[i] : 1;
                for (int j = 0; j < quantity; j++)
                {
                    inventory.AddItem(testItems[i]);
                }
                Debug.Log($"Added {quantity}x {testItems[i].itemName} to inventory");
            }
        }
    }
}

