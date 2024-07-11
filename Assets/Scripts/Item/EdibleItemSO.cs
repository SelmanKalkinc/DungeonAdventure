using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Inventory/EdibleItem", order = 1)]

public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
{
    [SerializeField]
    private List<ModifierData> modifiersData = new List<ModifierData>();

    public string ActionName => "Consume";

    public AudioClip actionSFX {get; private set;}

    public bool PerformAction(GameObject character)
    {
        bool anyAffectApplied = false;

        foreach (ModifierData data in modifiersData)
        {
            if (data.statModifier.AffectCharacter(character, data.value))
            {
                anyAffectApplied = true;
            }
        }
        return anyAffectApplied;
    }
}

public interface IDestroyableItem
{

}

public interface IItemAction
{
    public string ActionName { get; }
    public AudioClip actionSFX { get; }
    bool PerformAction(GameObject character);
}

[Serializable]
public class ModifierData
{
    public CharacterStatModifierSO statModifier;
    public float value;
}
