using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    public int currentExperience = 0;
    public int currentLevel = 1;

    [Header("UI Elements")]
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText; // Level text for displaying current level
    public Slider expSlider;

    public LevelExperienceData levelExperienceData;

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this instance persists across scenes
            saveFilePath = Path.Combine(Application.persistentDataPath, "playerExpData.json");
            Debug.Log($"Save file path: {saveFilePath}");
            LoadPlayerData(); // Load player data when the game starts
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI(); // Update the UI when the game starts
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the sceneLoaded event
        SavePlayerData(); // Save data when the scene changes
    }

    private void OnApplicationQuit()
    {
        SavePlayerData(); // Save data when the application quits
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReassignUIReferences(); // Reassign UI references when a new scene is loaded
        UpdateUI(); // Update the UI when a new scene is loaded
    }

    public void AddExperience(int exp)
    {
        currentExperience += exp;
        int experienceToNextLevel = levelExperienceData.GetExperienceForLevel(currentLevel);
        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
        UpdateUI();
        SavePlayerData(); // Save data after gaining experience
    }

    private void LevelUp()
    {
        int experienceToNextLevel = levelExperienceData.GetExperienceForLevel(currentLevel);
        currentExperience -= experienceToNextLevel;
        currentLevel++;
        Debug.Log("Level Up! New Level: " + currentLevel);
        UpdateUI();
        SavePlayerData(); // Save data after leveling up
    }

    private void UpdateUI()
    {
        Debug.Log($"Updating UI: Current EXP: {currentExperience}, Current Level: {currentLevel}");
        int experienceToNextLevel = levelExperienceData.GetExperienceForLevel(currentLevel);
        if (expText != null)
        {
            expText.text = $"EXP: {currentExperience}/{experienceToNextLevel}";
            Debug.Log("Experience text updated.");
        }
        if (levelText != null)
        {
            levelText.text = $"Level: {currentLevel}";
            Debug.Log("Level text updated.");
        }
        if (expSlider != null)
        {
            expSlider.maxValue = experienceToNextLevel;
            expSlider.value = currentExperience;
            Debug.Log("Experience slider updated.");
        }
    }

    private void ReassignUIReferences()
    {
        if (expText == null)
        {
            expText = GameObject.Find("ExpText")?.GetComponent<TextMeshProUGUI>();
            if (expText == null) Debug.LogWarning("ExpText not found");
        }
        if (levelText == null)
        {
            levelText = GameObject.Find("LevelText")?.GetComponent<TextMeshProUGUI>();
            if (levelText == null) Debug.LogWarning("LevelText not found");
        }
        if (expSlider == null)
        {
            expSlider = GameObject.Find("ExpSlider")?.GetComponent<Slider>();
            if (expSlider == null) Debug.LogWarning("ExpSlider not found");
        }
    }

    public void SavePlayerData()
    {
        PlayerExpData data = new PlayerExpData
        {
            currentExperience = currentExperience,
            currentLevel = currentLevel,
            experienceToNextLevel = levelExperienceData.GetExperienceForLevel(currentLevel)
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Player data saved: {json}");
    }

    public void LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerExpData data = JsonUtility.FromJson<PlayerExpData>(json);

            Debug.Log($"Loaded data from file: {json}");
            currentExperience = data.currentExperience;
            currentLevel = data.currentLevel;
            UpdateUI(); // Update the UI after loading data
        }
        else
        {
            Debug.LogWarning("Save file not found. Starting with default values.");
        }
    }

    public void SetPlayerData(PlayerExpData data)
    {
        currentExperience = data.currentExperience;
        currentLevel = data.currentLevel;
        UpdateUI();
        Debug.Log($"Player data set: {JsonUtility.ToJson(data)}");
    }
}
