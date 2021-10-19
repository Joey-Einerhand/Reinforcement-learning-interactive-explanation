using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalSnakeTileBased : Agent
{
    [SerializeField] internal TileBasedSnake snake;
    [SerializeField] internal FoodTileBased food;
    [SerializeField] internal GridManager environmentGridManager;
    [SerializeField] internal Color winColor;
    [SerializeField] internal Color loseColor;
    [SerializeField] internal SpriteRenderer floorSpriteRenderer;

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        SetReward(0f);
        environmentGridManager.ResetGridContent();
        food.RandomizePosition();
        snake.ResetState();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        //Currently, you cannot modify the agent's obvervation vector size in script. 
        // If you could you could edit it here. We're hard-coding it for now, hoping support will be added
        // in the future.
        foreach (List<float> horizontalRow in environmentGridManager.tileContent)
        {
            sensor.AddObservation(horizontalRow);
        }
        sensor.AddObservation(snake.direction);

    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        // [0, 1, 2, 3]
        // left, right, up, down
        int directionToTurnTo = actions.DiscreteActions[0];
        //Debug.Log(directionToTurnTo);

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

        float horizontalMovementDirection = Input.GetAxisRaw("Horizontal");
        float verticalMovementDirection = Input.GetAxisRaw("Horizontal");
        if (horizontalMovementDirection < 0)
        {
            discreteActions[0] = 0;
        }
        else if (horizontalMovementDirection > 0)
        {
            discreteActions[0] = 1;
        }
        else if (verticalMovementDirection > 0)
        {
            discreteActions[0] = 2;
        }
        else if (verticalMovementDirection < 0)
        {
            discreteActions[0] = 3;
        }
    }

    /// <summary>
    /// Adds a negative reward per step the snake sets. Prevents snake from improving livespan but not improving reward
    /// </summary>
    /// Should be less than (reward for eating food / (amount of tiles in field - 1)) to prevent snake from suiciding 
    /// because getting to the reward would be more punishing than getting the reward
    public void Discounting()
    {
        // calculation is done here to support editing of field size mid-play
        float maxAmountOfTilesToMove = (environmentGridManager.AmountOfTilesHorizontally * environmentGridManager.AmountOfTilesVertically) - 1;
        // Assuming there's only one type of food and it gives exactly 1 reward when eaten
        // make the reward negative to punish snake for taking steps
        float maxNegativeRewardForStep = -1 * (1 / maxAmountOfTilesToMove);
        AddReward(maxNegativeRewardForStep);
        //AddReward(-0.05f);
    }

    public void EndGame()
    {
        AddNegativeAward(-2f);
        EndEpisode();


    }

    public void SnakeAteFoodReward(float rewardNum)
    {
        AddReward(rewardNum);
        floorSpriteRenderer.color = winColor;
        food.RandomizePosition();
    }

    void AddNegativeAward(float awardNum)
    {
        AddReward(awardNum);
        floorSpriteRenderer.color = loseColor;
    }
}
