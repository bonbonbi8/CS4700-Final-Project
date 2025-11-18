using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;       // Panel toggled with I
    public Transform inventorySlotParent;   // Parent for all slot instances
    public GameObject inventorySlotPrefab;  // Prefab with Image + TMP_Text + Button

    private Inventory playerInventory;
    private ItemConsumer itemConsumer;
    private EquipmentSystem equipmentSystem;

    void Start()
    {
        // Start closed
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        // Find player + systems
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInventory = player.GetComponent<Inventory>();
            itemConsumer = player.GetComponent<ItemConsumer>();
            equipmentSystem = player.GetComponent<EquipmentSystem>();

            if (playerInventory != null)
            {
                playerInventory.OnInventoryChanged += RefreshInventory;
                RefreshInventory();
            }
            else
            {
                Debug.LogWarning("InventoryUI: Player has no Inventory component!");
            }

            if (itemConsumer == null)
                Debug.LogWarning("InventoryUI: Player has no ItemConsumer component!");

            if (equipmentSystem == null)
                Debug.LogWarning("InventoryUI: Player has no EquipmentSystem component!");
        }
        else
        {
            Debug.LogWarning("InventoryUI: No GameObject tagged 'Player' found.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && inventoryPanel != null)
        {
            bool newState = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(newState);

            if (newState && playerInventory != null)
            {
                RefreshInventory();
            }
        }
    }

    private void RefreshInventory()
    {
        if (inventorySlotParent == null || playerInventory == null)
            return;

        // Clear existing UI slots
        for (int i = inventorySlotParent.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotParent.GetChild(i).gameObject);
        }

        // Build new slots from inventory contents
        Dictionary<ItemData, int> items = playerInventory.GetAllItems();

        foreach (KeyValuePair<ItemData, int> kvp in items)
        {
            ItemData item = kvp.Key;
            int quantity = kvp.Value;

            GameObject slotGO = Instantiate(inventorySlotPrefab, inventorySlotParent);

            // Icon + quantity text
            Image iconImage = slotGO.GetComponentInChildren<Image>();
            TMP_Text qtyText = slotGO.GetComponentInChildren<TMP_Text>();

            if (iconImage != null && item != null)
                iconImage.sprite = item.icon;

            if (qtyText != null)
                qtyText.text = quantity > 1 ? quantity.ToString() : "";

            slotGO.name = item != null ? item.itemName + "_Slot" : "EmptySlot";

            // Button behavior
            Button button = slotGO.GetComponent<Button>();
            if (button != null && item != null)
            {
                ItemData capturedItem = item; // capture for lambda
                button.onClick.AddListener(() =>
                {
                    HandleItemClick(capturedItem);
                    RefreshInventory(); // refresh after use/equip
                });
            }
        }
    }

    private void HandleItemClick(ItemData item)
    {
        if (item == null) return;

        switch (item.itemType)
        {
            case ItemData.ItemType.Consumable:
                if (itemConsumer != null)
                    itemConsumer.ConsumeItem(item);
                break;

            case ItemData.ItemType.Weapon:
            case ItemData.ItemType.Tool:
                if (equipmentSystem != null)
                    equipmentSystem.EquipItem(item);
                break;

            // Materials etc. do nothing for now
            default:
                Debug.Log("Clicked on " + item.itemName + " (no direct action yet).");
                break;
        }
    }

    void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= RefreshInventory;
        }
    }
}
