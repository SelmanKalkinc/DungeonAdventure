using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSaveData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetInitialSaveData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInitialSaveData()
    {
        SaveSystem.SaveHighestCompletedDungeon(0); // Set to initial level
    }

}
