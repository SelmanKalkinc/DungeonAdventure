using UnityEngine;

public interface IDamageDealer
{
    int Damage { get; }
    void DealDamage(GameObject target);
}
