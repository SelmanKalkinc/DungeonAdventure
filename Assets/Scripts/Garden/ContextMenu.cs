using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    public Button plantButton;
    public Button waterButton;
    public Button fertilizeButton;
    public Button harvestButton;

    private Vector3 currentPosition;
    private GardenManager gardenManager;

    public void Initialize(Vector3 position, GardenManager manager)
    {
        Debug.Log("Initializing ContextMenu");
        currentPosition = position;
        gardenManager = manager;

        if (plantButton == null)
        {
            Debug.LogError("Plant Button is not assigned in the Inspector");
        }
        if (waterButton == null)
        {
            Debug.LogError("Water Button is not assigned in the Inspector");
        }
        if (fertilizeButton == null)
        {
            Debug.LogError("Fertilize Button is not assigned in the Inspector");
        }
        if (harvestButton == null)
        {
            Debug.LogError("Harvest Button is not assigned in the Inspector");
        }

        GameObject plant = gardenManager.FindPlantAtPosition(position);
        if (plant != null)
        {
            Debug.Log("Plant found at position");

            plantButton.gameObject.SetActive(false);
            waterButton.gameObject.SetActive(true);
            fertilizeButton.gameObject.SetActive(true);
            harvestButton.gameObject.SetActive(true);

            waterButton.onClick.RemoveAllListeners();
            fertilizeButton.onClick.RemoveAllListeners();
            harvestButton.onClick.RemoveAllListeners();

            waterButton.onClick.AddListener(() => HandleWater());
            fertilizeButton.onClick.AddListener(() => HandleFertilize());
            harvestButton.onClick.AddListener(() => HandleHarvest());

            Debug.Log("Buttons activated: Water, Fertilize, Harvest");
        }
        else
        {
            Debug.Log("No plant found at position");

            plantButton.gameObject.SetActive(true);
            waterButton.gameObject.SetActive(false);
            fertilizeButton.gameObject.SetActive(false);
            harvestButton.gameObject.SetActive(false);

            plantButton.onClick.RemoveAllListeners();
            plantButton.onClick.AddListener(() => ShowPlantList());

            Debug.Log("Button activated: Plant");
        }
    }

    private void HandleWater()
    {
        Debug.Log("Watering plant at position: " + currentPosition);
        gardenManager.WaterPlant(currentPosition);
        Destroy(gameObject);
    }

    private void HandleFertilize()
    {
        Debug.Log("Fertilizing plant at position: " + currentPosition);
        gardenManager.FertilizePlant(currentPosition);
        Destroy(gameObject);
    }

    private void HandleHarvest()
    {
        Debug.Log("Harvesting plant at position: " + currentPosition);
        gardenManager.HarvestPlant(currentPosition);
        Destroy(gameObject);
    }

    private void ShowPlantList()
    {
        Debug.Log("Showing Plant List");

        if (PlantListMenu.Instance == null)
        {
            Debug.LogError("PlantListMenu instance is not found");
            return;
        }

        PlantListMenu.Instance.gameObject.SetActive(true);
        PlantListMenu.Instance.Initialize(currentPosition, gardenManager);
        Destroy(gameObject);
    }
}
