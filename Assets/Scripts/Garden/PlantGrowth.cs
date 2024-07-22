using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System.Linq;

public class PlantGrowth : MonoBehaviour
{
    public PlantItemSO plantData;
    public PlantCareRoutineSO careRoutine;
    public DateTime plantedTime;
    public int growthStage; // 0 = Seed, 1 = Half-Growth, 2 = Full-Growth
    public bool isDead;
    public List<ActionLog> actionLogs = new List<ActionLog>();
    public float growthTimer;
    private GardenManager gardenManager;
    private Vector3Int gridPosition;
    public bool isHarvestable;

    private const float toleranceInSeconds = 5f; // Fixed tolerance value of 5 seconds
    private TMP_Text timerText; // Reference to the TMP_Text UI component

    private int currentActionIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    public void Initialize(PlantItemSO plantData, PlantCareRoutineSO careRoutine, GardenManager gardenManager, Vector3Int gridPosition, Canvas parentCanvas, GameObject timerTextPrefab)
    {
        this.plantData = plantData;
        this.careRoutine = careRoutine;
        this.gardenManager = gardenManager;
        this.gridPosition = gridPosition;
        this.plantedTime = DateTime.Now; // Ensures each plant has its own planted time
        this.growthStage = 0;
        this.isDead = false;
        this.actionLogs = new List<ActionLog>();
        this.growthTimer = 0;
        this.isHarvestable = false;

        Debug.Log("Initialize called for plant: " + plantData.name + " at " + plantedTime);

        StartCoroutine(GrowPlant());
        Debug.Log("PlantGrowth initialized with plantData: " + plantData.name);
        SetTileSprite(plantData.SeedSprite);

        // Instantiate and setup the timer UI under the parent canvas
        GameObject timerUI = Instantiate(timerTextPrefab, parentCanvas.transform);
        timerText = timerUI.GetComponent<TMP_Text>();
        if (timerText == null)
        {
            Debug.LogError("Timer UI TMP_Text component not found in the instantiated prefab!");
        }
        else
        {
            Debug.Log("Timer UI instantiated and assigned.");
            // Optionally adjust the position to follow the plant
            RectTransform rectTransform = timerText.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("RectTransform component not found on timer UI prefab!");
            }
            else
            {
                Vector3 worldPosition = transform.position + new Vector3(0, -1, 0); // Adjust the Y offset as needed
                rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
            }
        }
    }

    IEnumerator GrowPlant()
    {
        while (growthStage < 2)
        {
            float growthTime = plantData.GrowthTime / 2;
            yield return new WaitForSeconds(growthTime);

            growthStage++;
            Debug.Log($"Plant {plantData.name} at {gridPosition} grew to stage {growthStage} at {DateTime.Now}");

            if (growthStage == 1)
            {
                SetTileSprite(plantData.HalfGrowthSprite);
            }
            else if (growthStage == 2)
            {
                SetTileSprite(plantData.FullGrowthSprite);
                isHarvestable = true;
            }
        }
    }

    void Update()
    {
        if (timerText != null)
        {
            // Update the timer UI with the elapsed time
            TimeSpan elapsedTime = DateTime.Now - plantedTime;
            timerText.text = $"{elapsedTime.TotalSeconds:F0}"; // Display only the elapsed seconds as numbers

            // Adjust the position to follow the plant
            RectTransform rectTransform = timerText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Vector3 worldPosition = transform.position + new Vector3(0, -1, 0); // Adjust the Y offset as needed
                rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
            }
            else
            {
                Debug.LogError("RectTransform component not found on timerText!");
            }
        }
        else
        {
            Debug.LogError("timerText is null in Update!");
        }

        // Update growth timer
        growthTimer = (float)(DateTime.Now - plantedTime).TotalSeconds;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                currentActionIndex++;
            }
        }
        Debug.Log($"Plant {plantData.name} at stage {growthStage} with growth timer: {growthTimer}");

    }

    public void ApplyAction(ActionType actionType)
    {
        actionLogs.Add(new ActionLog(actionType, DateTime.Now));
        Debug.Log($"Action applied: {actionType} at {DateTime.Now}");
    }

    public ItemSO Harvest()
    {
        if (isHarvestable)
        {
            isHarvestable = false;
            float quality = CalculateQuality();
            GenerateReport(quality);

            // Destroy the timer UI
            if (timerText != null)
            {
                Destroy(timerText.gameObject);
            }

            return CreateHarvestableItem(quality);
        }
        else if (isDead)
        {
            return plantData.RottenHarvestableItem;
        }
        return null;
    }

    private void GenerateReport(float quality)
    {
        Debug.Log("===== Plant Care Report =====");
        Debug.Log($"Plant: {plantData.name}");
        Debug.Log($"Planted Time: {plantedTime}");
        Debug.Log($"Harvest Time: {DateTime.Now}");
        Debug.Log($"Final Quality: {quality}");

        Debug.Log("Expected Routine:");
        DateTime expectedActionTime = plantedTime;
        List<DateTime> expectedActionTimes = new List<DateTime>();

        foreach (var careAction in careRoutine.careActions)
        {
            if (careAction.actionType != ActionType.Wait)
            {
                expectedActionTimes.Add(expectedActionTime);
                Debug.Log($"Action: {careAction.actionType}, Time: {expectedActionTime}");
            }
            expectedActionTime = expectedActionTime.AddSeconds(careAction.waitTime);
        }

        Debug.Log("Actual Actions:");
        foreach (var log in actionLogs)
        {
            Debug.Log($"Action: {log.actionType}, Time: {log.timestamp}");
        }

        int correctActions = 0;
        List<string> incorrectActions = new List<string>();

        foreach (var log in actionLogs)
        {
            bool isCorrect = false;
            foreach (var expectedTime in expectedActionTimes)
            {
                double timeDiff = (log.timestamp - expectedTime).TotalSeconds;
                if (timeDiff >= -toleranceInSeconds && timeDiff <= toleranceInSeconds)
                {
                    correctActions++;
                    expectedActionTimes.Remove(expectedTime);
                    isCorrect = true;
                    break;
                }
            }
            if (!isCorrect)
            {
                incorrectActions.Add($"Unexpected action {log.actionType} at {log.timestamp}");
            }
        }

        Debug.Log($"Correct Actions: {correctActions} out of {careRoutine.careActions.Count(action => action.actionType != ActionType.Wait)}");

        if (incorrectActions.Count > 0)
        {
            Debug.Log("Incorrect Actions:");
            foreach (var incorrectAction in incorrectActions)
            {
                Debug.Log(incorrectAction);
            }
        }

        Debug.Log("===== End of Report =====");
    }

    public float CalculateQuality()
    {
        float maxQuality = 100f;
        float quality = maxQuality;
        int correctActions = 0;

        DateTime expectedActionTime = plantedTime;
        List<DateTime> expectedActionTimes = new List<DateTime>();

        foreach (var careAction in careRoutine.careActions)
        {
            if (careAction.actionType != ActionType.Wait)
            {
                expectedActionTimes.Add(expectedActionTime);
            }
            expectedActionTime = expectedActionTime.AddSeconds(careAction.waitTime);
        }

        foreach (var log in actionLogs)
        {
            foreach (var expectedTime in expectedActionTimes.ToList())
            {
                double timeDiff = (log.timestamp - expectedTime).TotalSeconds;
                if (timeDiff >= -toleranceInSeconds && timeDiff <= toleranceInSeconds)
                {
                    correctActions++;
                    expectedActionTimes.Remove(expectedTime);
                    break;
                }
            }
        }

        quality = (float)correctActions / (careRoutine.careActions.Count(action => action.actionType != ActionType.Wait)) * maxQuality;
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
