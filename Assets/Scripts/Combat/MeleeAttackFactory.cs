using UnityEngine;

public class MeleeAttackFactory : IAttackFactory
{
    private int damage;
    private float range;
    private Transform attackPoint;
    private LayerMask enemyLayers;
    private float attackAngle;

    public MeleeAttackFactory(int damage, float range, Transform attackPoint, LayerMask enemyLayers, float attackAngle)
    {
        this.damage = damage;
        this.range = range;
        this.attackPoint = attackPoint;
        this.enemyLayers = enemyLayers;
        this.attackAngle = attackAngle;
    }

    public IAttack CreateAttack(Vector2 shootDirection)
    {
        // For melee attacks, the direction may not be necessary
        return new MeleeAttack(damage, range, attackPoint, enemyLayers, attackAngle);
    }
}
