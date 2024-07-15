using System.Collections;
using UnityEngine;

public class FruitGrowth : MonoBehaviour
{
    private PlantItemSO plantItem;
    private int currentStage = 0;
    private int wateringCount = 0;
    private bool wateredOnTime = true;

    public void Initialize(PlantItemSO plantItem)
    {
        this.plantItem = plantItem;
        StartCoroutine(Grow());
        StartCoroutine(WateringRoutine());
    }

    private IEnumerator Grow()
    {
        while (currentStage < plantItem.GrowthStages.Length - 1)
        {
            yield return new WaitForSeconds(plantItem.GrowthTime);
            if (wateredOnTime)
            {
                AdvanceGrowth();
            }
            wateredOnTime = true; // Reset the condition for the next stage
        }
    }

    private IEnumerator WateringRoutine()
    {
        while (currentStage < plantItem.GrowthStages.Length - 1)
        {
            yield return new WaitForSeconds(plantItem.WaterInterval);
            wateredOnTime = false;
        }
    }

    private void AdvanceGrowth()
    {
        if (currentStage < plantItem.GrowthStages.Length - 1)
        {
            currentStage++;
            UpdatePlant();
        }
    }

    private void UpdatePlant()
    {
        for (int i = 0; i < plantItem.GrowthStages.Length; i++)
        {
            plantItem.GrowthStages[i].SetActive(i == currentStage);
        }
    }

    public void WaterPlant()
    {
        if (wateringCount < plantItem.RequiredWatering)
        {
            wateringCount++;
            wateredOnTime = true;
        }
        else
        {
            wateredOnTime = false;
        }
    }

    public void FertilizePlant()
    {
        // Add fertilizer logic here if needed
    }

    public GameObject GetQuality()
    {
        if (wateringCount >= plantItem.RequiredWatering)
        {
            return plantItem.QualityStages[2]; // High Quality
        }
        else if (wateringCount >= plantItem.RequiredWatering / 2)
        {
            return plantItem.QualityStages[1]; // Medium Quality
        }
        else
        {
            return plantItem.QualityStages[0]; // Low Quality
        }
    }
}
