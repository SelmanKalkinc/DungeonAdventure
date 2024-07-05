using System.Collections.Generic;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : MonoBehaviour
{
    public GameObject dungeonGatePrefab; // Assign this in the Inspector
    private IProceduralGenerationStrategy dungeonGenerationStrategy;
    private HashSet<Vector2Int> floorPositions;
    private GameObject gateInstance;

    public HashSet<Vector2Int> FloorPositions => floorPositions; // Add this property to expose floorPositions

    private void Awake()
    {
        // Initialize with the simple random walk strategy by default
        dungeonGenerationStrategy = new SimpleRandomWalkStrategy();
    }

    public SpawnPositions RunProceduralGeneration(TilemapVisualizer tilemapVisualizer, int width, int height)
    {
        tilemapVisualizer.Clear(); // Clear previous dungeon layout
        Debug.Log("Cleared tilemaps.");

        floorPositions = dungeonGenerationStrategy.GeneratePath(new Vector2Int(0, 0), width * height / 10);
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        
        Vector2Int playerStartPosition = GetLeftmostPosition(floorPositions, height);

        Vector2Int gatePosition = GetFurthestPositionByPath(playerStartPosition);
        gateInstance = Instantiate(dungeonGatePrefab, new Vector3(gatePosition.x + 0.5f, gatePosition.y + 0.5f, 0f), Quaternion.identity);

        return new SpawnPositions(playerStartPosition, gatePosition);
    }

    public void SetDungeonGenerationStrategy(IProceduralGenerationStrategy strategy)
    {
        dungeonGenerationStrategy = strategy;
    }

    private Vector2Int GetFurthestPositionByPath(Vector2Int startPosition)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        queue.Enqueue(startPosition);
        visited.Add(startPosition);

        Vector2Int furthestPosition = startPosition;

        while (queue.Count > 0)
        {
            Vector2Int currentPosition = queue.Dequeue();
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int neighbor = currentPosition + direction;
                if (floorPositions.Contains(neighbor) && !visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    furthestPosition = neighbor;
                }
            }
        }

        Debug.Log($"Furthest position by path from {startPosition}: {furthestPosition}");
        return furthestPosition;
    }

    private Vector2Int GetLeftmostPosition(HashSet<Vector2Int> floorPositions, int height)
    {
        Vector2Int leftmostPosition = new Vector2Int(int.MaxValue, height / 2);
        foreach (var pos in floorPositions)
        {
            if (pos.x < leftmostPosition.x)
            {
                leftmostPosition = pos;
            }
        }

        if (!floorPositions.Contains(leftmostPosition))
        {
            leftmostPosition = FindValidStartPosition(floorPositions);
        }

        Debug.Log($"Leftmost position: {leftmostPosition}");
        return leftmostPosition;
    }

    private Vector2Int FindValidStartPosition(HashSet<Vector2Int> floorPositions)
    {
        Vector2Int leftmostPosition = new Vector2Int(int.MaxValue, int.MaxValue);
        foreach (var pos in floorPositions)
        {
            if (pos.x < leftmostPosition.x)
            {
                leftmostPosition = pos;
            }
        }

        return leftmostPosition;
    }

    public void InactivateDungeonGate()
    {
        if (gateInstance != null)
        {
            gateInstance.SetActive(false);
        }
    }
    public void ActivateDungeonGate()
    {
        if (gateInstance != null)
        {
            gateInstance.SetActive(true);
        }
    }
}
