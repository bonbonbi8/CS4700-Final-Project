using UnityEngine;

/// <summary>
/// Simple campfire that burns fuel over time and can be refueled
/// using items from the player's inventory.
/// </summary>
public class Campfire : MonoBehaviour
{
    [Header("Fuel Settings")]
    public int maxFuel = 100;
    public int startingFuel = 50;
    public float burnRatePerSecond = 2f;   // how many fuel units burn per second

    [Tooltip("Items that can be used as fuel.")]
    public ItemData[] fuelItems;

    [Tooltip("How much fuel each item in 'fuelItems' adds.")]
    public int[] fuelValues;

    [Header("Visuals (optional)")]
    public SpriteRenderer fireSprite;
    public Light fireLight; // or Light2D if you're using URP, just change type

    private float currentFuel;

    public bool IsLit => currentFuel > 0f;

    void Awake()
    {
        currentFuel = Mathf.Clamp(startingFuel, 0, maxFuel);

        if (fireSprite == null)
            fireSprite = GetComponent<SpriteRenderer>();

        UpdateVisuals();
    }

    void Update()
    {
        if (!IsLit)
            return;

        // Burn fuel over time
        currentFuel -= burnRatePerSecond * Time.deltaTime;

        if (currentFuel <= 0f)
        {
            currentFuel = 0f;
            Debug.Log("Campfire: went out.");
        }

        UpdateVisuals();
    }

    /// <summary>
    /// Try to add fuel using items from the given inventory.
    /// Returns true if at least one item was consumed.
    /// </summary>
    public bool TryAddFuel(Inventory inventory)
    {
        if (inventory == null || fuelItems == null || fuelItems.Length == 0)
        {
            Debug.LogWarning("Campfire: No inventory or fuel items set.");
            return false;
        }

        bool addedAny = false;

        for (int i = 0; i < fuelItems.Length; i++)
        {
            if (i >= fuelValues.Length) break;

            ItemData fuelItem = fuelItems[i];
            int fuelAmount = fuelValues[i];

            if (fuelItem == null || fuelAmount <= 0)
                continue;

            int count = inventory.GetItemCount(fuelItem);
            if (count <= 0)
                continue;

            // Consume ONE item and add fuel
            bool removed = inventory.RemoveItem(fuelItem, 1);
            if (removed)
            {
                currentFuel += fuelAmount;
                currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
                Debug.Log($"Campfire: added 1x {fuelItem.itemName}, fuel = {currentFuel}/{maxFuel}");
                addedAny = true;
                break; // only use one item per interaction
            }
        }

        if (addedAny)
            UpdateVisuals();
        else
            Debug.Log("Campfire: no suitable fuel in inventory.");

        return addedAny;
    }

    private void UpdateVisuals()
    {
        float t = (maxFuel > 0) ? currentFuel / maxFuel : 0f;

        // Fade sprite based on fuel
        if (fireSprite != null)
        {
            Color c = fireSprite.color;
            c.a = Mathf.Lerp(0.2f, 1f, t);
            fireSprite.color = c;
        }

        // Adjust light if one is assigned
        if (fireLight != null)
        {
            fireLight.intensity = Mathf.Lerp(0f, 1.5f, t);
            fireLight.range = Mathf.Lerp(0f, 5f, t);
        }
    }
}
