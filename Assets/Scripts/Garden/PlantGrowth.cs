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

        Debug.Log("PlantGrowth initialized with plantData: " + plantData.Name);
        SetTileSprite(plantData.SeedSprite);
    }

    private IEnumerator GrowPlant()
    {
        while (isGrowing)
        {
            growthTimer += Time.deltaTime;

            if (growthTimer >= plantData.GrowthTime / 2 && !isHarvestable)
            {
                SetTileSprite(plantData.HalfGrowthSprite);
            }

            if (growthTimer >= plantData.GrowthTime && !isHarvestable)
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
    }

    private void CompleteGrowth()
    {
        isGrowing = false;
        isHarvestable = true;
        StartCoroutine(CheckForDeath());
    }

    private IEnumerator CheckForDeath()
    {
        yield return new WaitForSeconds(plantData.DeadTime);
        if (isHarvestable)
        {
            isDead = true;
            isHarvestable = false;
            SetTileSprite(plantData.DeadSprite);
        }
    }

    public ItemSO Harvest()
    {
        if (isHarvestable)
        {
            isHarvestable = false;
            float quality = CalculateQuality();
            return CreateHarvestableItem(quality);
        }
        else if (isDead)
        {
            return plantData.RottenHarvestableItem;
        }
        return null;
    }

    public float CalculateQuality()
    {
        float maxQuality = 100f;
        float quality = maxQuality;

        if (waterCount < plantData.RequiredWatering)
        {
            quality -= (plantData.RequiredWatering - waterCount) * 10f;
        }

        return Mathf.Clamp(quality, 0f, maxQuality);
    }

    private HarvestableItemSO CreateHarvestableItem(float quality)
    {
        HarvestableItemSO harvestedItem = ScriptableObject.CreateInstance<HarvestableItemSO>();
        harvestedItem.Name = plantData.HarvestableItem.Name;
        harvestedItem.Description = plantData.HarvestableItem.Description;
        harvestedItem.ItemImage = plantData.HarvestableItem.ItemImage;
        harvestedItem.BaseQuality = quality;
        return harvestedItem;
    }

    private void SetTileSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            gardenManager.UpdateTile(gridPosition, tile);
        }
    }
}
