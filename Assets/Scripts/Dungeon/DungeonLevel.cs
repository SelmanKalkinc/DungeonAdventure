using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel", menuName = "ScriptableObjects/DungeonLevel", order = 1)]
public class DungeonLevel : ScriptableObject
{
    public int level;
    public int enemyCount;
    public int dungeonWidth;
    public int dungeonHeight;
    public List<EnemyStats> enemyTypes; // List of enemy types for this dungeon level
    public EnemyStats boss;
}
