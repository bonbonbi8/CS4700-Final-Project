using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class SimpleEnemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    private Transform player;
    private EnemyHealth health;
    private float lastAttackTime;
    private bool isChasing = false;

    void Start()
    {
        health = GetComponent<EnemyHealth>();
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Ensure enemy is on correct layer
        if (gameObject.layer == 0)
        {
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            if (enemyLayer != -1)
            {
                gameObject.layer = enemyLayer;
            }
        }
    }

    void Update()
    {
        if (player == null || health.IsDead()) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if player is in detection range
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;

            // Move towards player if not in attack range
            if (distanceToPlayer > attackRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                
                // Face player
                if (direction.magnitude > 0.1f)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                }
            }
            else
            {
                // Attack player if in range
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            isChasing = false;
        }
    }

    void AttackPlayer()
    {
        if (player == null) return;

        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(10f);
            Debug.Log($"{gameObject.name} attacked player!");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

