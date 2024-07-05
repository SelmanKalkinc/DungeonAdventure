using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    private int currentHealth;

    public delegate void EnemyDied();
    public event EnemyDied OnEnemyDied;
    private EnemyDamageDealer enemyDamageDealer;

    private void Awake()
    {
        enemyDamageDealer = GetComponent<EnemyDamageDealer>();
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
            Die();
        }
    }

    private void Die()
    {
        OnEnemyDied?.Invoke();
        Destroy(gameObject);
    }
}
