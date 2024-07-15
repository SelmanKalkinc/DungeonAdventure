using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantItem", menuName = "ScriptableObjects/Inventory/PlantItem", order = 2)]
public class PlantItemSO : ItemSO
{
    [field: SerializeField]
    public GameObject PlantPrefab { get; set; } // Prefab for the plant

    [field: SerializeField]
    public float GrowthTime { get; set; } // Time for each growth stage

    [field: SerializeField]
    public int RequiredWatering { get; set; } // Number of waterings required per stage

    [field: SerializeField]
    public float WaterInterval { get; set; } // Interval for watering

    [field: SerializeField]
    public GameObject[] GrowthStages { get; set; } // Array of growth stage prefabs

    [field: SerializeField]
    public GameObject[] QualityStages { get; set; } // Array of quality stage prefabs (Low, Medium, High)
}
