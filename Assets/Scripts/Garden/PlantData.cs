using System;
using System.Collections.Generic;

[Serializable]
public class PlantData
{
    public string plantID;
    public SerializableVector3 position;
    public DateTime plantedTime;
    public int growthStage;
    public bool isDead;
    public List<ActionLog> actionLogs;

    public PlantData(string plantID, SerializableVector3 position, DateTime plantedTime, int growthStage, bool isDead, List<ActionLog> actionLogs)
    {
        this.plantID = plantID;
        this.position = position;
        this.plantedTime = plantedTime;
        this.growthStage = growthStage;
        this.isDead = isDead;
        this.actionLogs = actionLogs;
    }
}
