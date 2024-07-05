using UnityEngine;

public class MeleeAttack : IAttack
{
    private int damage;
    private float range;
    private Transform attackPoint;
    private LayerMask targetLayers;
    private float attackAngle;

    public MeleeAttack(int damage, float range, Transform attackPoint, LayerMask targetLayers, float attackAngle)
    {
        this.damage = damage;
        this.range = range;
        this.attackPoint = attackPoint;
        this.targetLayers = targetLayers;
        this.attackAngle = attackAngle;
    }

    public void ExecuteAttack(Vector2 direction)
    {
        Debug.Log("Executing Melee Attack");
        if (attackPoint == null)
        {
            Debug.LogError("attackPoint is null");
            return;
        }

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, range, targetLayers);
        foreach (Collider2D target in hitTargets)
        {
            Vector2 toTarget = (target.transform.position - attackPoint.position).normalized;
            float angle = Vector2.Angle(direction, toTarget);

            if (angle <= attackAngle / 2)
            {
                Debug.Log("Hit " + target.name);
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth != null)
                {
                    Debug.Log("Damaging " + target.name);
                    targetHealth.TakeDamage(damage);
                }
                else
                {
                    Debug.LogError("Health component not found on " + target.name);
                }
            }
        }
    }

    public int Damage => damage;
    public float Range => range;
}
