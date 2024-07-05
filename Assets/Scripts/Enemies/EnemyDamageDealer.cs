using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    private float attackTimer;
    private Transform player;
    private Health playerHealth;
    private Enemy enemy;

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
        }

        if (enemy.enemyStats != null)
        {
            attackTimer = 1f / enemy.enemyStats.attackSpeed;
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
            attackTimer = 1f / enemy.enemyStats.attackSpeed;
        }
    }

    private void DealDamage()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(enemy.enemyStats.damage);
        }
        else
        {
            Debug.LogWarning("PlayerHealth component not found on player.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (enemy != null && enemy.enemyStats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemy.enemyStats.attackRange);
        }
    }
}
