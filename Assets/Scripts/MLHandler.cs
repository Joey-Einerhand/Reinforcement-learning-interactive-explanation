using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using Unity.MLAgents;

public class MLHandler : MonoBehaviour
{
    // A list of all currently active moveToGoal agents.
    // Agents add/remove themself from this list when enabled/disabled.
    [SerializeField] List<Agent> agents = new List<Agent>();
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
        GetAllAgentsInScene();
    }
    
    public void GetAllAgentsInScene()
    {
        // get all objects with agent tag in scene, get their agent script and add those to our list
        // this is used to assign brains
        GameObject[] agentObjectsInScene = GameObject.FindGameObjectsWithTag("Agent");
        foreach (GameObject agentGameObject in agentObjectsInScene)
        {
            agents.Add(agentGameObject.GetComponent<Agent>());
        }
    }    

    public void ChangeAgentBrain(int brainIndexToUse)
    {
        Debug.Log("Changing brain");
        foreach (Agent agent in agents)
        {
            agent.SetModel("MoveToGoal", brainModels[brainIndexToUse]);
            agent.EndEpisode();
        }
    }

    public List<Agent> Agents { get => agents; set => agents = value; }
}
