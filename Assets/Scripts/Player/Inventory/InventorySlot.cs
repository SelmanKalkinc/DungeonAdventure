using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    private Item item;

    public void SetItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && UIManager.Instance != null)
        {
            UIManager.Instance.ShowTooltip(item.itemName, item.itemTooltip); // Use item tooltip from ScriptableObject
            Debug.Log("OnPointerEnter called for item: " + item.itemName); // Add debug log
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideTooltip();
            Debug.Log("OnPointerExit called"); // Add debug log
        }
    }
}
