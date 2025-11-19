using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    [Header("Attack Visual")]
    public GameObject attackEffectPrefab;
    public Transform attackPoint;

    private ItemData currentWeapon;
    private float lastAttackTime;
    private Animator animator;
    private bool isAttacking = false;

    [Header("Input")]
    public KeyCode attackKey = KeyCode.Space;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (attackPoint == null)
        {
            GameObject point = new GameObject("AttackPoint");
            point.transform.SetParent(transform);
            point.transform.localPosition = Vector3.up * 0.5f;
            attackPoint = point.transform;
        }
    }

    void Start()
    {
        // Default enemy layer if not set
        if (enemyLayer.value == 0)
        {
            enemyLayer = LayerMask.GetMask("Enemy");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && !isAttacking)
        {
            PerformAttack();
        }
    }

    public void SetWeapon(ItemData weapon)
    {
        currentWeapon = weapon;

        if (weapon != null)
        {
            attackRange = weapon.attackRange;
            attackCooldown = weapon.attackCooldown;
        }
        else
        {
            attackRange = 1.5f;
            attackCooldown = 0.5f;
        }
    }

    void PerformAttack()
    {
        // Cooldown check
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        // Require a weapon
        if (currentWeapon == null)
        {
            Debug.Log("No weapon equipped!");
            return;
        }

        lastAttackTime = Time.time;
        isAttacking = true;

        // Animation
        if (animator != null)
            animator.SetTrigger("Attack");

        // Slight delay to match animation
        Invoke(nameof(DealDamage), 0.2f);
        Invoke(nameof(ResetAttackState), attackCooldown);
    }

    void DealDamage()
    {
        // Find enemies in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        float damage = currentWeapon != null ? currentWeapon.damage : 10f;

        foreach (var hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Hit {enemy.gameObject.name} for {damage} damage!");
            }
        }

        // Visual effect
        if (attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, attackPoint.position, attackPoint.rotation);
            Destroy(effect, 0.5f);
        }
    }

    void ResetAttackState()
    {
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
