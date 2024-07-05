using UnityEngine;

public class RangedAttackFactory : IAttackFactory
{
    private int damage;
    private float range;
    private Transform attackPoint;
    private GameObject projectilePrefab;
    private LayerMask targetLayers;
    private Collider2D playerCollider;

    public RangedAttackFactory(int damage, float range, Transform attackPoint, GameObject projectilePrefab, LayerMask targetLayers, Collider2D playerCollider)
    {
        this.damage = damage;
        this.range = range;
        this.attackPoint = attackPoint;
        this.projectilePrefab = projectilePrefab;
        this.targetLayers = targetLayers;
        this.playerCollider = playerCollider;
    }

    public IAttack CreateAttack(Vector2 direction)
    {
        return new RangedAttack(damage, range, attackPoint, projectilePrefab, targetLayers, playerCollider);
    }
}
