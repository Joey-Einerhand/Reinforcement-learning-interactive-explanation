using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TileBasedSnake : MonoBehaviour
{
    private List<SnakeSegmentTileBased> _segments = new List<SnakeSegmentTileBased>();
    public Transform segmentPrefab;
    public Vector2 direction = Vector2.right;
    public int initialSize = 4;
    public float timeSinceLastMoved;
    public float tileSpeedPerSecond = 50f;
    public MoveToGoalSnakeTileBased moveToGoalSnakeTileBased;
    [SerializeField] FoodTileBased environmentFood;
    [SerializeField] GridManager environmentGridManager;
    private Tile currentLocationOfHead;
    // This counter counts the steps since a last major event, like
    // eating a piece of food, or the last episode end.
    // used for punishing the snake for looping too much without doing anything.
    private int stepsSinceLastMajorAction = 0;

    private void Start()
    {
        timeSinceLastMoved = Time.time;
        environmentFood.EnvironmentGridManager = environmentGridManager;
    }

    private void Update()
    {
        // random number. End a round if this number has been reached
        if (stepsSinceLastMajorAction >= 150)
        {
            moveToGoalSnakeTileBased.EndEpisode();
            return;
        }

        if (Time.time - timeSinceLastMoved >= (1 / tileSpeedPerSecond))
        {
            int rowsWithRewards = 0;
            foreach (List<float> contentRow in environmentGridManager.tileContent)
            {
                if (contentRow.Contains(3f))
                {
                    rowsWithRewards += 1;
                }
            }
            Debug.Log("Rows with rewards: " + rowsWithRewards);
            moveToGoalSnakeTileBased.RequestDecision();
            TryToMove();
        }

        stepsSinceLastMajorAction += 1;

    }

    public void TryToMove()
    {
        Tile tileToMoveTo = GetTileToMoveInto();
        MoveIntoTile(tileToMoveTo);

        timeSinceLastMoved = Time.time;
        
    }

    private Tile GetTileToMoveInto()
    {
        int[] currentGridPosition = currentLocationOfHead.CoordinateInGrid;
        int[] gridPositionToMoveTo;
        if (direction == Vector2.up)
        {
            // Get the current position, add 1 to Y
            gridPositionToMoveTo = new int[] { currentGridPosition[0], currentGridPosition[1] + 1 };
            
        }
        else if (direction == Vector2.right)
        {
            gridPositionToMoveTo = new int[] { currentGridPosition[0] + 1, currentGridPosition[1] };
        }
        else if (direction == Vector2.down)
        {
            gridPositionToMoveTo = new int[] { currentGridPosition[0], currentGridPosition[1] - 1 };
        }
        else
        {
            gridPositionToMoveTo = new int[] { currentGridPosition[0] - 1, currentGridPosition[1] };
        }
        Tile tileToMoveTo = environmentGridManager.tiles[gridPositionToMoveTo[0]][gridPositionToMoveTo[1]];
        return tileToMoveTo;
    }

    public void MoveIntoTile(Tile tileToMoveInto)
    {
        // check if we can move first (and do stuff like dying if neccesarry)
        if (tileToMoveInto.TileContentType == ContentType.empty)
        {
            Move(tileToMoveInto);
        }
        else if (tileToMoveInto.TileContentType == ContentType.wall)
        {
            Die();
        }
        else if (tileToMoveInto.TileContentType == ContentType.snake)
        {
            Die();
        }
        // else, eat normal food
        else
        {
            EatFood(1, 1);
            Move(tileToMoveInto);
        }
    }

    private void Move(Tile tileToMoveInto)
    {

        // Set each segment's position to be the same as the one it follows. We
        // must do this in reverse order so the position is set to the previous
        // position, otherwise they will all be stacked on top of each other.
        for (int i = _segments.Count - 1; i >= 0; i--)
        {
            SnakeSegmentTileBased segmentToMove = _segments[i];
            // If last segment, set current tile to empty
            if (i == _segments.Count - 1)
            {
                segmentToMove.CurrentTile.ChangeTileContentType(ContentType.empty);
            }
            // if first segment, set current tile to be the head's tile
            if (i == 0)
            {
                segmentToMove.CurrentTile = currentLocationOfHead;
            }
            else
            {
                // We don't have to set the tile the segment's moved to, to Snake
                // because it's already that content type; the head has set it to Snake earlier.
                _segments[i].CurrentTile = _segments[i - 1].CurrentTile;
            }
            segmentToMove.transform.position = segmentToMove.CurrentTile.transform.position;
        }

        // move head
        currentLocationOfHead = tileToMoveInto;
        transform.position = tileToMoveInto.transform.position;
        tileToMoveInto.ChangeTileContentType(ContentType.snake);

        // give negative reward for movement to encourage going to food asap
        moveToGoalSnakeTileBased.Discounting();
    }

    public void EatFood(int reward, int amountToGrow)
    {
        moveToGoalSnakeTileBased.SnakeAteFoodReward(reward);
        //environmentFood.RandomizePositionDebug();
        //environmentFood.RandomizePosition();
        Grow(amountToGrow);
        stepsSinceLastMajorAction = 0;
    }

    public void Grow(int amountToGrow)
    {
        for (int i = 0; i < amountToGrow; i++)
        {
            Transform segment = Instantiate(this.segmentPrefab);
            SnakeSegmentTileBased snakeSegment = segment.gameObject.GetComponent<SnakeSegmentTileBased>();
            snakeSegment.SnakeSegmentNumber = _segments.Count;
            if (snakeSegment.SnakeSegmentNumber == 0)
            {
                segment.position = transform.position;
            }
            else
            {
                segment.position = _segments[_segments.Count - 1].transform.position;
            }
            snakeSegment.CurrentTile = currentLocationOfHead;
            snakeSegment.CurrentTile.ChangeTileContentType(ContentType.snake);
            _segments.Add(snakeSegment);

        }
    }

    public void Die()
    {
        moveToGoalSnakeTileBased.EndGame();
    }

    public void ResetState()
    {
        this.direction = Vector2.right;
        RandomizePosition();
        stepsSinceLastMajorAction = 0;

        for (int i = 0; i < _segments.Count; i++) {
            Destroy(_segments[i].gameObject);
        }

        // Clear the list but add back this as the head
        _segments.Clear();

        Grow(initialSize);
    }

    public void RandomizePosition()
    {
        Tile randomTile = environmentGridManager.GetRandomTile();
        while (randomTile.TileContentType != ContentType.empty)
        {
            randomTile = environmentGridManager.GetRandomTile();
        }
        currentLocationOfHead = randomTile;
        currentLocationOfHead.ChangeTileContentType(ContentType.snake);
        transform.position = currentLocationOfHead.transform.position;
    }

}
