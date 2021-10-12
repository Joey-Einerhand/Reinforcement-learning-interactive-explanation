using UnityEngine;

public class FoodTileBased : MonoBehaviour
{
    [SerializeField] GridManager environmentGridManager;
    Tile currentTile;

    public void RandomizePosition()
    {
        // It's possible that there are no empty tiles left in the field, which means the snake fills the entire board (minus walls)
        // So make sure we're not getting into an infinite loop
        int amountOfEmptyTilesInGrid = environmentGridManager.GetAmountOfTilesWithContent(ContentType.empty);
        if (amountOfEmptyTilesInGrid == 0)
        {
            // TODO: Do something to indicate game is over. For now, just return to stop trying to place the food in a filled grid
            return;
        }


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
        //transform.position = currentTile.transform.position;
        transform.position = currentTile.transform.position;
    }

    public void RandomizePositionDebug()
    {

        // get a random empty tile in the field
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
        //Debug.Log("Moving food");
        //Debug.Log(transform.position + " " + transform.localPosition);
        //Debug.Log("Previous food pos " + transform.localPosition + " " + currentTile.transform.position);
        //transform.localPosition = currentTile.transform.position;
        Debug.Log(gameObject);
        transform.position = transform.position + new Vector3(9999, 9999, 0);
        //Debug.Log("current food pos " + transform.localPosition + " " + currentTile.transform.position);
    }

    public GridManager EnvironmentGridManager { get => environmentGridManager; set => environmentGridManager = value; }
}
