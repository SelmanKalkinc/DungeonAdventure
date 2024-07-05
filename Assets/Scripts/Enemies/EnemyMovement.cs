using UnityEngine;

public class EnemyMovement : MonoBehaviour, IMovable
{
    private Transform player;
    private Rigidbody2D rb;
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
    }

    private void FixedUpdate()
    {
        if (enemy != null && enemy.enemyStats != null && player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= enemy.enemyStats.followRange) // Use followRange from EnemyStats
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * enemy.enemyStats.moveSpeed; // Use moveSpeed from EnemyStats
            }
            else
            {
                rb.velocity = Vector2.zero; // Stop moving if out of follow range
            }
        }
    }

    public void Move(Vector2 direction)
    {
        if (enemy != null && enemy.enemyStats != null)
        {
            rb.velocity = direction * enemy.enemyStats.moveSpeed; // Use moveSpeed from EnemyStats
        }
    }
}
