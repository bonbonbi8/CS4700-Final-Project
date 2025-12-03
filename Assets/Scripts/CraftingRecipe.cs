using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Game/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Recipe Info")]
    public string recipeName;
    public string description;

    [Header("Required Materials")]
    public List<MaterialRequirement> requiredMaterials = new List<MaterialRequirement>();

    [Header("Output")]
    public ItemData outputItem;
    public int outputQuantity = 1;

    [Header("Recipe Unlock")]
    public bool isUnlockedByDefault = true;
    public string unlockCondition; // Optional: for future unlock system

    [Serializable]
    public class MaterialRequirement
    {
        public ItemData material;
        public int quantity;
    }

    // Check if player has all required materials
    public bool CanCraft(Inventory inventory)
    {
        if (inventory == null) return false;

        foreach (var requirement in requiredMaterials)
        {
            if (requirement.material == null) continue;
            int availableQuantity = inventory.GetItemCount(requirement.material);
            if (availableQuantity < requirement.quantity)
            {
                return false;
            }
        }
        return true;
    }

    // Craft the item (assumes CanCraft was checked first)
    public bool Craft(Inventory inventory)
    {
        if (!CanCraft(inventory)) return false;

        // Remove required materials
        foreach (var requirement in requiredMaterials)
        {
            inventory.RemoveItem(requirement.material, requirement.quantity);
        }

        // Add output item
        for (int i = 0; i < outputQuantity; i++)
        {
            inventory.AddItem(outputItem);
        }

        return true;
    }
}

