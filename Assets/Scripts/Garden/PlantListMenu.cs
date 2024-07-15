using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlantListMenu : MonoBehaviour
{
    public static PlantListMenu Instance { get; private set; }

    public Transform plantListContainer; // Should be the Content GameObject of the Scroll View
    public GameObject plantListItemPrefab;

    private Vector3 currentPosition;
    private GardenManager gardenManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false); // Start inactive
        }
    }

    public void Initialize(Vector3 position, GardenManager manager)
    {
        Debug.Log("Initializing PlantListMenu");
        currentPosition = position;
        gardenManager = manager;

        // Clear existing items
        foreach (Transform child in plantListContainer)
        {
            Destroy(child.gameObject);
        }

        InventoryController inventoryController = FindObjectOfType<InventoryController>();
        if (inventoryController == null)
        {
            Debug.LogError("InventoryController not found");
            return;
        }

        Dictionary<int, InventoryItem> inventory = inventoryController.GetInventory();
        if (inventory == null)
        {
            Debug.LogError("Inventory is null");
            return;
        }

        Dictionary<PlantItemSO, int> plantItems = new Dictionary<PlantItemSO, int>();

        // Group items by type and sum their quantities
        foreach (var item in inventory)
        {
            if (item.Value.item is PlantItemSO plantItem)
            {
                if (plantItems.ContainsKey(plantItem))
                {
                    plantItems[plantItem] += item.Value.quantity;
                }
                else
                {
                    plantItems[plantItem] = item.Value.quantity;
                }
            }
        }

        // Instantiate UI elements for each plant item type
        foreach (var plantItem in plantItems)
        {
            Debug.Log("Instantiating item: " + plantItem.Key.Name);
            GameObject listItem = Instantiate(plantListItemPrefab, plantListContainer);
            if (listItem == null)
            {
                Debug.LogError("Failed to instantiate plantListItemPrefab");
                continue;
            }

            // Ensure the item is active
            listItem.SetActive(true);

            // Correctly access the TMP_Text component
            TMP_Text listItemText = listItem.GetComponentInChildren<TMP_Text>();
            if (listItemText == null)
            {
                Debug.LogError("TMP_Text component not found on listItem");
                continue;
            }

            listItemText.text = $"{plantItem.Key.Name} x{plantItem.Value}";
            Button listItemButton = listItem.GetComponent<Button>();
            if (listItemButton == null)
            {
                Debug.LogError("Button component not found on listItem");
                continue;
            }

            Image itemIcon = listItem.transform.Find("ItemIcon").GetComponent<Image>();
            if (itemIcon == null)
            {
                Debug.LogError("ItemIcon component not found on listItem");
                continue;
            }

            itemIcon.sprite = plantItem.Key.ItemImage;

            listItemButton.onClick.AddListener(() => PlantSeed(plantItem.Key));

            Debug.Log("Instantiated list item: " + plantItem.Key.Name);
            Debug.Log("Item parent: " + listItem.transform.parent.name); // Check the parent
            Debug.Log("Item position: " + listItem.transform.position);
            Debug.Log("Item size: " + listItem.GetComponent<RectTransform>().rect.size);
        }

        Debug.Log("Total items instantiated: " + plantItems.Count);

        if (plantItems.Count == 0)
        {
            Debug.Log("No plantable items found in inventory.");
        }
    }

    private void PlantSeed(PlantItemSO plantItem)
    {
        gardenManager.PlantSeed(currentPosition, plantItem);
        FindObjectOfType<InventoryController>().RemoveItemFromInventory(plantItem);
        gameObject.SetActive(false); // Hide the plant list menu after planting
    }
}
