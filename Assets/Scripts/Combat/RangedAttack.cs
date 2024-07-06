using UnityEngine;

public class RangedAttack : IAttack
{
    private int damage;
    private float range;
    private Transform attackPoint;
    private GameObject projectilePrefab;
    private LayerMask targetLayers;
    private Collider2D playerCollider;
    private float projectileSpeed;

    public RangedAttack(int damage, float range, Transform attackPoint, GameObject projectilePrefab, LayerMask targetLayers, Collider2D playerCollider, float projectileSpeed)
    {
        this.damage = damage;
        this.range = range;
        this.attackPoint = attackPoint;
        this.projectilePrefab = projectilePrefab;
        this.targetLayers = targetLayers;
        this.playerCollider = playerCollider;
        this.projectileSpeed = projectileSpeed;
    }

    public void ExecuteAttack(Vector2 direction)
    {
        if (projectilePrefab != null)
        {
            GameObject projectileInstance = Object.Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
            Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(direction, damage, targetLayers, playerCollider, projectileSpeed);
            }
            else
            {
                Debug.LogError("Projectile script not found on projectile prefab.");
            }
        }
        else
        {
            Debug.LogError("Projectile prefab is null.");
        }
    }

    public int Damage => damage;
    public float Range => range;
}
