using UnityEngine;

public class House : MonoBehaviour
{
    // Basic properties of the house
    public string houseName;
    public int numberOfRooms;

    void Start()
    {
        Debug.Log("House initialized: " + houseName);
    }

    // Additional functionality can be added here
}
