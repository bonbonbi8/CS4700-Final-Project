using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;
    public float healthRegenRate = 0f; // Per second
    public float healthRegenDelay = 5f; // Delay before regen starts

    [Header("Hunger")]
    public float maxHunger = 100f;
    private float currentHunger;
    public float hungerDecayRate = 0.5f; // Per second
    public float hungerDecayInterval = 1f; // How often to decay

    [Header("Hydration")]
    public float maxHydration = 100f;
    private float currentHydration;
    public float hydrationDecayRate = 0.8f; // Per second
    public float hydrationDecayInterval = 1f; // How often to decay

    [Header("Death Handling")]
    public float lowStatThreshold = 20f; // Health damage when hunger/thirst is low

    // Events
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<float> OnHungerChanged;
    public UnityEvent<float> OnHydrationChanged;
    public UnityEvent OnDeath;

    private Coroutine hungerDecayCoroutine;
    private Coroutine hydrationDecayCoroutine;
    private Coroutine healthRegenCoroutine;

    void Start()
    {
        // Initialize stats
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentHydration = maxHydration;

        // Start decay coroutines
        StartCoroutines();

        // Trigger initial UI updates
        OnHealthChanged?.Invoke(currentHealth);
        OnHungerChanged?.Invoke(currentHunger);
        OnHydrationChanged?.Invoke(currentHydration);
    }

    void StartCoroutines()
    {
        if (hungerDecayCoroutine != null) StopCoroutine(hungerDecayCoroutine);
        if (hydrationDecayCoroutine != null) StopCoroutine(hydrationDecayCoroutine);
        if (healthRegenCoroutine != null) StopCoroutine(healthRegenCoroutine);

        hungerDecayCoroutine = StartCoroutine(HungerDecayCoroutine());
        hydrationDecayCoroutine = StartCoroutine(HydrationDecayCoroutine());
        healthRegenCoroutine = StartCoroutine(HealthRegenCoroutine());
    }

    // Hunger decay coroutine
    IEnumerator HungerDecayCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerDecayInterval);
            
            if (currentHunger > 0)
            {
                currentHunger = Mathf.Max(0, currentHunger - hungerDecayRate * hungerDecayInterval);
                OnHungerChanged?.Invoke(currentHunger);

                // Take health damage if hunger is too low
                if (currentHunger <= lowStatThreshold)
                {
                    TakeDamage(1f * hungerDecayInterval);
                }
            }
        }
    }

    // Hydration decay coroutine
    IEnumerator HydrationDecayCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(hydrationDecayInterval);
            
            if (currentHydration > 0)
            {
                currentHydration = Mathf.Max(0, currentHydration - hydrationDecayRate * hydrationDecayInterval);
                OnHydrationChanged?.Invoke(currentHydration);

                // Take health damage if hydration is too low
                if (currentHydration <= lowStatThreshold)
                {
                    TakeDamage(1.5f * hydrationDecayInterval);
                }
            }
        }
    }

    // Health regeneration coroutine
    IEnumerator HealthRegenCoroutine()
    {
        float timeSinceLastDamage = 0f;

        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (currentHealth < maxHealth && currentHunger > lowStatThreshold && currentHydration > lowStatThreshold)
            {
                timeSinceLastDamage += 0.1f;
                if (timeSinceLastDamage >= healthRegenDelay && healthRegenRate > 0)
                {
                    currentHealth = Mathf.Min(maxHealth, currentHealth + healthRegenRate * 0.1f);
                    OnHealthChanged?.Invoke(currentHealth);
                }
            }
            else
            {
                timeSinceLastDamage = 0f;
            }
        }
    }

    // Public methods to modify stats
    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void RestoreHunger(float amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
        OnHungerChanged?.Invoke(currentHunger);
    }

    public void RestoreHydration(float amount)
    {
        currentHydration = Mathf.Clamp(currentHydration + amount, 0, maxHydration);
        OnHydrationChanged?.Invoke(currentHydration);
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    // Getters
    public float GetHealth() => currentHealth;
    public float GetHunger() => currentHunger;
    public float GetHydration() => currentHydration;
    public float GetHealthPercent() => currentHealth / maxHealth;
    public float GetHungerPercent() => currentHunger / maxHunger;
    public float GetHydrationPercent() => currentHydration / maxHydration;
}

