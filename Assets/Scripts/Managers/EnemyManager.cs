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

        while (spawnedEnemies < dungeonLevel.enemyCount)
        {
            int randomIndex = Random.Range(0, floorPositionsList.Count);
            Vector2Int position = floorPositionsList[randomIndex];
            Vector3 spawnPosition = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);

            // Select a random enemy type
            EnemyStats selectedEnemyType = dungeonLevel.enemyTypes[Random.Range(0, dungeonLevel.enemyTypes.Count)];
            GameObject enemy = Instantiate(selectedEnemyType.enemyPrefab, spawnPosition, Quaternion.identity, parent);
            Enemy spawnedEnemy = enemy.GetComponent<Enemy>();
            if (spawnedEnemy != null)
            {
                // Assign the enemy stats to the enemy script
                spawnedEnemy.SetStats(selectedEnemyType);
                spawnedEnemy.InitializeEnemy();
                spawnedEnemy.OnEnemyDied += DecrementEnemyCount; // Subscribe to enemy death event
                enemiesAlive++; // Increment the counter
                UpdateEnemiesAliveText();
            }
            spawnedEnemies++;
        }



        // Optionally spawn a boss
        if (dungeonLevel.boss != null)
        {
            Vector3 bossSpawnPosition = new Vector3(bossPosition.x + 0.5f, bossPosition.y + 0.5f, 0);
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
        DungeonManager.HandleBossDied();
    }
}
