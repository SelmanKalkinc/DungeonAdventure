using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltipText; // Reference to the TextMeshProUGUI component
    private RectTransform rectTransform; // Reference to the RectTransform component
    private Canvas canvas; // Reference to the Canvas component
    private CanvasScaler canvasScaler; // Reference to the CanvasScaler component

    void Start()
    {
        Hide(); // Hide the tooltip on start
        rectTransform = GetComponent<RectTransform>(); // Get the RectTransform component
        canvas = GetComponentInParent<Canvas>(); // Get the Canvas component from the parent
        canvasScaler = canvas.GetComponent<CanvasScaler>(); // Get the CanvasScaler component from the Canvas
    }

    public void Show(string itemName, string tooltip)
    {
        if (tooltipText != null)
        {
            tooltipText.text = $"{itemName}\n{tooltip}";
            gameObject.SetActive(true);
            UpdatePosition(); // Update the position to follow the cursor
            Debug.Log("Tooltip shown with text: " + tooltipText.text);
        }
        else
        {
            Debug.LogError("TooltipText is not assigned.");
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Debug.Log("Tooltip hidden");
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            UpdatePosition(); // Continuously update the position if the tooltip is active
        }
    }

    private void UpdatePosition()
    {
        // Ensure the canvasScaler reference is not null before accessing its scaleFactor property
        float scaleFactor = canvasScaler != null ? canvasScaler.scaleFactor : 1f;
        Vector2 mousePosition = (Vector2)Input.mousePosition; // Convert mousePosition to Vector2
        Vector2 offset = new Vector2(15, -25); // Offset to position tooltip slightly below and to the right
        Vector2 anchoredPosition = (mousePosition / scaleFactor) + offset; // Apply offset

        // Get the dimensions of the tooltip
        Vector2 tooltipSize = rectTransform.sizeDelta;

        // Get the dimensions of the canvas
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // Adjust the anchored position to keep the tooltip within screen bounds
        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, 0, canvasSize.x - tooltipSize.x);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, 0, canvasSize.y - tooltipSize.y);

        rectTransform.anchoredPosition = anchoredPosition;

        // Adjust the pivot point to ensure the tooltip appears correctly relative to the cursor
        if (anchoredPosition.x + tooltipSize.x > canvasSize.x)
        {
            rectTransform.pivot = new Vector2(1, rectTransform.pivot.y);
        }
        else
        {
            rectTransform.pivot = new Vector2(0, rectTransform.pivot.y);
        }

        if (anchoredPosition.y + tooltipSize.y > canvasSize.y)
        {
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 1);
        }
        else
        {
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, 0);
        }
    }
}
