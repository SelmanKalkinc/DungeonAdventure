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
        this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        this.inventoryUI.OnSwapItems += HandleSwapItems;
        this.inventoryUI.OnStartDragging += HandleDragging;
        this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;
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
        string description = item.Description;
        if (inventoryItem.quality.HasValue)
        {
            description += "\nQuality: " + inventoryItem.quality.Value;
        }
        inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.Name, description);
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

    private void HandleMergeItems(int fromIndex, int toIndex)
    {
        inventoryData.MergeItems(fromIndex, toIndex);
    }

    private void HandleItemActionRequest(int itemIndex)
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
            Debug.Log("Inventory UI Active: " + inventoryUI.isActiveAndEnabled);
            if (!inventoryUI.isActiveAndEnabled)
            {
                Debug.Log("Calling Show() on inventoryUI");
                inventoryUI.Show();
                StartCoroutine(CheckInventoryUIActive()); // Check active state after a short delay
            }
            else
            {
                Debug.Log("Calling Hide() on inventoryUI");
                inventoryUI.Hide();
                Debug.Log("After Hide() called, Inventory UI Active: " + inventoryUI.isActiveAndEnabled);
            }
        }
    }

    private IEnumerator CheckInventoryUIActive()
    {
        yield return new WaitForEndOfFrame(); // Wait for end of frame to ensure the active state is updated
        Debug.Log("After Show() called, Inventory UI Active: " + inventoryUI.isActiveAndEnabled);
        foreach (var item in inventoryData.GetCurrentInventoryState())
        {
            inventoryUI.UpdateData(
                item.Key,
                item.Value.item.ItemImage,
                item.Value.quantity);
        }
    }

    public Dictionary<int, InventoryItem> GetInventory()
    {
        return inventoryData.GetCurrentInventoryState();
    }

    public void RemoveItemFromInventory(PlantItemSO plantItem)
    {
        Dictionary<int, InventoryItem> inventory = GetInventory();
        foreach (var item in inventory)
        {
            if (item.Value.item == plantItem)
            {
                RemoveItem(item.Key, 1);
                break;
            }
        }
    }

    private void RemoveItem(int itemIndex, int quantity)
    {
        inventoryData.RemoveItem(itemIndex, quantity);
    }

    public void AddItem(ItemSO item, int quantity, float? quality = null)
    {
        if (quality.HasValue)
        {
            inventoryData.AddItem(item, quantity, quality.Value);
            Debug.Log($"Added item to inventory: {item.Name} (Quantity: {quantity}, Quality: {quality.Value})");
        }
        else
        {
            inventoryData.AddItem(item, quantity);
            Debug.Log($"Added item to inventory: {item.Name} (Quantity: {quantity})");
        }
    }
}
