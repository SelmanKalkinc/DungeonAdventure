using UnityEngine;
using System.Collections.Generic;

public interface IProceduralGenerationStrategy
{
    HashSet<Vector2Int> GeneratePath(Vector2Int startPosition, int walkLength);
}
