using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    private int currentHealth;

    public delegate void EnemyDied();
    public event EnemyDied OnEnemyDied;
    private EnemyDamageDealer enemyDamageDealer;
    private Health health;

    private void Awake()
    {
        enemyDamageDealer = GetComponent<EnemyDamageDealer>();
        health = GetComponent<Health>();
        if (health != null)
        {
            health.OnDied += HandleDeath;
        }
        else
        {
            Debug.LogError("Health component not found on " + gameObject.name);
        }
    }

    public void SetStats(EnemyStats stats)
    {
        enemyStats = stats;
    }

    public void InitializeEnemy()
    {
        currentHealth = enemyStats.health;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && enemyStats.sprite != null)
        {
            sr.sprite = enemyStats.sprite;
        }
        else
        {
            if (sr == null)
            {
                Debug.LogError("SpriteRenderer not found on " + gameObject.name);
            }
            if (enemyStats.sprite == null)
            {
                Debug.LogError("Sprite not assigned in EnemyStats for " + gameObject.name);
            }
        }
        enemyDamageDealer.ActivateDamage();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        OnEnemyDied?.Invoke();
        if (enemyStats.dropTable != null)
        {
            ItemDropManager.Instance.DropItems(enemyStats.dropTable, transform.position); // Drop items on death
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} does not have a drop table assigned in EnemyStats.");
        }
        Destroy(gameObject);
    }
}
