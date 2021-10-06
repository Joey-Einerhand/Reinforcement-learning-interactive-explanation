using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContentType
{
    empty,
    wall,
    snake,
    reward
}

public class Tile : MonoBehaviour
{
    // 0 = empty,   1 = wall,   2 = snake,  3 = reward
    // default empty
    [SerializeField] int tileContentType;
    GridManager ownGridManager;
    Vector2 coordinateInGrid;

    [SerializeField] Color emptyColor;
    [SerializeField] Color wallColor;
    [SerializeField] Color rewardColor;
    [SerializeField] Color snakeHeadColor;
    [SerializeField] Color snakeBodyColor;

    public Tile()
    {
        
    }

    // change own content type and send that data to the GridSystem
    public void ChangeTileContentType(ContentType typeToChangeTo)
    {
        // enums types have indexes in that enum. Get that index and assign it to tiletype
        tileContentType = (int)typeToChangeTo;
        UpdateContentTypeInGridManager();
    }

    // the gridmanager keeps track of the content of every tile in a 2d array.
    // If the content of THIS tile is changed, also change the tracked content
    // in the 2d array.
    private void UpdateContentTypeInGridManager()
    {
        int coordinateX = (int)coordinateInGrid.x;
        int coordinateY = (int)coordinateInGrid.y;
        Debug.Log(ownGridManager);
        Debug.Log(tileContentType); 
        ownGridManager.tilesWithContent[coordinateX][coordinateY] = tileContentType;
    }

    public GridManager OwnGridManager { get => ownGridManager; set => ownGridManager = value; }
    public Vector2 CoordinateInGrid { get => coordinateInGrid; set => coordinateInGrid = value; }
}
