using System;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public TextMeshProUGUI healthText; // Reference to the TextMeshProUGUI component
    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthText();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            OnDied?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthText();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth.ToString();
        }
    }

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
}
