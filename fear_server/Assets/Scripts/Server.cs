using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_USER = 2; 
    private const int PORT = 26000;
    private const int BYTE_SIZE = 1024; // standard packet size
    
    private byte reliableChannel;
    private int hostId;
    private bool isStarted = false;
    private byte error; // general error byte, see documentation

    private DatabaseController dbCont;

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
        hostId = NetworkTransport.AddHost(topo, PORT, null);

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
            case NetOP.CreateAccount:
                Net_CreateAccount(connId, channelId, recHostId, (Net_CreateAccount)msg);
                break;
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
    private void Net_CreateAccount(int connId, int channelId, int recHostId, Net_CreateAccount msg)
    {
        Debug.Log($"Create account: UN {msg.Username}, PW {msg.Password}, EM {msg.Email}");
    }
    #endregion

    #region Send
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

    public void PropogateTroops()
    {
        Debug.Log("Sending troops");
        dbCont = new DatabaseController();
        List<Troop> troops = dbCont.GetAllTroops();
        dbCont.CloseDB();

        foreach (Troop t in troops)
        {
            Net_Propogate np = new Net_Propogate();
            np.Prefab = t.TroopType;
            np.AbsoluteXPos = t.XPos;
            np.AbsoluteZPos = t.ZPos;
            SendToClient(0, 1, np);
        }
        

    }

    #endregion

    public void TESTPROPOGATEANDSWITCH()
    {
        ChangeScene("Battlefield");
        PropogateTroops();

    }
}
