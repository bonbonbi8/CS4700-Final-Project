using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public int maxSlots = 20;

    // Dictionary: ItemData -> quantity
    private Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    // Events for UI updates
    public event Action<ItemData, int> OnItemAdded;
    public event Action<ItemData, int> OnItemRemoved;
    public event Action OnInventoryChanged;

    // Get count of specific item
    public int GetItemCount(ItemData item)
    {
        if (item == null) return 0;
        return items.ContainsKey(item) ? items[item] : 0;
    }

    // Add item to inventory
    public bool AddItem(ItemData item, int quantity = 1)
    {
        if (item == null) return false;

        if (items.ContainsKey(item))
        {
            items[item] += quantity;
        }
        else
        {
            // Check if we have space for a new item type
            if (items.Count >= maxSlots)
            {
                Debug.LogWarning("Inventory is full!");
                return false;
            }
            items[item] = quantity;
        }

        // Clamp to max stack size
        if (items[item] > item.maxStackSize)
        {
            int excess = items[item] - item.maxStackSize;
            items[item] = item.maxStackSize;
            OnItemAdded?.Invoke(item, item.maxStackSize - (items[item] - quantity));
            OnInventoryChanged?.Invoke();
            return false; // Couldn't add all items
        }

        OnItemAdded?.Invoke(item, quantity);
        OnInventoryChanged?.Invoke();
        return true;
    }

    // Remove item from inventory
    public bool RemoveItem(ItemData item, int quantity = 1)
    {
        if (item == null || !items.ContainsKey(item)) return false;

        if (items[item] <= quantity)
        {
            int removed = items[item];
            items.Remove(item);
            OnItemRemoved?.Invoke(item, removed);
            OnInventoryChanged?.Invoke();
            return true;
        }
        else
        {
            items[item] -= quantity;
            OnItemRemoved?.Invoke(item, quantity);
            OnInventoryChanged?.Invoke();
            return true;
        }
    }

    // Get all items as a list
    public Dictionary<ItemData, int> GetAllItems()
    {
        return new Dictionary<ItemData, int>(items);
    }

    // Check if inventory has space
    public bool HasSpace()
    {
        return items.Count < maxSlots;
    }
}

