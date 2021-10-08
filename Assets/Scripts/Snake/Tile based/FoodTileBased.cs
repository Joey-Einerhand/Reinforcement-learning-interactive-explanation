using UnityEngine;

public class FoodTileBased : MonoBehaviour
{
    [SerializeField] GridManager environmentGridManager;
    Tile currentTile;

    public void RandomizePosition()
    {

        
        Tile randomTile = EnvironmentGridManager.GetRandomTile();
        while (randomTile.TileContentType != ContentType.empty)
        {
            randomTile = EnvironmentGridManager.GetRandomTile();
        }

        // needs to be executed after getting a random new tile, else the current tile might be a
        // candidate tile to move to
        if (currentTile != null)
        {
            currentTile.ChangeTileContentType(ContentType.empty);
        }
        currentTile = randomTile;
        currentTile.ChangeTileContentType(ContentType.reward);
        transform.position = currentTile.transform.position;
    }

    public void RandomizePositionDebug()
    {


        Tile randomTile = EnvironmentGridManager.GetRandomTile();
        while (randomTile.TileContentType != ContentType.empty)
        {
            randomTile = EnvironmentGridManager.GetRandomTile();
        }

        // needs to be executed after getting a random new tile, else the current tile might be a
        // candidate tile to move to
        if (currentTile != null)
        {
            currentTile.ChangeTileContentType(ContentType.empty);
            Debug.Log(currentTile.CoordinateInGrid[0] + " " + currentTile.CoordinateInGrid[1] + "--" + randomTile.CoordinateInGrid[0] + " " + randomTile.CoordinateInGrid[1]);
        }

        currentTile = randomTile;
        currentTile.ChangeTileContentType(ContentType.reward);
        Debug.Log("Moving food");
        Debug.Log(transform.position + " " + currentTile.transform.position);
        transform.position = currentTile.transform.position;
    }

    public GridManager EnvironmentGridManager { get => environmentGridManager; set => environmentGridManager = value; }
}
