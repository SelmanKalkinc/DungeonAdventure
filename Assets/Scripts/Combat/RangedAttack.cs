using UnityEngine;

public class RangedAttack : IAttack
{
    private int damage;
    private float range;
    private Transform attackPoint;
    private GameObject projectilePrefab;
    private LayerMask targetLayers;
    private Collider2D playerCollider;

    public RangedAttack(int damage, float range, Transform attackPoint, GameObject projectilePrefab, LayerMask targetLayers, Collider2D playerCollider)
    {
        this.damage = damage;
        this.range = range;
        this.attackPoint = attackPoint;
        this.projectilePrefab = projectilePrefab;
        this.targetLayers = targetLayers;
        this.playerCollider = playerCollider;
    }

    public void ExecuteAttack(Vector2 direction)
    {
        Debug.Log("Executing Ranged Attack");
        if (projectilePrefab != null)
        {
            Debug.Log("Instantiating projectile from RangedAttack");
            GameObject projectileInstance = Object.Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
            Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(direction, damage, targetLayers, playerCollider);
            }
        }
    }

    public int Damage => damage;
    public float Range => range;
}
