using UnityEngine;

public class SpawnPositions
{
    private Vector2Int playerStartPosition;
    private Vector2Int bossPosition;

    public SpawnPositions(Vector2Int playerStartPosition, Vector2Int gatePosition)
    {
        this.playerStartPosition = playerStartPosition;
        this.bossPosition = gatePosition;
    }

    public Vector2Int PlayerStartPosition
    {
        get { return playerStartPosition; }
        set { playerStartPosition = value; }
    }

    public Vector2Int BossPosition
    {
        get { return bossPosition; }
        set { bossPosition = value; }
    }
}
