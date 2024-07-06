using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private int damage;
    private LayerMask targetLayers;
    private Vector2 direction;
    private Collider2D playerCollider;

    public void Initialize(Vector2 direction, int damage, LayerMask targetLayers, Collider2D playerCollider, float speed)
    {
        this.direction = direction.normalized; // Normalize the direction vector
        this.damage = damage;
        this.targetLayers = targetLayers;
        this.playerCollider = playerCollider;
        this.speed = speed;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
        Destroy(gameObject, 5f); // Destroy after 5 seconds to avoid lingering
    }


    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("Health component not found on " + other.gameObject.name);
            }
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
