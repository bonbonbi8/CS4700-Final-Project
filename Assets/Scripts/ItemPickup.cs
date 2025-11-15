using UnityEngine;

/// <summary>
/// World item that the player can pick up by walking into it.
/// Requires:
/// - A Collider2D with "Is Trigger" checked
/// - The Player has tag "Player" and an Inventory component
/// </summary>
public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemData itemData;
    public int quantity = 1;

    [Header("Pickup Settings")]
    public bool destroyOnPickup = true;

    private void Reset()
    {
        // Try to auto-assign sprite based on ItemData icon
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = gameObject.AddComponent<SpriteRenderer>();

        if (itemData != null && itemData.icon != null)
        {
            sr.sprite = itemData.icon;
        }

        // Make sure we have a trigger collider
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<CircleCollider2D>();
        }
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("ItemPickup: Player has no Inventory component.");
            return;
        }

        if (itemData == null)
        {
            Debug.LogWarning("ItemPickup: No ItemData assigned on " + gameObject.name);
            return;
        }

        if (quantity <= 0)
            quantity = 1;

        bool added = inventory.AddItem(itemData, quantity);
        if (added)
        {
            Debug.Log($"ItemPickup: Picked up {quantity}x {itemData.itemName}");
            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("ItemPickup: Inventory full, could not pick up " + itemData.itemName);
        }
    }
}
