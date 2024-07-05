using UnityEngine;

public class RangedAttackFactory
{
    private int damage;
    private float range;
    private Transform attackPoint;
    private GameObject projectilePrefab;
    private Collider2D playerCollider;
    private float projectileSpeed;

    public RangedAttackFactory(int damage, float range, Transform attackPoint, GameObject projectilePrefab, Collider2D playerCollider, float projectileSpeed)
    {
        this.damage = damage;
        this.range = range;
        this.attackPoint = attackPoint;
        this.projectilePrefab = projectilePrefab;
        this.playerCollider = playerCollider;
        this.projectileSpeed = projectileSpeed;
    }

    public IAttack CreateAttack(LayerMask targetLayers)
    {
        return new RangedAttack(damage, range, attackPoint, projectilePrefab, targetLayers, playerCollider, projectileSpeed);
    }
}
