using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // tile size in Unity units
    [SerializeField] private int tileWidth, tileHeight;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Collider2D areaToFillWithGrid;
    // keeps track of individual classes
    public List<List<Tile>> tiles = new List<List<Tile>>();
    // keeps track of tile content, but instead of classes, uses ints.
    // helps the machine learn quicker by limiting amount of data which needs to be observed
    // needs to be float so ML-agents can observe it
    public List<List<float>> tileContent = new List<List<float>>();
    int amountOfTilesHorizontally;
    int amountOfTilesVertically;



    private void Awake()
    {
        GenerateGrid();
    }
    public void GenerateGrid()
    {
        Debug.Log("generating grid..");
        Bounds boundsOfAreaToFill = areaToFillWithGrid.bounds;
        // Try to have the location of the bounds to not have a decimal.
        // if it's not, the amount of tiles might differ based on location due to rounding
        AmountOfTilesHorizontally = (int)(boundsOfAreaToFill.size.x / tileWidth);
        AmountOfTilesVertically = (int)(boundsOfAreaToFill.size.y / tileHeight);
        float firstTileX = transform.position.x + (tileWidth / 2f) - ((int)(AmountOfTilesHorizontally / 2) * tileWidth);
        float firstTileY = transform.position.y + (tileHeight / 2f) - ((int)(AmountOfTilesVertically / 2) * tileHeight);
        for (int x = 0; x < AmountOfTilesHorizontally; x++)
        {
            tileContent.Add(new List<float>());
            tiles.Add(new List<Tile>());
            for (int y = 0; y < AmountOfTilesVertically; y++)
            {
                
                float xToSpawnTileAt = firstTileX + (x * tileWidth);
                float yToSpawnTileAt = firstTileY + (y * tileHeight);

                Tile spawnedTile = Instantiate(tilePrefab, new Vector2(xToSpawnTileAt, yToSpawnTileAt), Quaternion.identity);
                spawnedTile.transform.parent = transform;
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.OwnGridManager = this;
                spawnedTile.CoordinateInGrid = new int[] { x, y };
                tiles[x].Add(spawnedTile);
                tileContent[x].Add(0);

            }
        }
        ResetGridContent();
    }
    public void ResetGridContent()
    {
        // reset numbers array first, it'll be changed by changing the tile classes themselves
        for (int i = 0; i < AmountOfTilesHorizontally - 1; i++)
        {
            for (int j = 0; j < AmountOfTilesVertically - 1; j++)
            {
                tileContent[i][j] = 0;

            }
        }

        foreach (List<Tile> horizontalRow in tiles)
        {
            foreach (Tile tile in horizontalRow)
            {
                // if the tile is a boundary, change the tile type to be a wall.
                if (tile.CoordinateInGrid[0] == 0 || tile.CoordinateInGrid[0] == AmountOfTilesHorizontally - 1 || tile.CoordinateInGrid[1] == 0 || tile.CoordinateInGrid[1] == AmountOfTilesVertically - 1)
                {
                    tile.ChangeTileContentType(ContentType.wall);
                }
                else
                {
                    tile.ChangeTileContentType(ContentType.empty);
                }
            }
        }
    }

    public Tile GetRandomTile()
    {
        int yIndex = Random.Range(0, AmountOfTilesVertically - 1);
        int xIndex = Random.Range(0, AmountOfTilesHorizontally - 1);
        return tiles[xIndex][yIndex];
    }

    // this could be handled better by keeping track of these variables and changing them when a tile's content changes, instead of
    // looping through the entire 2d array.
    public int GetAmountOfTilesWithContent(ContentType typeToCheckFor)
    {
        int amountOfTilesWithContent = 0;
        foreach (List<Tile> horizontalRow in tiles)
        {
            foreach (Tile tile in horizontalRow)
            {
                if (tile.TileContentType == typeToCheckFor)
                {
                    amountOfTilesWithContent++;
                }
            }
        }
        return amountOfTilesWithContent;
    }

    public int AmountOfTilesHorizontally { get => amountOfTilesHorizontally; set => amountOfTilesHorizontally = value; }
    public int AmountOfTilesVertically { get => amountOfTilesVertically; set => amountOfTilesVertically = value; }
}
