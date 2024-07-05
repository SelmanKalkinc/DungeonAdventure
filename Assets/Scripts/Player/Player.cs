using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthUIUpdater healthUIUpdater;
    private int currentHealth = 100;
    private int maxHealth = 100;

    void Start()
    {
        if (healthUIUpdater != null) 
        {
            GameObject healthManager = GameObject.Find("PlayerHealthManager");
            healthUIUpdater = healthManager.GetComponent<HealthUIUpdater>();
            UpdateHealthUI();
        }
        
    }

    void UpdateHealthUI()
    {
        if (healthUIUpdater != null)
        {
            healthUIUpdater.UpdateHealthText(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }
}
