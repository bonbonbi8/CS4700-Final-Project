using UnityEngine;

public class ItemConsumer : MonoBehaviour
{
    private Inventory inventory;
    private PlayerStats playerStats;
    private AudioSource audioSource;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        playerStats = GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Consume an item from inventory
    public bool ConsumeItem(ItemData item)
    {
        if (item == null) return false;

        // Check if item is consumable
        if (item.itemType != ItemData.ItemType.Consumable)
        {
            Debug.LogWarning($"{item.itemName} is not a consumable item!");
            return false;
        }

        // Check if player has the item
        if (inventory.GetItemCount(item) <= 0)
        {
            Debug.LogWarning($"Player doesn't have {item.itemName}!");
            return false;
        }

        // Apply stat effects
        if (playerStats != null)
        {
            if (item.healthRestore > 0)
            {
                playerStats.RestoreHealth(item.healthRestore);
            }
            if (item.hungerRestore > 0)
            {
                playerStats.RestoreHunger(item.hungerRestore);
            }
            if (item.hydrationRestore > 0)
            {
                playerStats.RestoreHydration(item.hydrationRestore);
            }
        }

        // Remove item from inventory
        inventory.RemoveItem(item, 1);

        // Play sound effect (optional)
        if (audioSource != null)
        {
            // You can add a sound effect here
            // audioSource.PlayOneShot(consumeSound);
        }

        Debug.Log($"Consumed {item.itemName}!");
        return true;
    }
}

