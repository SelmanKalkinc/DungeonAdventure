using System;
using System.Collections.Generic;

[Serializable]
public class GardenData
{
    public List<PlantData> plants;

    public GardenData()
    {
        plants = new List<PlantData>();
    }
}
