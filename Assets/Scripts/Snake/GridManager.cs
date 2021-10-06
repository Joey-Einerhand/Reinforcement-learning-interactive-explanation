using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // tile size in Unity units
    [SerializeField] private int tileWidth, tileHeight;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Collider2D areaToFillWithGrid;
    public List<List<int>> tilesWithContent = new List<List<int>>();

    private void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        Bounds boundsOfAreaToFill = areaToFillWithGrid.bounds;
        int amountOfTilesHorizontally = (int)(boundsOfAreaToFill.size.x / tileWidth);
        int amountOfTilesVertically = (int)(boundsOfAreaToFill.size.y / tileHeight);
        float firstTileX = transform.position.x - (tileWidth / 2f) + ((int)(amountOfTilesHorizontally / 2) * tileWidth);
        float firstTileY = transform.position.y - (tileHeight / 2f) + ((int)(amountOfTilesVertically / 2) * tileHeight);
        for (int x = 0; x < amountOfTilesHorizontally; x++)
        {
            for (int y = 0; y < amountOfTilesVertically; y++)
            {
                
                float xToSpawnTileAt = firstTileX + (x * tileWidth);
                float yToSpawnTileAt = firstTileY + (y * tileHeight);

                Tile spawnedTile = Instantiate(tilePrefab, new Vector2(xToSpawnTileAt, yToSpawnTileAt), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.OwnGridManager = this;
                spawnedTile.CoordinateInGrid = new Vector2(x, y);

                // if the tile is a boundary, change the tile type to be a wall.
                if (x == 0 || x == amountOfTilesHorizontally || y == 0 || y == amountOfTilesVertically)
                {
                    spawnedTile.ChangeTileContentType(ContentType.wall);
                }
                else
                {
                    spawnedTile.ChangeTileContentType(ContentType.empty);
                }
                
            }
        }
    }
}
