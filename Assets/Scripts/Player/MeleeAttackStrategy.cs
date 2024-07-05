using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy
{
    public void Attack(Transform attackPoint, float attackRange, int attackDamage, LayerMask enemyLayers, float attackAngle)
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 directionToEnemy = enemy.transform.position - attackPoint.position;
            float angle = Vector2.Angle(attackPoint.right, directionToEnemy);

            if (angle <= attackAngle / 2)
            {
                // Enemy is within the attack arc
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
    }
}
