using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantCareRoutine", menuName = "ScriptableObjects/PlantCareRoutine", order = 1)]
public class PlantCareRoutineSO : ScriptableObject
{
    [Serializable]
    public struct CareAction
    {
        public ActionType actionType; // Action type (Water, Fertilize, or Wait)
        public float waitTime; // Time to wait if actionType is Wait (in seconds)

        public CareAction(ActionType actionType, float waitTime)
        {
            this.actionType = actionType;
            this.waitTime = waitTime;
        }
    }

    public CareAction[] careActions;
}

public enum ActionType
{
    Water,
    Fertilize,
    Wait
}
