using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    public Button waterButton;
    public Button fertilizeButton;
    public Button harvestButton;
    public Button plantButton; // Add this line

    private Vector3 position;
    private GardenManager gardenManager;

    public void Initialize(Vector3 position, GardenManager gardenManager)
    {
        this.position = position;
        this.gardenManager = gardenManager;

        Debug.Log("Initializing ContextMenu at position: " + position);

        if (waterButton == null || fertilizeButton == null || harvestButton == null || plantButton == null)
        {
            Debug.LogError("Buttons are not assigned in the Inspector!");
            return;
        }

        waterButton.onClick.RemoveAllListeners();
        fertilizeButton.onClick.RemoveAllListeners();
        harvestButton.onClick.RemoveAllListeners();
        plantButton.onClick.RemoveAllListeners();

        waterButton.onClick.AddListener(HandleWater);
        fertilizeButton.onClick.AddListener(HandleFertilize);
        harvestButton.onClick.AddListener(HandleHarvest);
        plantButton.onClick.AddListener(HandlePlant); // Add this line

        // Check if there is a plant at the position
        GameObject plant = gardenManager.FindPlantAtPosition(position);
        if (plant == null)
        {
            Debug.Log("No plant found at position");
            waterButton.gameObject.SetActive(false);
            fertilizeButton.gameObject.SetActive(false);
            harvestButton.gameObject.SetActive(false);
            plantButton.gameObject.SetActive(true); // Show plant button if no plant is found
        }
        else
        {
            Debug.Log("Plant found at position");
            waterButton.gameObject.SetActive(true);
            fertilizeButton.gameObject.SetActive(true);
            harvestButton.gameObject.SetActive(true);
            plantButton.gameObject.SetActive(false); // Hide plant button if a plant is found
        }
    }

    private void HandleWater()
    {
        Debug.Log($"Watering plant at position: {position}");
        gardenManager.WaterPlant(position);
        Destroy(gameObject);
    }

    private void HandleFertilize()
    {
        Debug.Log($"Fertilizing plant at position: {position}");
        gardenManager.FertilizePlant(position);
        Destroy(gameObject);
    }

    private void HandleHarvest()
    {
        Debug.Log($"Harvesting plant at position: {position}");
        gardenManager.HarvestPlant(position);
        Destroy(gameObject);
    }

    private void HandlePlant()
    {
        Debug.Log($"Planting at position: {position}");
        gardenManager.ShowPlantList(position); // Show plant list to select which seed to plant
        Destroy(gameObject);
    }
}
