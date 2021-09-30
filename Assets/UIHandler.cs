using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;
    [SerializeField] Dropdown brainDropdown;

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
    public void ChangeBrainInMLHandler(int brainIndexToUse)
    {
        MLHandler.instance.ChangeAgentBrain(brainIndexToUse);
    }

    public void BrainDropdownChanged()
    {
        //brainDropdown.va
    }
    
    public void ChangeSceneToName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
