using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("Health UI")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    [Header("Hunger UI")]
    public Slider hungerSlider;
    public TextMeshProUGUI hungerText;

    [Header("Hydration UI")]
    public Slider hydrationSlider;
    public TextMeshProUGUI hydrationText;

    private PlayerStats playerStats;

    void Start()
    {
        // Find player stats
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            
            if (playerStats != null)
            {
                // Subscribe to stat changes
                playerStats.OnHealthChanged.AddListener(UpdateHealthUI);
                playerStats.OnHungerChanged.AddListener(UpdateHungerUI);
                playerStats.OnHydrationChanged.AddListener(UpdateHydrationUI);

                // Initialize UI
                UpdateHealthUI(playerStats.GetHealth());
                UpdateHungerUI(playerStats.GetHunger());
                UpdateHydrationUI(playerStats.GetHydration());
            }
        }
    }

    void UpdateHealthUI(float health)
    {
        if (playerStats == null) return;

        if (healthSlider != null)
        {
            healthSlider.value = playerStats.GetHealthPercent();
        }

        if (healthText != null)
        {
            healthText.text = $"Health: {Mathf.Ceil(health)}/{playerStats.maxHealth}";
        }
    }

    void UpdateHungerUI(float hunger)
    {
        if (playerStats == null) return;

        if (hungerSlider != null)
        {
            hungerSlider.value = playerStats.GetHungerPercent();
        }

        if (hungerText != null)
        {
            hungerText.text = $"Hunger: {Mathf.Ceil(hunger)}/{playerStats.maxHunger}";
        }
    }

    void UpdateHydrationUI(float hydration)
    {
        if (playerStats == null) return;

        if (hydrationSlider != null)
        {
            hydrationSlider.value = playerStats.GetHydrationPercent();
        }

        if (hydrationText != null)
        {
            hydrationText.text = $"Hydration: {Mathf.Ceil(hydration)}/{playerStats.maxHydration}";
        }
    }

    void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged.RemoveListener(UpdateHealthUI);
            playerStats.OnHungerChanged.RemoveListener(UpdateHungerUI);
            playerStats.OnHydrationChanged.RemoveListener(UpdateHydrationUI);
        }
    }
}

