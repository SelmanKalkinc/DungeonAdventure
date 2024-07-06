using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public TextMeshProUGUI enemiesAliveText; // Reference to the UI Text element
    public int enemiesAlive; // Counter for enemies alive

    public void SpawnEnemies(HashSet<Vector2Int> floorPositions, Transform parent, DungeonLevel dungeonLevel, Vector2Int bossPosition)
    {
        Debug.Log("Spawning enemies...");
        List<Vector2Int> floorPositionsList = new List<Vector2Int>(floorPositions);
        int spawnedEnemies = 0;

        if (dungeonLevel.enemyCount > 0)
        {
            if (floorPositionsList.Count == 0)
            {
                Debug.LogError("No floor positions available to spawn enemies.");
                return;
            }

            while (spawnedEnemies < dungeonLevel.enemyCount)
            {
                int randomIndex = Random.Range(0, floorPositionsList.Count);
                Vector2Int position = floorPositionsList[randomIndex];
                Vector3 spawnPosition = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);

                // Select a random enemy type
                EnemyStats selectedEnemyType = dungeonLevel.enemyTypes[Random.Range(0, dungeonLevel.enemyTypes.Count)];
                Debug.Log($"Selected enemy type: {selectedEnemyType.name}");

                if (selectedEnemyType.enemyPrefab == null)
                {
                    Debug.LogError("Selected enemy type prefab is null.");
                    continue;
                }

                Debug.Log($"Instantiating enemy prefab: {selectedEnemyType.enemyPrefab.name}");
                GameObject enemy = Instantiate(selectedEnemyType.enemyPrefab, spawnPosition, Quaternion.identity, parent);
                Enemy spawnedEnemy = enemy.GetComponent<Enemy>();
                if (spawnedEnemy != null)
                {
                    Debug.Log($"Successfully instantiated enemy prefab: {selectedEnemyType.enemyPrefab.name} with Enemy component.");
                    // Assign the enemy stats to the enemy script
                    spawnedEnemy.SetStats(selectedEnemyType);
                    spawnedEnemy.InitializeEnemy();
                    spawnedEnemy.OnEnemyDied += DecrementEnemyCount; // Subscribe to enemy death event
                    enemiesAlive++; // Increment the counter
                    UpdateEnemiesAliveText();
                }
                else
                {
                    Debug.LogError($"Spawned enemy prefab: {selectedEnemyType.enemyPrefab.name} does not have an Enemy component.");
                }
                spawnedEnemies++;
            }
        }
        else
        {
            Debug.Log("No regular enemies to spawn.");
        }

        // Optionally spawn a boss
        if (dungeonLevel.boss != null)
        {
            Vector3 bossSpawnPosition = new Vector3(bossPosition.x + 0.5f, bossPosition.y + 0.5f, 0);
            if (dungeonLevel.boss.enemyPrefab == null)
            {
                Debug.LogError("Boss enemy prefab is null.");
                return;
            }

            GameObject bossEnemy = Instantiate(dungeonLevel.boss.enemyPrefab, bossSpawnPosition, Quaternion.identity, parent);
            bossEnemy.transform.localScale = new Vector3(2, 2, 1); // Scale the boss by 2x
            Enemy bossScript = bossEnemy.GetComponent<Enemy>();
            if (bossScript != null)
            {
                // Assign the boss stats to the enemy script
                bossScript.SetStats(dungeonLevel.boss);
                bossScript.InitializeEnemy();
                bossScript.OnEnemyDied += DecrementEnemyCount; // Subscribe to boss death event
                bossScript.OnEnemyDied += HandleBossDied; // Subscribe to boss death event
                enemiesAlive++; // Increment the counter
                UpdateEnemiesAliveText();
                DungeonManager.HandleBossSpawn();
                Debug.Log($"Spawned boss at {bossSpawnPosition} with health {dungeonLevel.boss.health}, damage {dungeonLevel.boss.damage}, move speed {dungeonLevel.boss.moveSpeed}, follow range {dungeonLevel.boss.followRange}. Enemies alive: {enemiesAlive}");
            }
            else
            {
                Debug.LogError("Spawned boss does not have an Enemy component.");
            }
        }
    }

    private void DecrementEnemyCount()
    {
        enemiesAlive--;
        UpdateEnemiesAliveText();
        if (enemiesAlive == 0)
        {
            // Mark the current dungeon as completed
            // Implement your logic to handle dungeon completion
            Debug.Log("All enemies are dead. Dungeon completed.");
        }
    }

    private void UpdateEnemiesAliveText()
    {
        if (enemiesAliveText != null)
        {
            enemiesAliveText.text = $"Enemies Alive: {enemiesAlive}";
        }
    }

    private void HandleBossDied()
    {
        Debug.Log("Handle boss died");
        DungeonManager.HandleBossDied();
    }
}
