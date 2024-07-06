using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);
        // Notify UI or other systems here
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        // Notify UI or other systems here
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }
}
