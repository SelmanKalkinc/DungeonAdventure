using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject gameOverPanel; // Assign this in the Inspector
    public DungeonLevel SelectedDungeonLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        SceneManager.LoadScene("DungeonScene"); // Replace with your dungeon scene name
    }
}
