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

    protected override void OnEnable()
    {
        base.OnEnable();
        
        //MLHandler.instance.MoveToGoalAgents.Add(this);
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        //MLHandler.instance.MoveToGoalAgents.Remove(this);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(snake.direction);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        // [0, 1, 2, 3, 4]
        // 0 = forward (no change), 1 = left (-90 degrees), 2 = right (+90 degrees)
        int directionToTurnTo = actions.DiscreteActions[0];
        Debug.Log(directionToTurnTo);

        switch (directionToTurnTo)
        {
            case (0):
                break;
            case (1):
                snake.direction = Vector2.left;
                break;
            case (2):
                snake.direction = Vector2.right;
                break;
            case (3):
                snake.direction = Vector2.up;
                break;
            case (4):
                snake.direction = Vector2.down;
                break;
        }
        // moving is handled by Snake
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // manual moving
        base.Heuristic(actionsOut);
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        if (Input.GetAxis("Horizontal") < 0)
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            discreteActions[0] = 2;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            discreteActions[0] = 3;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            discreteActions[0] = 4;
        }
        else
        {
            discreteActions[0] = 0;
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
