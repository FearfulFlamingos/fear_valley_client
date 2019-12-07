using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public GameObject networkCommands;
    void Start()
    {
        //GetComponent<ArmyBudget>().connectionID = GetComponent<PlayerObject>().GetConnID();
    }
    public void SwitchToMainScene()
    {
        SceneManager.LoadScene("main");
        networkCommands.GetComponent<NetworkArmyQueueManager>().CmdExecute();
    }
}
