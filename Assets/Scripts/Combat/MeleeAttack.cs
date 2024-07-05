using UnityEngine;

public class MeleeAttack : IAttack
{
    private int damage;
    private float range;
    private Transform attackPoint;
    private LayerMask enemyLayers;
    private float attackAngle;

    public MeleeAttack(int damage, float range, Transform attackPoint, LayerMask enemyLayers, float attackAngle)
    {
        this.damage = damage;
        this.range = range;
        this.attackPoint = attackPoint;
        this.enemyLayers = enemyLayers;
        this.attackAngle = attackAngle;
    }

    public void ExecuteAttack(Vector2 direction)
    {
        Debug.Log("Executing Melee Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, range, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 toEnemy = (enemy.transform.position - attackPoint.position).normalized;
            float angle = Vector2.Angle(direction, toEnemy);

            if (angle <= attackAngle / 2)
            {
                Debug.Log("Hit " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    public int Damage => damage;
    public float Range => range;
}
