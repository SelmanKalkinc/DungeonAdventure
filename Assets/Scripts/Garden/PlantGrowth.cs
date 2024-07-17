using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantGrowth : MonoBehaviour
{
    public PlantItemSO plantData;
    public DateTime plantedTime;
    public int growthStage; // 0 = Seed, 1 = Half-Growth, 2 = Full-Growth
    public bool isDead;
    public List<DateTime> wateredTimes;
    public List<DateTime> fertilizedTimes;
    public float growthTimer;
    private GardenManager gardenManager;
    private Vector3Int gridPosition;
    public bool isHarvestable;

    public void Initialize(PlantItemSO plantData, GardenManager gardenManager, Vector3Int gridPosition)
    {
        this.plantData = plantData;
        this.gardenManager = gardenManager;
        this.gridPosition = gridPosition;
        this.plantedTime = DateTime.Now;
        this.growthStage = 0;
        this.isDead = false;
        this.wateredTimes = new List<DateTime>();
        this.fertilizedTimes = new List<DateTime>();
        growthTimer = 0;
        isHarvestable = false;

        StartCoroutine(GrowPlant());
        Debug.Log("PlantGrowth initialized with plantData: " + plantData.Name);
        SetTileSprite(plantData.SeedSprite);
    }

    public void Initialize(PlantItemSO plantData, GardenManager gardenManager, Vector3Int gridPosition, DateTime plantedTime, int growthStage, bool isDead, List<DateTime> wateredTimes, List<DateTime> fertilizedTimes)
    {
        this.plantData = plantData;
        this.gardenManager = gardenManager;
        this.gridPosition = gridPosition;
        this.plantedTime = plantedTime;
        this.growthStage = growthStage;
        this.isDead = isDead;
        this.wateredTimes = wateredTimes;
        this.fertilizedTimes = fertilizedTimes;

        float elapsedTime = (float)(DateTime.Now - plantedTime).TotalSeconds;

        if (growthStage == 1)
        {
            SetTileSprite(plantData.HalfGrowthSprite);
        }
        else if (growthStage == 2)
        {
            SetTileSprite(plantData.FullGrowthSprite);
            isHarvestable = true;
            if ((float)(DateTime.Now - plantedTime).TotalSeconds > plantData.GrowthTime + plantData.DeadTime)
            {
                isDead = true;
                SetTileSprite(plantData.DeadSprite);
            }
        }

        if (!isDead)
        {
            StartCoroutine(GrowPlant(elapsedTime));
        }

        Debug.Log("PlantGrowth initialized with plantData: " + plantData.Name);
    }

    private IEnumerator GrowPlant(float elapsedTime = 0)
    {
        while (!isDead)
        {
            growthTimer += Time.deltaTime;

            if (growthStage == 0 && elapsedTime + growthTimer >= plantData.GrowthTime / 2)
            {
                SetTileSprite(plantData.HalfGrowthSprite);
                growthStage = 1;
            }

            if (growthStage == 1 && elapsedTime + growthTimer >= plantData.GrowthTime)
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
        if (plantData != null && (DateTime.Now - (wateredTimes.Count > 0 ? wateredTimes[wateredTimes.Count - 1] : DateTime.MinValue)).TotalSeconds >= plantData.WaterInterval)
        {
            wateredTimes.Add(DateTime.Now);
            Debug.Log("Plant watered. Total waterings: " + wateredTimes.Count);
        }
        else
        {
            Debug.LogWarning("Watering too soon or plantData is null!");
        }
    }

    public void FertilizePlant()
    {
        fertilizedTimes.Add(DateTime.Now);
        Debug.Log("Plant fertilized. Total fertilizers: " + fertilizedTimes.Count);
    }

    private void CompleteGrowth()
    {
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

        if (wateredTimes.Count < plantData.RequiredWatering)
        {
            quality -= (plantData.RequiredWatering - wateredTimes.Count) * 10f;
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

    public void SetTileSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            gardenManager.UpdateTile(gridPosition, tile);
        }
    }
}
