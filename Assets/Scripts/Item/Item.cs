using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab; // Prefab to instantiate when the item is dropped
    public string itemTooltip; // Add this line for the tooltip


    // Optional: Ensure the prefab has the correct sprite assigned at runtime
    public void InitializePrefab()
    {
        if (itemPrefab != null)
        {
            SpriteRenderer sr = itemPrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = itemIcon;
            }
            else
            {
                Debug.LogError($"SpriteRenderer not found on the prefab for item: {itemName}");
            }
        }
        else
        {
            Debug.LogError($"Item prefab not assigned for item: {itemName}");
        }
    }
}
