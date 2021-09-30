using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalSnake : Agent
{
    [SerializeField] internal Snake snake;
    [SerializeField] internal Food food;
    [SerializeField] internal Collider2D rewardCollider;
    [SerializeField] internal Collider2D agentCollider;
    [SerializeField] internal Transform targetTransform;
    [SerializeField] internal Color winColor;
    [SerializeField] internal Color loseColor;
    [SerializeField] internal SpriteRenderer floorSpriteRenderer;

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        SetReward(0f);
        food.RandomizePosition();
        snake.ResetState();
        

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        Vector2 normalisedAgentLocation = new Vector2(
                                                        (transform.position.x - snake.bounds.min.x) / (snake.bounds.max.x - snake.bounds.min.x),
                                                        (transform.position.y - snake.bounds.min.y) / (snake.bounds.max.y - snake.bounds.min.y)
                                                    );
        Vector2 normalisedGoalLocation = new Vector2(
                                                (targetTransform.position.x - snake.bounds.min.x) / (snake.bounds.max.x - snake.bounds.min.x),
                                                (targetTransform.position.y - snake.bounds.min.y) / (snake.bounds.max.y - snake.bounds.min.y)
                                            );
        sensor.AddObservation(normalisedAgentLocation);
        sensor.AddObservation(normalisedGoalLocation);
        //sensor.AddObservation(snake.direction);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        // [0, 1, 2, 3]
        // left, right, up, down
        int directionToTurnTo = actions.DiscreteActions[0];
        Debug.Log(directionToTurnTo);

        switch (directionToTurnTo)
        {
            case (0):
                if (snake.direction != Vector2.right)
                    snake.direction = Vector2.left;
                break;
            case (1):
                if (snake.direction != Vector2.left)
                    snake.direction = Vector2.right;
                break;
            case (2):
                if (snake.direction != Vector2.down)
                    snake.direction = Vector2.up;
                break;
            case (3):
                if (snake.direction != Vector2.up)
                    snake.direction = Vector2.down;
                break;

        }

        //snake.Move();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // manual moving
        base.Heuristic(actionsOut);
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        if (Input.GetAxis("Horizontal") < 0)
        {
            discreteActions[0] = 0;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            discreteActions[0] = 2;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            discreteActions[0] = 3;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // needs to go first because this gets triggered more often
        if (other.TryGetComponent<SnakeSegment>(out SnakeSegment snakeSegment) && (snakeSegment.snakeSegmentNumber > 3))
        {
            NegativeAward(-1f);
            EndEpisode();
        }
        else if (other.TryGetComponent<Goal>(out Goal goal))
        {
            PositiveAward(1f);
            food.RandomizePosition();
            snake.Grow();
        }
        else if (other.TryGetComponent<Wall>(out Wall wall))
        {
            NegativeAward(-1f);
            EndEpisode();
        }

    }

    void PositiveAward(float awardNum)
    {
        SetReward(awardNum);
        floorSpriteRenderer.color = winColor;
    }

    void NegativeAward(float awardNum)
    {
        SetReward(-1f);
        floorSpriteRenderer.color = loseColor;
    }
}
