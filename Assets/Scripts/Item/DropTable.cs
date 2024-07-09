using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "ScriptableObjects/DropTable", order = 1)]
public class DropTable : ScriptableObject
{
    public List<DropItem> dropItems;
}

[System.Serializable]
public class DropItem
{
    public ItemSO item; // Reference to the Item ScriptableObject
    public float dropChance;
}
