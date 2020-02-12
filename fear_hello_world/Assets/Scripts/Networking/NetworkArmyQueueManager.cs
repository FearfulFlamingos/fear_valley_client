using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkArmyQueueManager : NetworkBehaviour
{
    public Queue<string> updates = new Queue<string>();
    // Start is called before the first frame update

    public void Add(string command)
    {
        updates.Enqueue(command);
    }

    [Command]
    public void CmdExecute()
    {
        if (!isLocalPlayer)
            return;
        while ( updates.Count > 0)
        {
            string command = updates.Dequeue();
            DatabaseController dbCont = new DatabaseController();
            dbCont.update(command);
            dbCont.CloseDB();
        }
    }

    public void TestQueue()
    {
        for (int i = 0; i < 10; i++)
            this.Add(i.ToString());
        Debug.Log(this.ToString());

    }
}
