using UnityEngine;

public class Player : MonoBehaviour
{
    private Health health;

    void Start()
    {
        GameObject healthManager = GameObject.Find("PlayerHealthManager");
        if (healthManager != null)
        {
            health = healthManager.GetComponent<Health>();
            if (health == null)
            {
                Debug.LogError("Health component not found on PlayerHealthManager.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerHealthManager not found. Health management will be disabled.");
        }
    }

    public void TakeDamage(int damage)
    {
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("Health component not found. Cannot take damage.");
        }
    }

    public void Heal(int amount)
    {
        if (health != null)
        {
            health.Heal(amount);
        }
        else
        {
            Debug.LogWarning("Health component not found. Cannot heal.");
        }
    }
}
