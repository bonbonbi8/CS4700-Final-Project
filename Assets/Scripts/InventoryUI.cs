using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;      // The whole inventory window (InventoryPanel)
    public Transform slotParent;           // SlotParent (Grid Layout Group)
    public GameObject slotPrefab;          // InventorySlot prefab

    [Header("Item List to Display")]
    [Tooltip("Drag ALL possible ItemData assets here (wood, apples, water, sword, axe, etc).")]
    public ItemData[] knownItems;

    [Header("Selection Details")]
    public TextMeshProUGUI selectedItemNameText;
    public TextMeshProUGUI selectedItemDescriptionText;
    public Image selectedItemIcon;
    public Button useButton;

    // References on the player
    private Inventory inventory;
    private ItemConsumer itemConsumer;
    private EquipmentSystem equipmentSystem;

    // Internal
    private readonly List<GameObject> spawnedSlots = new List<GameObject>();
    private ItemData selectedItem;

    void Start()
    {
        // Find player + components
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<Inventory>();
            itemConsumer = player.GetComponent<ItemConsumer>();
            equipmentSystem = player.GetComponent<EquipmentSystem>();
        }
        else
        {
            Debug.LogWarning("InventoryUI: No object with tag 'Player' found!");
        }

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);   // make sure it's closed at start

        if (useButton != null)
        {
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(OnUseButtonPressed);
        }

        ClearSelection();
        RefreshUI();
    }

    void Update()
    {
        // Toggle inventory with I
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // --- Open / Close ---

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        bool newState = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(newState);

        if (newState)
        {
            RefreshUI();
        }
    }

    // --- Refresh UI from Inventory ---

    public void RefreshUI()
    {
        // Clear old slots
        foreach (var slot in spawnedSlots)
        {
            if (slot != null) Destroy(slot);
        }
        spawnedSlots.Clear();

        if (slotParent == null || slotPrefab == null || inventory == null || knownItems == null)
            return;

        // Create a slot for each item that the player actually has
        foreach (var item in knownItems)
        {
            if (item == null) continue;

            int count = inventory.GetItemCount(item);
            if (count <= 0) continue; // Player doesn't own this item right now

            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            spawnedSlots.Add(slotObj);

            // Try to find UI components on the slot
            Image iconImage = slotObj.transform.Find("Icon")?.GetComponent<Image>();
            TextMeshProUGUI nameText = slotObj.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI countText = slotObj.transform.Find("CountText")?.GetComponent<TextMeshProUGUI>();

            if (iconImage != null)
            {
                iconImage.sprite = item.icon;
                iconImage.enabled = item.icon != null;
            }

            if (nameText != null)
            {
                nameText.text = item.itemName;
            }

            if (countText != null)
            {
                countText.text = count.ToString();
            }

            // Set up click behavior
            Button button = slotObj.GetComponent<Button>();
            if (button != null)
            {
                ItemData capturedItem = item;
                button.onClick.AddListener(() => OnSlotClicked(capturedItem));
            }
        }

        // If selected item count dropped to 0, clear selection
        if (selectedItem != null && inventory.GetItemCount(selectedItem) <= 0)
        {
            ClearSelection();
        }

        UpdateUseButtonState();
    }

    // --- Slot selection ---

    private void OnSlotClicked(ItemData item)
    {
        if (item == null) return;

        selectedItem = item;

        if (selectedItemNameText != null)
            selectedItemNameText.text = item.itemName;

        if (selectedItemDescriptionText != null)
            selectedItemDescriptionText.text = item.description;

        if (selectedItemIcon != null)
        {
            selectedItemIcon.sprite = item.icon;
            selectedItemIcon.enabled = item.icon != null;
        }

        UpdateUseButtonState();
    }

    private void ClearSelection()
    {
        selectedItem = null;

        if (selectedItemNameText != null)
            selectedItemNameText.text = "";

        if (selectedItemDescriptionText != null)
            selectedItemDescriptionText.text = "";

        if (selectedItemIcon != null)
        {
            selectedItemIcon.sprite = null;
            selectedItemIcon.enabled = false;
        }

        UpdateUseButtonState();
    }

    private void UpdateUseButtonState()
    {
        if (useButton == null)
            return;

        if (selectedItem == null || inventory == null)
        {
            useButton.interactable = false;
            return;
        }

        int count = inventory.GetItemCount(selectedItem);
        if (count <= 0)
        {
            useButton.interactable = false;
            return;
        }

        // You can use or equip Consumable / Tool / Weapon
        if (selectedItem.itemType == ItemData.ItemType.Consumable ||
            selectedItem.itemType == ItemData.ItemType.Tool ||
            selectedItem.itemType == ItemData.ItemType.Weapon)
        {
            useButton.interactable = true;
        }
        else
        {
            useButton.interactable = false; // Materials only (for crafting)
        }
    }

    // --- Use / Equip logic ---

    private void OnUseButtonPressed()
    {
        if (selectedItem == null || inventory == null)
            return;

        int count = inventory.GetItemCount(selectedItem);
        if (count <= 0)
        {
            UpdateUseButtonState();
            return;
        }

        switch (selectedItem.itemType)
        {
            case ItemData.ItemType.Consumable:
                // Eat / drink
                if (itemConsumer != null)
                {
                    bool used = itemConsumer.ConsumeItem(selectedItem);
                    if (used)
                    {
                        RefreshUI();
                    }
                }
                else
                {
                    Debug.LogWarning("InventoryUI: No ItemConsumer on player!");
                }
                break;

            case ItemData.ItemType.Tool:
            case ItemData.ItemType.Weapon:
                // Equip
                if (equipmentSystem != null)
                {
                    bool equipped = equipmentSystem.EquipItem(selectedItem);
                    if (equipped)
                    {
                        Debug.Log($"InventoryUI: Equipped {selectedItem.itemName}");
                    }
                }
                else
                {
                    Debug.LogWarning("InventoryUI: No EquipmentSystem on player!");
                }
                break;

            case ItemData.ItemType.Material:
                Debug.Log($"InventoryUI: {selectedItem.itemName} is a material. Use it in crafting.");
                break;
        }

        UpdateUseButtonState();
    }
}
