using UnityEngine;

public abstract class BaseAttack : IAttack
{
    protected int damage;
    protected float range;
    protected Transform attackPoint;
    protected LayerMask enemyLayers;

    public BaseAttack(int damage, float range, Transform attackPoint, LayerMask enemyLayers)
    {
        this.damage = damage;
        this.range = range;
        this.attackPoint = attackPoint;
        this.enemyLayers = enemyLayers;
    }

    public int Damage => damage;

    public float Range => range;

    public abstract void ExecuteAttack(Vector2 direction);
}
