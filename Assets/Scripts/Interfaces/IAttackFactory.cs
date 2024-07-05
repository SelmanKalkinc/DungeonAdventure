using UnityEngine;

public interface IAttackFactory
{
    IAttack CreateAttack(Vector2 shootDirection);
}
