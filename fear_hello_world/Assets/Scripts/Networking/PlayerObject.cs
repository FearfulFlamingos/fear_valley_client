using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour
{
   // public GameObject PlayerUnitPrefab;
    //public static int connID = 0;
    public GameObject armyBuildingRef;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            Debug.Log("This belongs to another player");
            return;
        }

        PlayerPrefs.SetInt("connID",connectionToClient.connectionId);

        //Instantiate(PlayerUnitPrefab);
        //NetworkServer.Spawn(PlayerUnitPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            CmdGetConn();
        
    }
    // [Command] - functions that run on the server
    //

    [Command]
    public void CmdGetConn()
    {
        Debug.Log(connectionToClient.connectionId);
    }

}
