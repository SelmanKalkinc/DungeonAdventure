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
        this.plantedTime = DateTime.Now; // Initialize with current time if not loaded
        this.growthStage = 0;
        this.isDead = false;
        this.actionLogs = new List<ActionLog>();
        this.growthTimer = 0;
        this.isHarvestable = false;

        Debug.Log("Initialize called for plant: " + plantData.name + " at " + plantedTime);

        CalculateGrowthStageAndState();

        StartCoroutine(CheckGrowthAndDeath());
        Debug.Log("PlantGrowth initialized with plantData: " + plantData.name);
        SetTileSprite(plantData.SeedSprite);

        // Instantiate and setup the timer UI under the parent canvas
        GameObject timerUI = Instantiate(timerTextPrefab, parentCanvas.transform);
        timerText = timerUI.GetComponent<TMP_Text>();
        if (timerText == null)
        {
            Debug.LogError("Timer Text component is missing on the Timer UI prefab.");
        }
    }

    public void InitializeLoadedPlant(PlantItemSO plantData, PlantCareRoutineSO careRoutine, GardenManager gardenManager, Vector3Int gridPosition, DateTime plantedTime, int growthStage, bool isDead, List<ActionLog> actionLogs, Canvas parentCanvas, GameObject timerTextPrefab)
    {
        this.plantData = plantData;
        this.careRoutine = careRoutine;
        this.gardenManager = gardenManager;
        this.gridPosition = gridPosition;
        this.plantedTime = plantedTime;
        this.growthStage = growthStage;
        this.isDead = isDead;
        this.actionLogs = actionLogs;
        this.growthTimer = (float)(DateTime.Now - plantedTime).TotalSeconds;
        this.isHarvestable = (growthStage == 2) && !isDead;

        Debug.Log("InitializeLoadedPlant called for plant: " + plantData.name + " at " + plantedTime);

        CalculateGrowthStageAndState();

        StartCoroutine(CheckGrowthAndDeath());
        Debug.Log("PlantGrowth initialized with plantData: " + plantData.name);

        // Check if timerText already exists to avoid duplicates
        timerText = GetComponentInChildren<TMP_Text>();
        if (timerText == null)
        {
            // Instantiate and setup the timer UI under the parent canvas
            GameObject timerUI = Instantiate(timerTextPrefab, parentCanvas.transform);
            timerText = timerUI.GetComponent<TMP_Text>();
            if (timerText == null)
            {
                Debug.LogError("Timer Text component is missing on the Timer UI prefab.");
            }
        }
    }

    private IEnumerator CheckGrowthAndDeath()
    {
        while (true)
        {
            UpdateGrowthTimer();
            CheckGrowthStage();
            CheckIfDead();
            UpdateTimerText();
            yield return new WaitForSeconds(1); // Check every second
        }
    }

    private void CalculateGrowthStageAndState()
    {
        float halfGrowthTime = plantData.GrowthTime / 2;
        float fullGrowthTime = plantData.GrowthTime;
        float deadTime = plantData.DeadTime;

        growthTimer = (float)(DateTime.Now - plantedTime).TotalSeconds;

        if (growthTimer >= fullGrowthTime && growthTimer < deadTime)
        {
            growthStage = 2;
            isHarvestable = true;
            isDead = false;
        }
        else if (growthTimer >= halfGrowthTime && growthTimer < fullGrowthTime)
        {
            growthStage = 1;
            isHarvestable = false;
            isDead = false;
        }
        else if (growthTimer >= deadTime)
        {
            isDead = true;
            growthStage = 2; // Set to full growth but dead
            isHarvestable = false;
        }

        UpdatePlantSprite();
    }

    private void UpdateGrowthTimer()
    {
        growthTimer = (float)(DateTime.Now - plantedTime).TotalSeconds;
    }

    private void CheckGrowthStage()
    {
        float halfGrowthTime = plantData.GrowthTime / 2;
        float fullGrowthTime = plantData.GrowthTime;

        if (growthTimer >= fullGrowthTime && growthStage < 2)
        {
            growthStage = 2;
            isHarvestable = true;
            SetTileSprite(plantData.FullGrowthSprite);
            Debug.Log($"Plant {plantData.name} at {gridPosition} grew to full growth stage at {DateTime.Now}");
        }
        else if (growthTimer >= halfGrowthTime && growthStage < 1)
        {
            growthStage = 1;
            SetTileSprite(plantData.HalfGrowthSprite);
            Debug.Log($"Plant {plantData.name} at {gridPosition} grew to half growth stage at {DateTime.Now}");
        }
    }

    private void CheckIfDead()
    {
        if (growthTimer >= plantData.DeadTime)
        {
            isDead = true;
            SetTileSprite(plantData.DeadSprite);
            Debug.Log("Plant is dead: " + plantData.name);
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            // Displaying the growth timer in seconds without milliseconds
            timerText.text = Mathf.Floor(growthTimer).ToString() + "s";
        }
    }

    private void UpdatePlantSprite()
    {
        if (isDead)
        {
            SetTileSprite(plantData.DeadSprite);
        }
        else if (growthStage == 2)
        {
            SetTileSprite(plantData.FullGrowthSprite);
        }
        else if (growthStage == 1)
        {
            SetTileSprite(plantData.HalfGrowthSprite);
        }
        else
        {
            SetTileSprite(plantData.SeedSprite);
        }
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

    void Update()
    {
        UpdateTimerText();

        // Adjust the timer text position to follow the plant
        if (timerText != null)
        {
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

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                currentActionIndex++;
            }
        }
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
}
