using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/HealthModifier", order = 1)]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override bool AffectCharacter(GameObject character, float value)
    {
        Health health = character.GetComponent<Health>();
        if (health != null && health.currentHealth < health.maxHealth)
        {
            health.Heal((int)value);
            return true;
        }
        else
        {
            return false;
        }
    }
}
