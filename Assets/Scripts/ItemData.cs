using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public string description;
    public Sprite icon;
    public int maxStackSize = 1;

    [Header("Item Type")]
    public ItemType itemType;

    [Header("Consumable Stats (if applicable)")]
    public float healthRestore = 0f;
    public float hungerRestore = 0f;
    public float hydrationRestore = 0f;

    [Header("Equipment Stats (if applicable)")]
    public float damage = 0f;
    public float attackRange = 1f;
    public float attackCooldown = 0.5f;

    public enum ItemType
    {
        Material,
        Consumable,
        Tool,
        Weapon
    }
}

