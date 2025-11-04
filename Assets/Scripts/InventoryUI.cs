using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;
    public Transform inventorySlotParent;
    public GameObject inventorySlotPrefab;

    private Inventory playerInventory;
    private List<GameObject> slotObjects = new List<GameObject>();

    void Start()
    {
        // Find player inventory
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInventory = player.GetComponent<Inventory>();
            
            if (playerInventory != null)
            {
                playerInventory.OnInventoryChanged += RefreshInventory;
            }
        }

        // Setup UI
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        RefreshInventory();
    }

    void Update()
    {
        // Toggle inventory with 'I' key
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    void RefreshInventory()
    {
        if (inventorySlotParent == null || inventorySlotPrefab == null || playerInventory == null) return;

        // Clear existing slots
        foreach (var slot in slotObjects)
        {
            if (slot != null) Destroy(slot);
        }
        slotObjects.Clear();

        // Create slots for each item
        var items = playerInventory.GetAllItems();
        foreach (var kvp in items)
        {
            ItemData item = kvp.Key;
            int quantity = kvp.Value;

            GameObject slotObj = Instantiate(inventorySlotPrefab, inventorySlotParent);
            
            // Setup slot UI
            Image iconImage = slotObj.GetComponentInChildren<Image>();
            TextMeshProUGUI quantityText = slotObj.GetComponentInChildren<TextMeshProUGUI>();
            Button slotButton = slotObj.GetComponent<Button>();

            if (iconImage != null && item.icon != null)
            {
                iconImage.sprite = item.icon;
            }

            if (quantityText != null)
            {
                quantityText.text = quantity > 1 ? quantity.ToString() : "";
            }

            // Add click handler for consumables
            if (slotButton != null && item.itemType == ItemData.ItemType.Consumable)
            {
                ItemData itemRef = item; // Capture for closure
                slotButton.onClick.AddListener(() => OnConsumableClicked(itemRef));
            }

            slotObjects.Add(slotObj);
        }
    }

    void OnConsumableClicked(ItemData item)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            ItemConsumer consumer = player.GetComponent<ItemConsumer>();
            if (consumer != null)
            {
                consumer.ConsumeItem(item);
            }
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

