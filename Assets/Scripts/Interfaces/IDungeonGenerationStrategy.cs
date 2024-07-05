using UnityEngine;
using System.Collections.Generic;

public interface IDungeonGenerationStrategy
{
    HashSet<Vector2Int> GenerateDungeon(int width, int height);
}
