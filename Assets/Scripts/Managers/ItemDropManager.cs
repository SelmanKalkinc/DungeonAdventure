using UnityEngine;
using System.Collections.Generic;

public class ItemDropManager : MonoBehaviour
{
    public static ItemDropManager Instance { get; private set; }

    public Vector3 itemScale = new Vector3(1f, 1f, 1f);
    public float dropRadiusPixels = 6.0f; // Radius of the circle in pixels

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void DropItems(DropTable dropTable, Vector3 position)
    {
        List<GameObject> droppedItems = new List<GameObject>();

        foreach (var dropItem in dropTable.dropItems)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= dropItem.dropChance)
            {
             //   GameObject itemInstance = Instantiate(dropItem.item.itemPrefab, position, Quaternion.identity);
             //
             //   // Set the scale to fit within a 6-pixel radius
             //   float scaleFactor = GetScaleFactorForPixels(6.0f);
             //   itemInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
             //
             //   SpriteRenderer sr = itemInstance.GetComponent<SpriteRenderer>();
             //   if (sr == null)
             //   {
             //       Debug.LogError("SpriteRenderer component not found on the item prefab.");
             //   }
             //   else if (sr.sprite == null)
             //   {
             //       //sr.sprite = dropItem.item.itemIcon;
             //       if (sr.sprite == null)
             //       {
             //           Debug.LogWarning("SpriteRenderer does not have a sprite assigned and no icon is available in the item.");
             //       }
             //       else
             //       {
             //           Debug.Log($"Assigned sprite: {sr.sprite.name}");
             //       }
             //   }
             //   else
             //   {
             //       Debug.Log($"Item sprite: {sr.sprite.name}");
             //   }
             //
             //   droppedItems.Add(itemInstance);
            }
        }

        PositionItemsInCircle(droppedItems, position);
    }

    private void PositionItemsInCircle(List<GameObject> items, Vector3 center)
    {
        int itemCount = items.Count;
        if (itemCount == 0) return;

        float unitsPerPixel = GetUnitsPerPixel();
        float radiusUnits = dropRadiusPixels * unitsPerPixel;

        float angleStep = 360f / itemCount;

        for (int i = 0; i < itemCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 dropPosition = new Vector3(
                center.x + Mathf.Cos(angle) * radiusUnits,
                center.y + Mathf.Sin(angle) * radiusUnits,
                center.z
            );

            items[i].transform.position = dropPosition;
        }
    }

    private float GetUnitsPerPixel()
    {
        Camera cam = Camera.main;
        float screenHeight = Screen.height;
        float unitsPerPixel = cam.orthographicSize * 2 / screenHeight;
        return unitsPerPixel;
    }

    private float GetScaleFactorForPixels(float targetPixels)
    {
        Camera cam = Camera.main;
        float screenHeight = Screen.height;
        float unitsPerPixel = cam.orthographicSize * 2 / screenHeight;
        float scaleFactor = targetPixels * unitsPerPixel;
        return scaleFactor;
    }
}
