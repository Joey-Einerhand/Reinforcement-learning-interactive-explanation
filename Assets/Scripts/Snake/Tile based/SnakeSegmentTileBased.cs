using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegmentTileBased : MonoBehaviour
{
    private int snakeSegmentNumber;
    private Tile currentTile;

    public int SnakeSegmentNumber { get => snakeSegmentNumber; set => snakeSegmentNumber = value; }
    public Tile CurrentTile { get => currentTile; set => currentTile = value; }
}
