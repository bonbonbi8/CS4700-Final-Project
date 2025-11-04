using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject craftingPanel;
    public Transform recipeListParent;
    public GameObject recipeButtonPrefab;
    public Button craftButton;
    public TextMeshProUGUI recipeDescriptionText;
    public TextMeshProUGUI materialsText;

    [Header("Recipe Data")]
    public List<CraftingRecipe> availableRecipes = new List<CraftingRecipe>();

    private Inventory playerInventory;
    private CraftingRecipe selectedRecipe;
    private List<GameObject> recipeButtons = new List<GameObject>();

    void Start()
    {
        // Find player inventory
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInventory = player.GetComponent<Inventory>();
        }

        // Setup UI
        if (craftingPanel != null)
        {
            craftingPanel.SetActive(false);
        }

        if (craftButton != null)
        {
            craftButton.onClick.AddListener(OnCraftButtonClicked);
        }

        PopulateRecipeList();
    }

    void Update()
    {
        // Toggle crafting menu with 'C' key
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCraftingMenu();
        }
    }

    public void ToggleCraftingMenu()
    {
        if (craftingPanel != null)
        {
            bool isActive = craftingPanel.activeSelf;
            craftingPanel.SetActive(!isActive);
            
            if (!isActive)
            {
                RefreshRecipeList();
            }
        }
    }

    void PopulateRecipeList()
    {
        if (recipeListParent == null || recipeButtonPrefab == null) return;

        // Clear existing buttons
        foreach (var button in recipeButtons)
        {
            if (button != null) Destroy(button);
        }
        recipeButtons.Clear();

        // Create buttons for each recipe
        foreach (var recipe in availableRecipes)
        {
            if (recipe == null) continue;

            GameObject buttonObj = Instantiate(recipeButtonPrefab, recipeListParent);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = recipe.recipeName;
            }

            if (button != null)
            {
                CraftingRecipe recipeRef = recipe; // Capture for closure
                button.onClick.AddListener(() => SelectRecipe(recipeRef));
            }

            recipeButtons.Add(buttonObj);
        }
    }

    void RefreshRecipeList()
    {
        PopulateRecipeList();
        if (selectedRecipe != null)
        {
            SelectRecipe(selectedRecipe);
        }
    }

    void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        UpdateRecipeDisplay();
        UpdateCraftButton();
    }

    void UpdateRecipeDisplay()
    {
        if (selectedRecipe == null) return;

        // Update description
        if (recipeDescriptionText != null)
        {
            recipeDescriptionText.text = selectedRecipe.description;
        }

        // Update materials text
        if (materialsText != null && playerInventory != null)
        {
            string materialsInfo = "Required Materials:\n";
            foreach (var requirement in selectedRecipe.requiredMaterials)
            {
                if (requirement.material == null) continue;
                int available = playerInventory.GetItemCount(requirement.material);
                int required = requirement.quantity;
                string color = available >= required ? "green" : "red";
                materialsInfo += $"<color={color}>{requirement.material.itemName}: {available}/{required}</color>\n";
            }

            materialsInfo += $"\nOutput: {selectedRecipe.outputItem.itemName} x{selectedRecipe.outputQuantity}";
            materialsText.text = materialsInfo;
        }
    }

    void UpdateCraftButton()
    {
        if (craftButton == null || selectedRecipe == null || playerInventory == null) return;

        bool canCraft = selectedRecipe.CanCraft(playerInventory);
        craftButton.interactable = canCraft;
    }

    void OnCraftButtonClicked()
    {
        if (selectedRecipe == null || playerInventory == null) return;

        if (selectedRecipe.Craft(playerInventory))
        {
            Debug.Log($"Successfully crafted {selectedRecipe.outputItem.itemName}!");
            UpdateRecipeDisplay();
            UpdateCraftButton();
            
            // Visual feedback - you could add particle effects or sound here
            StartCoroutine(FlashCraftButton());
        }
        else
        {
            Debug.LogWarning("Failed to craft item!");
        }
    }

    System.Collections.IEnumerator FlashCraftButton()
    {
        if (craftButton == null) yield break;

        Color originalColor = craftButton.image.color;
        craftButton.image.color = Color.green;
        yield return new WaitForSeconds(0.2f);
        craftButton.image.color = originalColor;
    }

    void OnEnable()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged += UpdateCraftButton;
        }
    }

    void OnDisable()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= UpdateCraftButton;
        }
    }
}

