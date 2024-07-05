using UnityEngine;

public class MeleeAttackFactory
{
    private int damage;
    private float range;
    private Transform attackPoint;
    private float attackAngle;

    public MeleeAttackFactory(int damage, float range, Transform attackPoint, float attackAngle)
    {
        this.damage = damage;
        this.range = range;
        this.attackPoint = attackPoint;
        this.attackAngle = attackAngle;
    }

    public IAttack CreateAttack(LayerMask targetLayers)
    {
        return new MeleeAttack(damage, range, attackPoint, targetLayers, attackAngle);
    }
}
