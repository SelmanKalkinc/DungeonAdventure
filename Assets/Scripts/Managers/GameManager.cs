using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject gameOverPanel; // Assign this in the Inspector
    public DungeonLevel SelectedDungeonLevel { get; private set; }

    [SerializeField] private InventorySO playerInventory; // Reference to the player's inventory

    private string inventoryFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            inventoryFilePath = Path.Combine(Application.persistentDataPath, "inventory.json");

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure the game over panel is inactive at the start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restart button clicked");
        //Time.timeScale = 1f; // Resume the game
        //DestroyAllEnemies(); // Destroy all enemies before restarting
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        StartDungeon();
    }

    public void GoToHouse()
    {
        Debug.Log("Go to House button clicked");
        Time.timeScale = 1f; // Resume the game
        DestroyAllEnemies(); // Destroy all enemies before loading the house scene
        SceneManager.LoadScene("HouseScene"); // Load the house scene
    }

    public void GoToGarden()
    {
        Debug.Log("Go to garden button clicked");
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("GardenScene"); // Load the house scene
    }

    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    public void SetSelectedDungeonLevel(DungeonLevel dungeonLevel)
    {
        SelectedDungeonLevel = dungeonLevel;
    }

    public void StartDungeon()
    {
        SaveGameState();
        SceneManager.LoadScene("DungeonScene"); // Replace with your dungeon scene name
    }

    public void SaveGameState()
    {
        SaveInventoryToFile();
        // Add other game state saving logic here if needed
    }

    private void SaveInventoryToFile()
    {
        Dictionary<int, InventoryItem> inventoryState = playerInventory.GetCurrentInventoryState();
        SerializableDictionary<int, InventoryItem> serializableInventory = new SerializableDictionary<int, InventoryItem>(inventoryState);
        string inventoryJson = JsonUtility.ToJson(serializableInventory);
        File.WriteAllText(inventoryFilePath, inventoryJson);
        Debug.Log($"Inventory saved to {inventoryFilePath}");
    }

    private void LoadInventoryFromFile()
    {
        if (File.Exists(inventoryFilePath))
        {
            string inventoryJson = File.ReadAllText(inventoryFilePath);
            SerializableDictionary<int, InventoryItem> serializableInventory = JsonUtility.FromJson<SerializableDictionary<int, InventoryItem>>(inventoryJson);
            playerInventory.SetCurrentInventoryState(serializableInventory.ToDictionary());
            Debug.Log("Inventory loaded from file");
        }
        else
        {
            Debug.Log("No saved inventory file found");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("onScene loaded");
        LoadInventoryFromFile(); // Load the inventory from file when a new scene is loaded
    }
}
