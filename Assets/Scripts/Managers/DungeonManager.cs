using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }
    public SimpleRandomWalkDungeonGenerator dungeonGenerator;
    public TilemapVisualizer tilemapVisualizer;
    public Transform player;
    public Transform enemyParent;
    public EnemyManager enemyManager;
    public GameObject gameOverPanel; // Reference to the Game Over panel

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DungeonLevel selectedLevel = GameManager.Instance.SelectedDungeonLevel;
        if (selectedLevel != null)
        {
            StartDungeon(selectedLevel);
        }
        else
        {
            Debug.LogError("No dungeon level selected.");
        }

        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.OnDied += ShowGameOverPanel;
            }
        }
    }

    public void StartDungeon(DungeonLevel dungeonLevel)
    {
        Debug.Log($"Starting dungeon: Level {dungeonLevel.level}");
        // Use the properties of dungeonLevel to generate the dungeon
        GenerateDungeon(dungeonLevel);
    }

    private void GenerateDungeon(DungeonLevel dungeonLevel)
    {
        Debug.Log($"Generating dungeon with width: {dungeonLevel.dungeonWidth}, height: {dungeonLevel.dungeonHeight}, enemy count: {dungeonLevel.enemyCount}");
        SpawnPositions spawnPositions = dungeonGenerator.RunProceduralGeneration(tilemapVisualizer, dungeonLevel.dungeonWidth, dungeonLevel.dungeonHeight);
        Vector2Int playerSpawnPosition = spawnPositions.PlayerStartPosition;
        player.position = new Vector3(playerSpawnPosition.x + 0.5f, playerSpawnPosition.y + 0.5f, 0);
        enemyManager.SpawnEnemies(dungeonGenerator.FloorPositions, enemyParent, dungeonLevel, spawnPositions.BossPosition);
    }

    internal static void HandleBossSpawn()
    {
        if (Instance.dungeonGenerator != null)
        {
            Instance.dungeonGenerator.InactivateDungeonGate();
            Debug.Log("HandleBossSpawn called and InactivateDungeonGate executed.");
        }
        else
        {
            Debug.LogError("dungeonGenerator is not assigned.");
        }
    }

    internal static void HandleBossDied()
    {
        if (Instance.dungeonGenerator != null)
        {
            Instance.dungeonGenerator.ActivateDungeonGate();
            Debug.Log("HandleBossDied called and ActivateDungeonGate executed.");
        }
        else
        {
            Debug.LogError("dungeonGenerator is not assigned.");
        }
    }

    private void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartDungeon()
    {
        GameManager.Instance.RestartGame();
    }

    public void GoToHouse()
    {
        GameManager.Instance.GoToHouse();
    }
}
