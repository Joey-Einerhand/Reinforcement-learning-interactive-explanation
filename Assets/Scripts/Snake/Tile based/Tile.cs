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
    // default empty
    [SerializeField] ContentType tileContentType;
    [SerializeField] SpriteRenderer ownSpriteRenderer;
    GridManager ownGridManager;
    int[] coordinateInGrid = new int[2];

    // these are default; edit the colors in the editor
    Color emptyGreyColor = new Color(0.8f, 0.8f, 0.8f);
    Color wallColor = Color.black;
    Color rewardColor = Color.green;
    Color snakeColor = Color.blue;




    // change own content type and send that data to the GridSystem
    public void ChangeTileContentType(ContentType typeToChangeTo)
    {
        // enums types have indexes in that enum. Get that index and assign it to tiletype
        tileContentType = typeToChangeTo;
        // sets the int in the array to the index of the enum
        // 0 = empty, 1 = wall, etc
        ownGridManager.tileContent[coordinateInGrid[0]][coordinateInGrid[1]] = (float)typeToChangeTo;

        switch(typeToChangeTo)
        {
            case ContentType.empty:
                if ((coordinateInGrid[0] %  2 == 0) && (coordinateInGrid[1] % 2 == 0))
                {
                    ownSpriteRenderer.color = Color.white;
                }
                else if ((coordinateInGrid[0] % 2 == 1) && (coordinateInGrid[1] % 2 == 1))
                {
                    ownSpriteRenderer.color = Color.white;
                }
                else
                {
                    ownSpriteRenderer.color = emptyGreyColor;
                }    
                break;
            case ContentType.wall:
                ownSpriteRenderer.color = wallColor;
                break;
            case ContentType.snake:
                ownSpriteRenderer.color = snakeColor;
                break;
            case ContentType.reward:
                ownSpriteRenderer.color = rewardColor;
                break;
        }
    }

    public GridManager OwnGridManager { get => ownGridManager; set => ownGridManager = value; }
    public int[] CoordinateInGrid { get => coordinateInGrid; set => coordinateInGrid = value; }
    public ContentType TileContentType { get => tileContentType; }
}
