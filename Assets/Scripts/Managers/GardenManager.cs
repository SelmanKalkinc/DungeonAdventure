using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public GameObject timerTextPrefab; // Assign this prefab in the inspector
    public Canvas uiCanvas; // Assign your existing canvas here in the inspector

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

        LoadGarden();
    }

    private void OnApplicationQuit()
    {
        SaveGarden();
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

            PlantCareRoutineSO careRoutine = Resources.Load<PlantCareRoutineSO>("CareRoutines/" + seedItem.careRoutine.name);
            if (careRoutine == null)
            {
                Debug.LogError("Failed to load CareRoutineSO: " + seedItem.careRoutine.name);
                return;
            }

            Debug.Log("Initializing PlantGrowth with careRoutine: " + careRoutine.name);
            growthComponent.Initialize(seedItem, careRoutine, this, gridPosition, uiCanvas, timerTextPrefab); // Pass the canvas and timer text prefab here
            plantedPlants.Add(newPlant);

            SaveGarden();
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
            plant.GetComponent<PlantGrowth>().ApplyAction(ActionType.Water);
            SaveGarden();
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
            plant.GetComponent<PlantGrowth>().ApplyAction(ActionType.Fertilize);
            SaveGarden();
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

                // Destroy the timer UI
                var timerText = plant.GetComponentInChildren<TMP_Text>();
                if (timerText != null)
                {
                    Destroy(timerText.gameObject);
                }

                SaveGarden();
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

    private void SaveGarden()
    {
        GardenData gardenData = new GardenData();

        foreach (var plant in plantedPlants)
        {
            PlantGrowth plantGrowth = plant.GetComponent<PlantGrowth>();
            if (plantGrowth != null)
            {
                PlantData plantData = new PlantData(
                    plantGrowth.plantData.name,
                    new SerializableVector3(plant.transform.position),
                    plantGrowth.plantedTime,
                    plantGrowth.growthStage,
                    plantGrowth.isDead,
                    plantGrowth.actionLogs
                );
                gardenData.plants.Add(plantData);
            }
        }

        SaveSystem.SaveData(gardenData, "gardenData.save");
    }

    private void LoadGarden()
    {
        GardenData gardenData = SaveSystem.LoadData<GardenData>("gardenData.save");
        if (gardenData == null)
        {
            Debug.LogError("Failed to load garden data.");
            return;
        }

        foreach (var plantData in gardenData.plants)
        {
            PlantItemSO plantItem = Resources.Load<PlantItemSO>("Plants/" + plantData.plantID);
            if (plantItem != null)
            {
                Vector3 position = plantData.position.ToVector3();
                PlantSeed(position, plantItem);
                GameObject plant = FindPlantAtPosition(position);
                if (plant != null)
                {
                    var plantGrowth = plant.GetComponent<PlantGrowth>();
                    plantGrowth.plantedTime = plantData.plantedTime;
                    plantGrowth.growthStage = plantData.growthStage;
                    plantGrowth.isDead = plantData.isDead;
                    plantGrowth.actionLogs = plantData.actionLogs;

                    if (plantGrowth.growthStage == 1)
                    {
                        plantGrowth.SetTileSprite(plantGrowth.plantData.HalfGrowthSprite);
                    }
                    else if (plantGrowth.growthStage == 2)
                    {
                        plantGrowth.SetTileSprite(plantGrowth.plantData.FullGrowthSprite);
                        plantGrowth.isHarvestable = true;
                    }

                    if (plantGrowth.isDead)
                    {
                        plantGrowth.SetTileSprite(plantGrowth.plantData.DeadSprite);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to load PlantItemSO with ID: " + plantData.plantID);
            }
        }
    }
}
