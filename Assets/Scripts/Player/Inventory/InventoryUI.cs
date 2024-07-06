using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform itemsParent;
    public GameObject inventorySlotPrefab;

    private List<InventorySlot> slots = new List<InventorySlot>();

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        UpdateUI();
    }

    private void AddItemToUI(Item item)
    {
        GameObject newSlot = Instantiate(inventorySlotPrefab, itemsParent);
        InventorySlot slot = newSlot.GetComponent<InventorySlot>();
        slot.SetItem(item);
        slots.Add(slot);
    }

    public void UpdateUI()
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventory.items)
        {
            AddItemToUI(item);
        }
    }
}
