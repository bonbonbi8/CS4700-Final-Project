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

    [Header("Damage When Low")]
    public float lowStatThreshold = 20f;
    public float starvingDamage = 1f;
    public float dehydratedDamage = 1.5f;

    // Events for UI
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<float> OnHungerChanged;
    public UnityEvent<float> OnHydrationChanged;
    public UnityEvent OnDeath;

    private Coroutine hungerDecayCoroutine;
    private Coroutine hydrationDecayCoroutine;
    private Coroutine healthRegenCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentHydration = maxHydration;

        StartStatCoroutines();

        // Initial UI update
        OnHealthChanged?.Invoke(currentHealth);
        OnHungerChanged?.Invoke(currentHunger);
        OnHydrationChanged?.Invoke(currentHydration);
    }

    void StartStatCoroutines()
    {
        if (hungerDecayCoroutine != null) StopCoroutine(hungerDecayCoroutine);
        if (hydrationDecayCoroutine != null) StopCoroutine(hydrationDecayCoroutine);
        if (healthRegenCoroutine != null) StopCoroutine(healthRegenCoroutine);

        hungerDecayCoroutine = StartCoroutine(HungerDecayCoroutine());
        hydrationDecayCoroutine = StartCoroutine(HydrationDecayCoroutine());
        healthRegenCoroutine = StartCoroutine(HealthRegenCoroutine());
    }

    IEnumerator HungerDecayCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerDecayInterval);

            currentHunger = Mathf.Max(0, currentHunger - hungerDecayRate * hungerDecayInterval);
            OnHungerChanged?.Invoke(currentHunger);

            if (currentHunger <= lowStatThreshold)
            {
                TakeDamage(starvingDamage * hungerDecayInterval);
            }
        }
    }

    IEnumerator HydrationDecayCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(hydrationDecayInterval);

            currentHydration = Mathf.Max(0, currentHydration - hydrationDecayRate * hydrationDecayInterval);
            OnHydrationChanged?.Invoke(currentHydration);

            if (currentHydration <= lowStatThreshold)
            {
                TakeDamage(dehydratedDamage * hydrationDecayInterval);
            }
        }
    }

    IEnumerator HealthRegenCoroutine()
    {
        float timer = 0f;

        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (currentHealth < maxHealth && currentHunger > lowStatThreshold && currentHydration > lowStatThreshold)
            {
                timer += 0.1f;
                if (timer >= healthRegenDelay && healthRegenRate > 0)
                {
                    currentHealth = Mathf.Min(maxHealth, currentHealth + healthRegenRate * 0.1f);
                    OnHealthChanged?.Invoke(currentHealth);
                }
            }
            else
            {
                timer = 0f;
            }
        }
    }

    // ---------------------------
    // Public methods
    // ---------------------------

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
