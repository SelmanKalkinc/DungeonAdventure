using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    public Transform attackPoint;

    private float attackTimer;
    private Transform player;
    private Health playerHealth;
    private Enemy enemy;
    private IAttack currentAttack;

    public void ActivateDamage()
    {
        enemy = GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }

        if (enemy == null)
        {
            Debug.LogError("Enemy component not found on " + gameObject.name);
            return;
        }

        if (enemy.enemyStats != null)
        {
            attackTimer = 1f / enemy.enemyStats.attackRate;

            // Initialize the attack based on the type
            switch (enemy.enemyStats.attackType)
            {
                case AttackType.Melee:
                    currentAttack = new MeleeAttackFactory(
                        enemy.enemyStats.damage,
                        enemy.enemyStats.attackRange,
                        attackPoint,
                        180f
                    ).CreateAttack(LayerMask.GetMask("Player"));
                    break;
                case AttackType.Ranged:
                    currentAttack = new RangedAttackFactory(
                        enemy.enemyStats.damage,
                        enemy.enemyStats.attackRange,
                        attackPoint,
                        enemy.enemyStats.projectilePrefab,
                        GetComponent<Collider2D>(),
                        enemy.enemyStats.projectileSpeed
                    ).CreateAttack(LayerMask.GetMask("Player"));
                    break;
            }
        }
        else
        {
            Debug.LogError("EnemyStats not assigned to " + gameObject.name);
        }

        if (player == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }
        else if (playerHealth == null)
        {
            Debug.LogError("Health component not found on Player GameObject.");
        }
    }

    private void Update()
    {
        if (player == null || playerHealth == null || enemy == null || enemy.enemyStats == null)
        {
            Debug.LogWarning("Player, PlayerHealth, Enemy, or EnemyStats not found");
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            if ((transform.position - player.position).sqrMagnitude <= enemy.enemyStats.attackRange * enemy.enemyStats.attackRange)
            {
                DealDamage();
            }
            attackTimer = 1f / enemy.enemyStats.attackRate;
        }
    }

    private void DealDamage()
    {
        if (currentAttack != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            currentAttack.ExecuteAttack(direction);
        }
        else
        {
            Debug.LogWarning("CurrentAttack is not set.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, enemy.enemyStats.attackRange);
        }
    }
}
