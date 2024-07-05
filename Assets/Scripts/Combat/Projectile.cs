using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private int damage;
    private LayerMask targetLayers;
    private Vector2 direction;
    private Collider2D playerCollider;

    public void Initialize(Vector2 direction, int damage, LayerMask targetLayers, Collider2D playerCollider)
    {
        this.direction = direction;
        this.damage = damage;
        this.targetLayers = targetLayers;
        this.playerCollider = playerCollider;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
        Destroy(gameObject, 5f); // Destroy after 5 seconds to avoid lingering
    }

    void Start()
    {
        Debug.Log("Projectile initialized. GameObject: " + gameObject.name + ", Instance ID: " + GetInstanceID() + ", Time: " + Time.time);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile collided with: " + other.gameObject.name + " at " + transform.position);
        if ((targetLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log("Projectile hit a target layer: " + other.gameObject.name);
            // Handle damage logic here
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Projectile hit a non-target layer and will be destroyed.");
            Destroy(gameObject);
        }
    }
}
