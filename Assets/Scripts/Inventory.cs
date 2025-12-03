using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public int maxSlots = 20;

    // Internal storage: ItemData → Quantity
    private Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    // UI event
    public event Action OnInventoryChanged;

    // ---------------------------------------------------
    // ADD ITEM
    // ---------------------------------------------------
    public bool AddItem(ItemData item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
            return false;

        // If new item and no space → fail
        if (!items.ContainsKey(item))
        {
            if (items.Count >= maxSlots)
            {
                Debug.Log("Inventory full: " + item.itemName);
                return false;
            }
            items[item] = 0;
        }

        int current = items[item];
        int maxStack = item.maxStackSize > 0 ? item.maxStackSize : int.MaxValue;

        current += quantity;
        current = Mathf.Clamp(current, 0, maxStack);

        items[item] = current;

        OnInventoryChanged?.Invoke();
        return true;
    }

    // ---------------------------------------------------
    // REMOVE ITEM
    // ---------------------------------------------------
    public bool RemoveItem(ItemData item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
            return false;

        if (!items.TryGetValue(item, out int current))
            return false;

        current -= quantity;

        if (current <= 0)
            items.Remove(item);
        else
            items[item] = current;

        OnInventoryChanged?.Invoke();
        return true;
    }

    // ---------------------------------------------------
    // GET QUANTITY
    // ---------------------------------------------------
    public int GetQuantity(ItemData item)
    {
        if (item == null) return 0;
        return items.TryGetValue(item, out int q) ? q : 0;
    }

    // ---------------------------------------------------
    // COMPATIBILITY FOR OTHER SCRIPTS
    // ---------------------------------------------------
    public int GetItemCount(ItemData item)
    {
        return GetQuantity(item);
    }

    // ---------------------------------------------------
    // GET ALL ITEMS (for UI)
    // ---------------------------------------------------
    public Dictionary<ItemData, int> GetAllItems()
    {
        return new Dictionary<ItemData, int>(items);
    }

    // ---------------------------------------------------
    // CHECK SPACE
    // ---------------------------------------------------
    public bool HasSpaceFor(ItemData item)
    {
        if (item == null) return false;

        if (items.ContainsKey(item))
            return true; // stackable

        return items.Count < maxSlots;
    }
}
