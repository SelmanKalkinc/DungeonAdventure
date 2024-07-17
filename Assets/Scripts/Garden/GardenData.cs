using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GardenData
{
    public List<PlantData> plants = new List<PlantData>();
}

[Serializable]
public class PlantData
{
    public string plantID;
    public SerializableVector3 position;
    public DateTime plantedTime;
    public int growthStage; // 0 = Seed, 1 = Half-Growth, 2 = Full-Growth
    public bool isDead;
    public List<DateTime> wateredTimes;
    public List<DateTime> fertilizedTimes;

    public PlantData(string plantID, SerializableVector3 position, DateTime plantedTime, int growthStage, bool isDead, List<DateTime> wateredTimes, List<DateTime> fertilizedTimes)
    {
        this.plantID = plantID;
        this.position = position;
        this.plantedTime = plantedTime;
        this.growthStage = growthStage;
        this.isDead = isDead;
        this.wateredTimes = wateredTimes;
        this.fertilizedTimes = fertilizedTimes;
    }
}
