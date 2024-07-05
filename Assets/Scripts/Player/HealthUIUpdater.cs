using UnityEngine;
using TMPro;

public class HealthUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    private void Awake()
    {
        // Find the health text component in the scene
        healthText = GameObject.FindGameObjectWithTag("PlayerHealthText").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
            Debug.Log("Health text found----------");
        }
        else 
        {
            
        }
    }
}
