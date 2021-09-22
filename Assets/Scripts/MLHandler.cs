using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class MLHandler : MonoBehaviour
{
    // A list of all currently active moveToGoal agents.
    // Agents add/remove themself from this list when enabled/disabled.
    [SerializeField] List<MoveToGoalAgent> moveToGoalAgents = new List<MoveToGoalAgent>();
    [SerializeField] List<NNModel> brainModels = new List<NNModel>();

    // Global accessor, used so other classes can call an existing MLHandler instance without having a direct
    // reference to it.
    public static MLHandler instance;

    private void Awake()
    {
        // singleton pattern, destroy if copy of itself already exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ChangeAgentBrain(int brainIndexToUse)
    {
        Debug.Log("Changing brain");
        foreach (MoveToGoalAgent agent in moveToGoalAgents)
        {
            agent.SetModel("MoveToGoal", brainModels[brainIndexToUse]);
        }
    }

    public List<MoveToGoalAgent> MoveToGoalAgents { get => moveToGoalAgents; set => moveToGoalAgents = value; }
}
