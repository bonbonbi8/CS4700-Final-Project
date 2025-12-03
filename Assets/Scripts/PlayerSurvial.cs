using UnityEngine;

/// <summary>
/// Handles cold/warmth effects for the player.
/// - Stay near a lit campfire to be safe (and regen health).
/// - Away from any lit campfire, you take cold damage.
/// - At night, cold damage can be multiplied.
/// </summary>
[RequireComponent(typeof(PlayerStats))]
public class PlayerSurvival : MonoBehaviour
{
    [Header("Warmth Settings")]
    [Tooltip("How close you need to be to a campfire to be considered 'warm'.")]
    public float warmthRange = 3f;

    [Tooltip("Health per second restored while near a lit campfire.")]
    public float healthRegenNearFire = 2f;

    [Header("Cold Damage Settings")]
    [Tooltip("Health per second lost when NOT near a lit campfire (base value).")]
    public float coldDamagePerSecond = 3f;

    [Tooltip("Multiplier applied to cold damage at night.")]
    public float nightColdMultiplier = 2f;

    private PlayerStats playerStats;
    private DayNightCycle dayNight;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerSurvival: No PlayerStats found on Player.");
        }

        dayNight = FindObjectOfType<DayNightCycle>();
        if (dayNight == null)
        {
            Debug.Log("PlayerSurvival: No DayNightCycle found in scene (night multiplier will be ignored).");
        }
    }

    void Update()
    {
        if (playerStats == null)
            return;

        bool nearLitFire = IsNearLitCampfire();

        if (nearLitFire)
        {
            // Heal a bit when by a warm fire
            if (healthRegenNearFire > 0f)
            {
                playerStats.RestoreHealth(healthRegenNearFire * Time.deltaTime);
            }
        }
        else
        {
            // Take cold damage when away from any lit fire
            float dmg = coldDamagePerSecond;

            if (dayNight != null && dayNight.IsNight)
            {
                dmg *= nightColdMultiplier;
            }

            if (dmg > 0f)
            {
                playerStats.TakeDamage(dmg * Time.deltaTime);
            }
        }
    }

    private bool IsNearLitCampfire()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, warmthRange);

        foreach (Collider2D hit in hits)
        {
            Campfire campfire = hit.GetComponent<Campfire>();
            if (campfire != null && campfire.IsLit)
            {
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, warmthRange);
    }
}
