using UnityEngine;

public interface IAttackStrategy
{
    void Attack(Transform attackPoint, float attackRange, int attackDamage, LayerMask enemyLayers, float attackAngle);
}
