using System.Collections.Generic;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    public GameObject contextMenuPrefab; // Prefab for the context menu
    public GameObject plantListMenuPrefab; // Prefab for the plant list menu

    private List<GameObject> plantedPlants = new List<GameObject>();
    private GameObject currentContextMenu;

    private void Start()
    {
        if (PlantListMenu.Instance == null)
        {
            Instantiate(plantListMenuPrefab);
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
        if (seedItem.PlantPrefab != null)
        {
            GameObject newPlant = Instantiate(seedItem.PlantPrefab, position, Quaternion.identity);
            var growthComponent = newPlant.AddComponent<FruitGrowth>();
            growthComponent.Initialize(seedItem);
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
            plant.GetComponent<FruitGrowth>().WaterPlant();
        }
    }

    public void FertilizePlant(Vector3 position)
    {
        GameObject plant = FindPlantAtPosition(position);
        if (plant != null)
        {
            plant.GetComponent<FruitGrowth>().FertilizePlant();
        }
    }

    public void HarvestPlant(Vector3 position)
    {
        GameObject plant = FindPlantAtPosition(position);
        if (plant != null)
        {
            plantedPlants.Remove(plant);
            Destroy(plant);
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
}
