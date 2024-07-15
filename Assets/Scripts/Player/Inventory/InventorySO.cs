using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Inventory/Inventory", order = 1)]
public class InventorySO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> inventoryItems;

    [SerializeField]
    public int Size { get; set; } = 10;

    public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

    public void Initialize()
{
    if (inventoryItems == null)
    {
        inventoryItems = new List<InventoryItem>();
    }
    
    if (inventoryItems.Count == 0)
    {
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }
}


    public int AddItem(ItemSO item, int quantity)
    {
        if (item.IsStackable == false)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                while (quantity > 0 && IsInventoryFull() == false)
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1);
                }
                InformAboutChange();
                return quantity;
            }
        }

        quantity = AddStackableItem(item, quantity);
        InformAboutChange();
        return quantity;
    }

    private int AddItemToFirstFreeSlot(ItemSO item, int quantity)
    {
        InventoryItem newItem = new InventoryItem
        {
            item = item,
            quantity = quantity
        };

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = newItem;
                return quantity;
            }
        }
        return 0;
    }

    private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;

    private int AddStackableItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                continue;
            }
            if (inventoryItems[i].item.ID == item.ID)
            {
                int amountOfPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                if (quantity > amountOfPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                    quantity -= amountOfPossibleToTake;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                    InformAboutChange();
                    return 0;
                }
            }
        }

        while (quantity > 0 && IsInventoryFull() == false)
        {
            int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
            quantity -= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity);
        }
        return quantity;
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                continue;
            }
            else
            {
                returnValue[i] = inventoryItems[i];
            }
        }
        return returnValue;
    }

    public void SetCurrentInventoryState(Dictionary<int, InventoryItem> inventoryState)
    {
        inventoryItems = new List<InventoryItem>(new InventoryItem[Size]);
        foreach (var item in inventoryState)
        {
            inventoryItems[item.Key] = item.Value;
        }
        InformAboutChange();
    }

    internal InventoryItem GetItemAt(int itemIndex)
    {
        return inventoryItems[itemIndex];
    }

    internal void AddItem(InventoryItem item)
    {
        AddItem(item.item, item.quantity);
    }

    public void SwapItems(int itemIndex_1, int itemIndex_2)
    {
        InventoryItem item1 = inventoryItems[itemIndex_1];
        inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
        inventoryItems[itemIndex_2] = item1;
        InformAboutChange();
    }

    private void InformAboutChange()
    {
        OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    }

    public void RemoveItem(int itemIndex, int amount)
    {
        if (inventoryItems.Count > itemIndex)
        {
            if (inventoryItems[itemIndex].IsEmpty)
            {
                return;
            }
            int reminder = inventoryItems[itemIndex].quantity - amount;
            if (reminder <= 0)
            {
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            }
            else
            {
                inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder);
            }
            InformAboutChange();
        }
    }
}

[Serializable]
public struct InventoryItem
{
    public ItemSO item;
    public int quantity;
    public bool IsEmpty => item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity
        };
    }
    public static InventoryItem GetEmptyItem() => new InventoryItem
    {
        item = null,
        quantity = 0
    };
}
