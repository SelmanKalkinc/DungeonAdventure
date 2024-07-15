using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image borderImage;

    public event Action<UIInventoryItem> ItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnItemClicked, OnRightMouseBtnClicked;

    private bool empty;

    public void Awake()
    {
        ResetData();
        Deselect();
    }


    public void ResetData()
    {
        if (itemImage != null && itemImage.gameObject != null)
        {
            itemImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Item image or its game object is null in UIInventoryItem.ResetData()");
        }
        empty = true;
    }

    public void Deselect()
    {
        if (borderImage != null)
        {
            borderImage.enabled = false;
        }
        else
        {
            Debug.LogWarning("Border image is null in UIInventoryItem.Deselect()");
        }
    }

    public void SetData(Sprite sprite, int quantity)
    {
        if (itemImage != null && itemImage.gameObject != null)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Item image or its game object is null in UIInventoryItem.SetData()");
        }

        if (quantityText != null)
        {
            quantityText.text = quantity.ToString();
        }
        else
        {
            Debug.LogWarning("Quantity text is null in UIInventoryItem.SetData()");
        }

        empty = false;
    }

    public void Select()
    {
        if (borderImage != null)
        {
            borderImage.enabled = true;
        }
        else
        {
            Debug.LogWarning("Border image is null in UIInventoryItem.Select()");
        }
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClicked?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
