using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class GroundGenerator : MonoBehaviour
{
    [Header("Map size in tiles")]
    public int mapWidth = 50;   // number of tiles horizontally
    public int mapHeight = 50;  // number of tiles vertically

    [Header("Tile to use")]
    public TileBase groundTile;

    void Start()
    {
        GenerateGround();
    }

    void GenerateGround()
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        tilemap.ClearAllTiles();

        // We want the map centered around (0,0),
        // so we offset the tile indices.
        int xStart = -mapWidth / 2;
        int yStart = -mapHeight / 2;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                int tileX = xStart + x;
                int tileY = yStart + y;

                Vector3Int cellPos = new Vector3Int(tileX, tileY, 0);
                tilemap.SetTile(cellPos, groundTile);
            }
        }
    }
}
