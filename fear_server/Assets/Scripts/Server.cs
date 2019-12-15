using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_USER = 2; 
    private const int PORT = 50000;
    private const int BYTE_SIZE = 1024; // standard packet size
    
    private byte reliableChannel;
    private int hostId;
    private bool isStarted = false;
    private byte error; // general error byte, see documentation

    private DatabaseController dbCont;
    private List<Troop> allTroops = new List<Troop>();

    #region Monobehavior
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Update()
    {
        UpdateMessagePump();
    }
    #endregion

    public void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable); // other channels available
        // need a QosType.ReliableFragmented if data needs to be bigger than 1024 bytes

        HostTopology topo = new HostTopology(cc, MAX_USER); // "map" of channels

        // Server only code
        hostId = NetworkTransport.AddHost(topo, PORT);

        isStarted = true;
        Debug.Log($"Started server on port {PORT}");

        // Clear out Army table for database
        dbCont = new DatabaseController();
        dbCont.Update("DELETE FROM Army;");
        dbCont.CloseDB();

        
    }                                         
    public void Shutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }

    public void UpdateMessagePump()
    {
        if (!isStarted)
            return;

        int recHostId; // From web or standalone?
        int connectionId; // Which user?
        int channelId; // Which "lane"  is message coming through?

        byte[] recievedBuffer = new byte[BYTE_SIZE];
        int dataSize; // length of byte[] that data fills

        NetworkEventType type = NetworkTransport.Receive(out recHostId, 
            out connectionId, 
            out channelId, 
            recievedBuffer, 
            BYTE_SIZE, 
            out dataSize, 
            out error);
        switch(type)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log($"User {connectionId} has connected through host {hostId}");
                break;
            case NetworkEventType.DataEvent:
                Debug.Log("Data recieved");
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recievedBuffer);
                NetMsg msg = (NetMsg)formatter.Deserialize(ms);
                OnData(connectionId, channelId, recHostId, msg);
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log($"User {connectionId} has disconnected");
                break;
            
            default:
            case NetworkEventType.BroadcastEvent:
                Debug.Log("Unexpected network event type");
                break;
        }
    }

    #region OnData
    private void OnData(int connId, int channelId, int recHostId, NetMsg msg)
    {
        Debug.Log($"Recieved message of type {msg.OperationCode}");
        switch (msg.OperationCode)
        {
            case NetOP.None:
                Debug.Log("Unexpected NETOP code");
                break;
            case NetOP.AddTroop:
                Debug.Log("NETOP: Add Troop to DB");
                Net_AddTroop(connId, channelId, recHostId, (Net_AddTroop)msg);
                break;
            case NetOP.FinishBuild:
                Debug.Log($"NETOP: Player {connId} is finished");
                Net_FinishBuild(connId, channelId, recHostId, (Net_FinishBuild)msg);
                break;
            case NetOP.MOVE:
                Debug.Log("NETOP: Move troop");
                Net_MOVE(connId, channelId, recHostId, (Net_MOVE)msg);
                break;
			case NetOP.ATTACK:
				Debug.Log("NETOP: Attack troop");
				Net_ATTACK(connId, channelId, recHostId, (Net_ATTACK)msg);
				break;
		}
    }

    private void Net_FinishBuild(int connId, int channelId, int recHostId, Net_FinishBuild msg)
    {
        throw new NotImplementedException();
    }

    private void Net_MOVE(int connId, int channelId, int recHostId, Net_MOVE msg)
    {
        if (connId == 1)
		{
			SendToClient(0, 2, msg);
            Debug.Log("Movement Sent");
        }
		else
		{
			SendToClient(0, 1, msg);
            Debug.Log("Movement Sent");
        }
    }
	private void Net_ATTACK(int connId, int channelId, int recHostId, Net_ATTACK msg)
	{
		if (connId == 1)
		{
			SendToClient(0, 2, msg);
		}
		else
		{
			SendToClient(0, 1, msg);
		}
	}


	private void Net_AddTroop(int connId, int channelId, int recHostId, Net_AddTroop msg)
    {
        dbCont.OpenDB();
        dbCont.AddTroopToDB(
            connId,
            msg.TroopType,
            msg.WeaponType,
            msg.ArmorType,
            msg.XPosRelative,
            msg.ZPosRelative);
        dbCont.CloseDB();
    }
    #endregion

    #region Send
    /// <summary>
    /// Sends a message to the client over the network.
    /// </summary>
    /// <param name="recHost">Recieving host socket type, typically 0.</param>
    /// <param name="connId">Connection to client.</param>
    /// <param name="msg">Any child of NetMsg, carries a payload.</param>
    public void SendToClient(int recHost, int connId, NetMsg msg)
    {
        // hold data to send
        byte[] buffer = new byte[BYTE_SIZE];

        // serialize to byte[]
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        int test = 0;
        for (int i = 0; i < BYTE_SIZE;i++)
        {
            if (buffer[i] != new byte())
                test++;
        }
        Debug.Log($"Transfer = {test} bytes");

        NetworkTransport.Send(hostId,
            connId,
            reliableChannel,
            buffer,
            BYTE_SIZE,
            out error);
        Debug.Log(error);
    }

    /// <summary>
    /// Changes the scene on the client.
    /// </summary>
    /// <param name="scene">Name of the scene to switch to.</param>
    public void ChangeScene(string scene)
    {
        Net_ChangeScene cs = new Net_ChangeScene();
        cs.SceneName = scene;
        for (int i = 1; i <= MAX_USER; i++)
        {
            try { SendToClient(0, i, cs); }
            catch(Exception e) { Debug.Log($"No user {i}"); }
        }
    }

    /// <summary>
    /// Reads DB for troops and sends info to clients.
    /// </summary>
    public void PropogateTroops()
    {
        Debug.Log("Sending troops");
        dbCont = new DatabaseController();
        allTroops = dbCont.GetAllTroops();
        dbCont.CloseDB();

        foreach (Troop t in allTroops)
        {
            Net_Propogate np = new Net_Propogate();
            np.Prefab = t.TroopType;
            np.TroopID = t.TroopID;
            np.TeamNum = t.TeamNum;
            np.AbsoluteXPos = t.XPos;
            np.AbsoluteZPos = t.ZPos;
            np.AtkBonus = t.TroopAtkBonus;
            np.Health = t.Health;
            np.MaxAttackVal = t.WeaponDamage;
            np.DefenseMod = t.Armor;
            np.ComingFrom = 1;
            SendToClient(0, 1, np);
            np.ComingFrom = 2;
			SendToClient(0, 2, np);
        }
    }

    /// <summary>
    /// Toggles whether the controls are enabled or disabled for the client.
    /// </summary>
    public void ToggleControls() //TODO: Add payload?
    {
        Net_ToggleControls tc = new Net_ToggleControls();
        for (int i = 1; i <= MAX_USER; i++)
        {
            try { SendToClient(0, i, tc); }
            catch (Exception e) { Debug.Log($"No user {i}"); }
        }
    }
    #endregion
    
    #region TEST FUNCTIONS
    /// <summary>
    /// These are test functions that are mapped to buttons in the server scene.
    /// </summary>

    public void TESTPROPOGATEANDSWITCH()
    {
        ChangeScene("Battlefield");
        PropogateTroops();
    }

    public void TESTTOGGLECONTROLS()
    {
        ToggleControls();
    }
    #endregion

}
