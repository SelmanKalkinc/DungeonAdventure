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
            Debug.Log("Hit " + enemy.name);
            // Add damage logic here
        }
    }

    public int Damage => damage;
    public float Range => range;
}
