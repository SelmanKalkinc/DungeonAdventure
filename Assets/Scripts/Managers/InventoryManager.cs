using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public GameObject inventoryUIPrefab; // Reference to the Inventory UI prefab
    private GameObject inventoryUI;
    private bool isInventoryOpen = false;

    void Awake()
    {
        Debug.Log("InventoryManager Awake called");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (inventoryUIPrefab != null)
            {
                Debug.Log("Instantiating Inventory UI Prefab");
                inventoryUI = Instantiate(inventoryUIPrefab);
                if (inventoryUI == null)
                {
                    Debug.LogError("Inventory UI Prefab instantiation failed");
                }
                else
                {
                    Debug.Log("Inventory UI Prefab instantiated successfully");
                    inventoryUI.SetActive(false); // Start with the inventory UI hidden

                    // Ensure inventoryUI is properly parented
                    Canvas canvas = FindObjectOfType<Canvas>();
                    if (canvas != null)
                    {
                        inventoryUI.transform.SetParent(canvas.transform, false);
                        Debug.Log("Inventory UI parented to Canvas");
                    }
                    else
                    {
                        Debug.LogError("No Canvas found in the scene");
                    }

                    DontDestroyOnLoad(inventoryUI);

                    InventoryUI inventoryUIComponent = inventoryUI.GetComponent<InventoryUI>();
                    if (inventoryUIComponent == null)
                    {
                        Debug.LogError("InventoryUI component not found on Inventory UI prefab");
                    }
                    else
                    {
                        inventoryUIComponent.inventory = GetComponent<Inventory>();
                        Debug.Log("InventoryUI component assigned");
                    }
                }
            }
            else
            {
                Debug.LogError("Inventory UIPrefab is not assigned in the Inspector");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I key pressed");
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryUI == null)
        {
            Debug.LogError("Inventory UI is null, cannot toggle inventory");
            return;
        }

        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);
        Debug.Log($"Inventory UI Active State: {inventoryUI.activeSelf}");

        if (isInventoryOpen)
        {
            Debug.Log("Inventory opened");
            // Update the UI with the current inventory items
            InventoryUI inventoryUIComponent = inventoryUI.GetComponent<InventoryUI>();
            if (inventoryUIComponent != null)
            {
                inventoryUIComponent.UpdateUI();
            }
            else
            {
                Debug.LogError("InventoryUI component not found on Inventory UI GameObject");
            }
        }
        else
        {
            Debug.Log("Inventory closed");
        }
    }
}
