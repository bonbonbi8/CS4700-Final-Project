using UnityEngine;

/// <summary>
/// Handles cold/warmth effects for the player.
/// Stay near a lit campfire to stay safe; away from fire you take cold damage.
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
    [Tooltip("Health per second lost when NOT near a lit campfire.")]
    public float coldDamagePerSecond = 3f;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerSurvival: No PlayerStats found on Player.");
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
            // Take cold damage when away from fire
            if (coldDamagePerSecond > 0f)
            {
                playerStats.TakeDamage(coldDamagePerSecond * Time.deltaTime);
            }
        }
    }

    private bool IsNearLitCampfire()
    {
        // Look for any Campfire collider in range
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
        // Draw warmth radius in Scene view
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, warmthRange);
    }
}
