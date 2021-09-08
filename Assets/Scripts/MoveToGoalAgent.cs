using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Collider2D rewardCollider;
    [SerializeField] private Collider2D agentCollider;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private SpriteRenderer floorSpriteRenderer;
    [SerializeField] private float movementSpeed = 2f;

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        SetReward(0f);
        transform.localPosition = new Vector3(Random.Range(-5.9f, 2.2f), Random.Range(-1.9f, 2.1f), 0);
        targetTransform.localPosition = new Vector3(Random.Range(-5.9f, 2.2f), Random.Range(-1.9f, 2.1f), 0);
        if (agentCollider.bounds.Intersects(rewardCollider.bounds))
        {
            targetTransform.localPosition = new Vector3(Random.Range(-5.9f, 2.2f), Random.Range(-1.9f, 2.1f), 0);
            Debug.Log("Colliding");
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        //MLHandler.instance.MoveToGoalAgents.Add(this);
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        MLHandler.instance.MoveToGoalAgents.Remove(this);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, moveY, 0) * Time.deltaTime * movementSpeed;
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
            EndEpisode();
        }
        else if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floorSpriteRenderer.color = loseColor;
            EndEpisode();
        }

    }
}
