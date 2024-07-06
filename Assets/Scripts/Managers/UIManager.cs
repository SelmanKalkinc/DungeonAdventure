using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject tooltipPanel; // Reference to the TooltipPanel GameObject
    private Tooltip tooltip; // Reference to the Tooltip script

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (tooltipPanel != null)
        {
            tooltip = tooltipPanel.GetComponent<Tooltip>();
            if (tooltip == null)
            {
                Debug.LogError("Tooltip component not found on the TooltipPanel.");
            }
        }
        else
        {
            Debug.LogError("TooltipPanel is not assigned in the UIManager.");
        }
    }

    public void ShowTooltip(string itemName, string tooltipText)
    {
        if (tooltip != null)
        {
            tooltip.Show(itemName, tooltipText);
            Debug.Log("ShowTooltip called with text: " + tooltipText); // Add debug log
        }
    }

    public void HideTooltip()
    {
        if (tooltip != null)
        {
            tooltip.Hide();
            Debug.Log("HideTooltip called"); // Add debug log
        }
    }
}
