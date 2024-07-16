using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GardenManager : MonoBehaviour
{
    public GameObject contextMenuPrefab;
    public GameObject plantListMenuPrefab;
    public Tilemap soilTilemap;
    public Tile soilTile;
    public Tile defaultTile;
    private List<GameObject> plantedPlants = new List<GameObject>();
    private GameObject currentContextMenu;
    private InventoryController inventoryController;

    private void Start()
    {
        if (PlantListMenu.Instance == null)
        {
            Instantiate(plantListMenuPrefab);
        }

        inventoryController = FindObjectOfType<InventoryController>();
        if (inventoryController == null)
        {
            Debug.LogError("InventoryController not found in the scene.");
        }
    }

    public void HandleTileClick(Vector3 position)
    {
        if (currentContextMenu != null)
        {
            Destroy(currentContextMenu);
        }

        currentContextMenu = Instantiate(contextMenuPrefab, position, Quaternion.identity);
        var contextMenu = currentContextMenu.GetComponent<ContextMenu>();
        contextMenu.Initialize(position, this);
    }

    public void PlantSeed(Vector3 position, PlantItemSO seedItem)
    {
        Vector3Int gridPosition = soilTilemap.WorldToCell(position);
        soilTilemap.SetTile(gridPosition, defaultTile);

        if (seedItem.PlantPrefab != null)
        {
            GameObject newPlant = Instantiate(seedItem.PlantPrefab, position, Quaternion.identity);

            var growthComponent = newPlant.GetComponent<PlantGrowth>();
            if (growthComponent == null)
            {
                growthComponent = newPlant.AddComponent<PlantGrowth>();
            }

            growthComponent.Initialize(seedItem, this, gridPosition);
            plantedPlants.Add(newPlant);
        }
        else
        {
            Debug.LogError("The seed item does not have a plant prefab assigned.");
        }
    }

    public void WaterPlant(Vector3 position)
    {
        GameObject plant = FindPlantAtPosition(position);
        if (plant != null)
        {
            plant.GetComponent<PlantGrowth>().WaterPlant();
        }
        else
        {
            Debug.LogWarning("No plant found at position to water.");
        }
    }

    public void FertilizePlant(Vector3 position)
    {
        GameObject plant = FindPlantAtPosition(position);
        if (plant != null)
        {
            plant.GetComponent<PlantGrowth>().FertilizePlant();
        }
        else
        {
            Debug.LogWarning("No plant found at position to fertilize.");
        }
    }

    public void HarvestPlant(Vector3 position)
    {
        GameObject plant = FindPlantAtPosition(position);
        if (plant != null)
        {
            var plantGrowth = plant.GetComponent<PlantGrowth>();
            ItemSO harvestedItem = plantGrowth.Harvest();
            if (harvestedItem != null)
            {
                if (harvestedItem is HarvestableItemSO harvestableItem)
                {
                    float quality = plantGrowth.CalculateQuality();
                    inventoryController.AddItem(harvestableItem, 1, quality);
                }
                else
                {
                    inventoryController.AddItem(harvestedItem, 1); // Add the rotten item without quality
                }

                plantedPlants.Remove(plant);
                Destroy(plant);

                Vector3Int gridPosition = soilTilemap.WorldToCell(position);
                soilTilemap.SetTile(gridPosition, soilTile);
            }
        }
    }

    public GameObject FindPlantAtPosition(Vector3 position)
    {
        foreach (var plant in plantedPlants)
        {
            if (Vector3.Distance(plant.transform.position, position) < 0.1f)
            {
                return plant;
            }
        }
        return null;
    }

    public void UpdateTile(Vector3Int gridPosition, Tile tile)
    {
        soilTilemap.SetTile(gridPosition, tile);
    }

    public void ShowPlantList(Vector3 position)
    {
        if (PlantListMenu.Instance != null)
        {
            PlantListMenu.Instance.Initialize(position, this);
            PlantListMenu.Instance.gameObject.SetActive(true);
        }
    }
}
