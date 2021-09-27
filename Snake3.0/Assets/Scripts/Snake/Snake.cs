using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    public Vector2 direction = Vector2.right;
    public int initialSize = 4;
    public float timeSinceLastMoved;
    public float tileSpeedPerSecond = 50f;
    public MoveToGoalSnake moveToGoalSnake;
    public Collider2D gridArea;


    private void Start()
    {
        timeSinceLastMoved = Time.time;
    }

    private void Update()
    {
        if (Time.time - timeSinceLastMoved >= (1 / tileSpeedPerSecond))
            Move();
    }

    public void Move()
    {

        // Set each segment's position to be the same as the one it follows. We
        // must do this in reverse order so the position is set to the previous
        // position, otherwise they will all be stacked on top of each other.
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        // Move the snake in the direction it is facing
        // Round the values to ensure it aligns to the grid
        float x = Mathf.Round(this.transform.position.x) + this.direction.x;
        float y = Mathf.Round(this.transform.position.y) + this.direction.y;

        this.transform.position = new Vector2(x, y);
        timeSinceLastMoved = Time.time;
    }

    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        SnakeSegment snakeSegment = segment.GetComponent<SnakeSegment>();
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
        snakeSegment.snakeSegmentNumber = _segments.Count - 1;
    }

    public void ResetState()
    {
        this.direction = Vector2.right;
        RandomizePosition();

        // Start at 1 to skip destroying the head
        for (int i = 1; i < _segments.Count; i++) {
            Destroy(_segments[i].gameObject);
        }

        // Clear the list but add back this as the head
        _segments.Clear();
        _segments.Add(this.transform);

        // -1 since the head is already in the list
        for (int i = 0; i < this.initialSize - 1; i++) {
            Grow();
        }
    }

    public void RandomizePosition()
    {
        Bounds bounds = this.gridArea.bounds;

        // Pick a random position inside the bounds
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        // Round the values to ensure it aligns with the grid
        x = Mathf.Round(x);
        y = Mathf.Round(y);

        this.transform.position = new Vector2(x, y);
    }

}
