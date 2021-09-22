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
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        Debug.Log(moveX);
        Debug.Log(moveY);

        // Only allow turning up or down while moving in the x-axis
        if (snake.direction.x != 0f)
        {
            if (moveY > 0)
            {
                snake.direction = Vector2.up;
            }
            else if (moveY < 0)
            {
                snake.direction = Vector2.down;
            }
        }
        // Only allow turning left or right while moving in the y-axis
        else if (snake.direction.y != 0f)
        {
            if (moveX > 0)
            {
                snake.direction = Vector2.right;
            }
            else if (moveX < 0)
            {
                snake.direction = Vector2.left;
            }
        }

        // moving is handled by Snake
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // manual moving
        base.Heuristic(actionsOut);
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1f);
            floorSpriteRenderer.color = winColor;
            food.RandomizePosition();
            snake.Grow();
        }
        else if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floorSpriteRenderer.color = loseColor;
            EndEpisode();
        }

    }
}
