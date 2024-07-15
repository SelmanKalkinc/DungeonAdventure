using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventoryPage inventoryUI;

    [SerializeField]
    private InventorySO inventoryData;

    public void Start()
    {
        PrepareUI();
        PrepareInventoryData();
    }

    private void PrepareInventoryData()
    {
        inventoryData.Initialize();
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
        }
    }

    private void PrepareUI()
    {
        inventoryUI.InitializeInventoryUI(inventoryData.Size);
        inventoryUI.gameObject.SetActive(false); // Ensure the UI starts inactive
        Debug.Log("PrepareUI: Inventory UI initialized and set inactive.");
        this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        this.inventoryUI.OnSwapItems += HandleSwapItems;
        this.inventoryUI.OnStartDragging += HandleDragging;
        this.inventoryUI.OnItemActionRequested += HandeItemActionRequest;
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            inventoryUI.ResetSelection();
            return;
        }

        ItemSO item = inventoryItem.item;
        inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }

    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        inventoryData.SwapItems(itemIndex_1, itemIndex_2);
    }

    private void HandleDragging(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            return;
        }
        inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
    }

    private void HandeItemActionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        bool itemConsumed = false;
        if (inventoryItem.IsEmpty)
        {
            return;
        }
        IItemAction itemAction = inventoryItem.item as IItemAction;
        if (itemAction != null)
        {
            itemConsumed = itemAction.PerformAction(gameObject);
        }
        IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
        if (destroyableItem != null && itemConsumed)
        {
            inventoryData.RemoveItem(itemIndex, 1);
        }
        else
        {
            Debug.Log("No Need To Consume This");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inventoryUI.isActiveAndEnabled)
            {
                inventoryUI.Show();
                StartCoroutine(CheckInventoryUIActive()); // Check active state after a short delay
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }

    private IEnumerator CheckInventoryUIActive()
    {
        yield return new WaitForEndOfFrame(); // Wait for end of frame to ensure the active state is updated
        foreach (var item in inventoryData.GetCurrentInventoryState())
        {
            inventoryUI.UpdateData(
                item.Key,
                item.Value.item.ItemImage,
                item.Value.quantity);
        }
    }
}
