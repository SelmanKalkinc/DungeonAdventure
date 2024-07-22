using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantItem", menuName = "ScriptableObjects/Inventory/PlantItem", order = 1)]
public class PlantItemSO : ItemSO
{
    public GameObject PlantPrefab;
    public float GrowthTime; // Total time for the plant to grow to maturity (in seconds)
    public float DeadTime;
    public int RequiredWatering; // Total number of waterings needed
    public float WaterInterval; // Optimal interval between waterings (in seconds)

    public Sprite SeedSprite; // Sprite for the seed state
    public Sprite HalfGrowthSprite; // Sprite for the half-growth state
    public Sprite FullGrowthSprite; // Sprite for the fully grown state
    public Sprite DeadSprite;

    public HarvestableItemSO HarvestableItem; // Reference to the harvestable item
    public HarvestableItemSO RottenHarvestableItem; // Reference to the rotten harvestable item

    public PlantCareRoutineSO careRoutine; // Reference to the plant care routine
}
