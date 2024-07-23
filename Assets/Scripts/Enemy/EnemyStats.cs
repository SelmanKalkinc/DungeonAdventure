using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyStats : ScriptableObject
{
    public string enemyName; // Ensure this field is defined
    public GameObject enemyPrefab;
    public int health;
    public int damage;
    public float attackRange;
    public float attackRate;
    public float moveSpeed; // Add this field
    public float followRange; // Add this field
    public Sprite sprite;
    public AttackType attackType;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public DropTable dropTable; // Add reference to DropTable
    public int expReward;
}
