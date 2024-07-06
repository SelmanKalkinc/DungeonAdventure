using System;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public TextMeshProUGUI healthText; // Optional: if you have a health UI
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
            Die();
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

    private void Die()
    {
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject);
    }

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
}
