using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantGrowth : MonoBehaviour
{
    private PlantItemSO plantData;
    private float growthTimer;
    private int waterCount;
    private int fertilizerCount;
    private float lastWaterTime;
    private bool isGrowing;
    private bool isHarvestable;
    private bool isDead;
    private GardenManager gardenManager;
    private Vector3Int gridPosition;

    public void Initialize(PlantItemSO plantData, GardenManager gardenManager, Vector3Int gridPosition)
    {
        this.plantData = plantData;
        this.gardenManager = gardenManager;
        this.gridPosition = gridPosition;
        growthTimer = 0;
        waterCount = 0;
        fertilizerCount = 0;
        lastWaterTime = -plantData.WaterInterval; // Ensures the first watering can happen immediately
        isGrowing = true;
        isHarvestable = false;
        isDead = false;
        StartCoroutine(GrowPlant());

        // Log to confirm initialization
        Debug.Log("PlantGrowth initialized with plantData: " + (plantData != null ? "Exists" : "Null"));
        Debug.Log("WaterInterval: " + plantData.WaterInterval);

        SetTileSprite(plantData.SeedSprite);
    }

    private IEnumerator GrowPlant()
    {
        while (isGrowing)
        {
            growthTimer += Time.deltaTime;

            // Check if the plant has reached half-growth
            if (growthTimer >= plantData.GrowthTime / 2)
            {
                SetTileSprite(plantData.HalfGrowthSprite);
            }

            // Check if the plant has grown to maturity
            if (growthTimer >= plantData.GrowthTime)
            {
                SetTileSprite(plantData.FullGrowthSprite);
                CompleteGrowth();
                yield break;
            }

            yield return null;
        }
    }

    public void WaterPlant()
    {
        Debug.Log("WaterPlant method called.");
        Debug.Log("PlantData: " + (plantData != null ? "Exists" : "Null"));
        Debug.Log("Time.time: " + Time.time);
        Debug.Log("LastWaterTime: " + lastWaterTime);
        Debug.Log("WaterInterval: " + (plantData != null ? plantData.WaterInterval.ToString() : "Null"));

        if (plantData != null && Time.time - lastWaterTime >= plantData.WaterInterval)
        {
            lastWaterTime = Time.time;
            waterCount++;
            Debug.Log("Plant watered. Total waterings: " + waterCount);
        }
        else
        {
            Debug.LogWarning("Watering too soon or plantData is null!");
        }
    }

    public void FertilizePlant()
    {
        fertilizerCount++;
        Debug.Log("Plant fertilized. Total fertilizers: " + fertilizerCount);
        // Add logic here to modify growth based on fertilization, if needed
    }

    private void CompleteGrowth()
    {
        isGrowing = false;
        isHarvestable = true;
        Debug.Log("Plant has fully grown!");
        StartCoroutine(CheckForDeath());
    }

    private IEnumerator CheckForDeath()
    {
        while (isHarvestable)
        {
            yield return new WaitForSeconds(plantData.GrowthTime); // Time until plant dies if not harvested
            if (isHarvestable)
            {
                isDead = true;
                isHarvestable = false;
                Debug.Log("Plant has died!");
                SetTileSprite(null); // Set the tile to an empty or dead plant sprite
                gardenManager.UpdateTile(gridPosition, null); // Optionally remove the tile
            }
        }
    }

    public HarvestableItemSO Harvest()
    {
        if (isHarvestable)
        {
            isHarvestable = false;
            float quality = CalculateQuality();
            Debug.Log("Harvesting plant with quality: " + quality);
            return CreateHarvestableItem(quality);
        }
        else if (isDead)
        {
            Debug.LogWarning("Cannot harvest a dead plant.");
        }
        return null;
    }

    public float CalculateQuality()
    {
        float maxQuality = 100f;
        float quality = maxQuality;

        // Example: decrease quality based on care conditions
        if (waterCount < plantData.RequiredWatering)
        {
            quality -= (plantData.RequiredWatering - waterCount) * 10f; // Adjust the penalty as needed
        }

        // Add more quality adjustments based on other care conditions (e.g., fertilization)

        return Mathf.Clamp(quality, 0f, maxQuality);
    }

    private HarvestableItemSO CreateHarvestableItem(float quality)
    {
        HarvestableItemSO harvestedItem = ScriptableObject.CreateInstance<HarvestableItemSO>();
        harvestedItem.Name = plantData.HarvestableItem.Name;
        harvestedItem.Description = plantData.HarvestableItem.Description;
        harvestedItem.ItemImage = plantData.HarvestableItem.ItemImage;
        harvestedItem.BaseQuality = quality; // Assign the calculated quality
        return harvestedItem;
    }

    private void SetTileSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            // Create a new tile with the specified sprite
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            gardenManager.UpdateTile(gridPosition, tile);
        }
        else
        {
            gardenManager.UpdateTile(gridPosition, null); // Optionally set to null or a specific sprite for dead plants
        }
    }
}
