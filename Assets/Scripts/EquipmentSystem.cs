using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [Header("Equipment Slots")]
    public Transform weaponHolder; // Where the weapon visual will be attached
    public ItemData currentWeapon;

    [Header("References")]
    private Inventory inventory;
    private PlayerCombat combat;
    private SpriteRenderer weaponSpriteRenderer;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        combat = GetComponent<PlayerCombat>();

        // Setup weapon holder
        if (weaponHolder == null)
        {
            GameObject holder = new GameObject("WeaponHolder");
            holder.transform.SetParent(transform);
            holder.transform.localPosition = Vector3.up * 0.3f; // Slightly above player
            weaponHolder = holder.transform;
        }

        weaponSpriteRenderer = weaponHolder.GetComponent<SpriteRenderer>();
        if (weaponSpriteRenderer == null)
        {
            weaponSpriteRenderer = weaponHolder.gameObject.AddComponent<SpriteRenderer>();
            weaponSpriteRenderer.sortingOrder = 1; // Render above player
        }
    }

    // Equip a weapon/tool
    public bool EquipItem(ItemData item)
    {
        if (item == null) return false;

        if (item.itemType != ItemData.ItemType.Weapon && item.itemType != ItemData.ItemType.Tool)
        {
            Debug.LogWarning($"{item.itemName} is not a weapon or tool!");
            return false;
        }

        // Unequip current weapon if any
        if (currentWeapon != null)
        {
            UnequipItem();
        }

        currentWeapon = item;

        // Update visual representation
        if (weaponSpriteRenderer != null && item.icon != null)
        {
            weaponSpriteRenderer.sprite = item.icon;
            weaponSpriteRenderer.sortingOrder = 1; // Make sure it renders above player
        }

        // Update combat system
        if (combat != null)
        {
            combat.SetWeapon(item);
        }

        Debug.Log($"Equipped {item.itemName}");
        return true;
    }

    // Unequip current weapon
    public void UnequipItem()
    {
        if (currentWeapon == null) return;

        ItemData oldWeapon = currentWeapon;
        currentWeapon = null;

        // Remove visual
        if (weaponSpriteRenderer != null)
        {
            weaponSpriteRenderer.sprite = null;
        }

        // Update combat system
        if (combat != null)
        {
            combat.SetWeapon(null);
        }

        Debug.Log($"Unequipped {oldWeapon.itemName}");
    }

    // Switch to a weapon by item data (if in inventory)
    public bool SwitchToWeapon(ItemData weapon)
    {
        if (weapon == null) return false;
        if (inventory != null && inventory.GetItemCount(weapon) <= 0)
        {
            Debug.LogWarning($"Don't have {weapon.itemName} in inventory!");
            return false;
        }

        return EquipItem(weapon);
    }
}

