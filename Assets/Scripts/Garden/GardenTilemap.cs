using UnityEngine;
using UnityEngine.Tilemaps;

public class GardenTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public GardenManager gardenManager;

    void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
        if (gardenManager == null)
        {
            gardenManager = FindObjectOfType<GardenManager>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
            HandleTileClick(cellPosition);
        }
    }

    void HandleTileClick(Vector3Int cellPosition)
    {
        if (tilemap.HasTile(cellPosition))
        {
            Vector3 cellWorldPos = tilemap.GetCellCenterWorld(cellPosition);
            gardenManager.HandleTileClick(cellWorldPos);
        }
    }
}
