using UnityEngine;

public class FoodTileBased : MonoBehaviour
{
    [SerializeField] GridManager environmentGridManager;
    Tile currentTile;



    private void Start()
    {
        //RandomizePosition();
    }

    public void RandomizePosition()
    {
        Debug.Log(EnvironmentGridManager);
        Tile randomTile = EnvironmentGridManager.GetRandomTile();
        while (randomTile.TileContentType != ContentType.empty)
        {
            randomTile = EnvironmentGridManager.GetRandomTile();
        }
        currentTile = randomTile;
        currentTile.ChangeTileContentType(ContentType.reward);
        transform.position = currentTile.transform.position;
    }

    public GridManager EnvironmentGridManager { get => environmentGridManager; set => environmentGridManager = value; }
}
