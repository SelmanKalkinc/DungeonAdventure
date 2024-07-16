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
        if (inventoryItems == null || inventoryItems.Count == 0)
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }
    }

    public int AddItem(ItemSO item, int quantity, float? quality = null)
    {
        if (!item.IsStackable)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                while (quantity > 0 && !IsInventoryFull())
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1, quality);
                }
                InformAboutChange();
                return quantity;
            }
        }

        quantity = AddStackableItem(item, quantity, quality);
        InformAboutChange();
        return quantity;
    }

    private int AddItemToFirstFreeSlot(ItemSO item, int quantity, float? quality)
    {
        InventoryItem newItem = new InventoryItem
        {
            item = item,
            quantity = quantity,
            quality = quality
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

    private bool IsInventoryFull() => inventoryItems.All(item => !item.IsEmpty);

    private int AddStackableItem(ItemSO item, int quantity, float? quality)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                continue;
            }
            if (inventoryItems[i].item.ID == item.ID)
            {
                int amountPossibleToTake = item.MaxStackSize - inventoryItems[i].quantity;

                if (quantity > amountPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(item.MaxStackSize);
                    quantity -= amountPossibleToTake;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                    InformAboutChange();
                    return 0;
                }
            }
        }

        while (quantity > 0 && !IsInventoryFull())
        {
            int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
            quantity -= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity, quality);
        }
        return quantity;
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (!inventoryItems[i].IsEmpty)
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
        AddItem(item.item, item.quantity, item.quality);
    }

    public void SwapItems(int itemIndex_1, int itemIndex_2)
    {
        InventoryItem item1 = inventoryItems[itemIndex_1];
        inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
        inventoryItems[itemIndex_2] = item1;
        InformAboutChange();
    }

    public void MergeItems(int fromIndex, int toIndex)
    {
        InventoryItem fromItem = inventoryItems[fromIndex];
        InventoryItem toItem = inventoryItems[toIndex];

        if (fromItem.item.ID == toItem.item.ID && fromItem.item.IsStackable)
        {
            int totalQuantity = fromItem.quantity + toItem.quantity;
            int maxStackSize = fromItem.item.MaxStackSize;

            if (totalQuantity <= maxStackSize)
            {
                inventoryItems[toIndex] = toItem.ChangeQuantity(totalQuantity);
                inventoryItems[fromIndex] = InventoryItem.GetEmptyItem();
            }
            else
            {
                inventoryItems[toIndex] = toItem.ChangeQuantity(maxStackSize);
                inventoryItems[fromIndex] = fromItem.ChangeQuantity(totalQuantity - maxStackSize);
            }

            InformAboutChange();
        }
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
    public float? quality;
    public bool IsEmpty => item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity,
            quality = this.quality
        };
    }

    public static InventoryItem GetEmptyItem() => new InventoryItem
    {
        item = null,
        quantity = 0,
        quality = null
    };
}
