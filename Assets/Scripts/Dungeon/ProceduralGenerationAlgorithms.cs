using UnityEngine;
using System.Collections.Generic;

public class ProceduralGenerationAlgorithms : MonoBehaviour
{
    private IProceduralGenerationStrategy generationStrategy;

    private void Awake()
    {
        generationStrategy = new SimpleRandomWalkStrategy();
    }

    public HashSet<Vector2Int> GeneratePath(Vector2Int startPosition, int walkLength)
    {
        return generationStrategy.GeneratePath(startPosition, walkLength);
    }

    public void SetGenerationStrategy(IProceduralGenerationStrategy strategy)
    {
        generationStrategy = strategy;
    }
}
