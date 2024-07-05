using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Health health;
    public HealthBar healthBar;

    private void Awake()
    {
        if (health == null)
        {
            health = GetComponent<Health>();
            if (health == null)
            {
                Debug.LogError("Health component not found!");
            }
        }

        health.OnHealthChanged += HandleHealthChanged;
    }

    private void Start()
    {
        health.ResetHealth();
        healthBar.SetMaxHealth(health.MaxHealth);
    }

    private void OnDestroy()
    {
        health.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }
}
