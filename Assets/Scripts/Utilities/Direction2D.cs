using System.Collections.Generic;
using UnityEngine;

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1, 0),  // Right
        new Vector2Int(-1, 0), // Left
        new Vector2Int(0, 1),  // Up
        new Vector2Int(0, -1)  // Down
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1, 1),   // Top-Right
        new Vector2Int(1, 0),   // Right
        new Vector2Int(1, -1),  // Bottom-Right
        new Vector2Int(0, 1),   // Top
        new Vector2Int(0, -1),  // Bottom
        new Vector2Int(-1, 1),  // Top-Left
        new Vector2Int(-1, 0),  // Left
        new Vector2Int(-1, -1)  // Bottom-Left
    };

    public static List<Vector2Int> GetCardinalDirectionsList()
    {
        return new List<Vector2Int>(cardinalDirectionsList);
    }

    public static Vector2Int GetRandomDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
