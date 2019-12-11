using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public static Client Instance { private set; get; }
    private const int MAX_USER = 2;
    private const int PORT = 26000;
    private const string SERVER_IP = "127.0.0.1";
    private const int BYTE_SIZE = 1024; // standard packet size

    private byte reliableChannel;
    private int connectionId;
    private int hostId;
    private bool isStarted = false;
    private byte error;

    public GameObject uiController;

    #region Monobehavior
    private void Start()
    {
        Instance = this;
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
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        // Client only code
        hostId = NetworkTransport.AddHost(topo, 0, SERVER_IP);
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
        Debug.Log("Connecting from standalone");

        isStarted = true;
        Debug.Log($"Attempting to connnect on {SERVER_IP}...");
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
        switch (type)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("Connected to server");
                break;
            case NetworkEventType.DataEvent:
                Debug.Log("Data recieved");
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recievedBuffer);
                NetMsg msg = (NetMsg)formatter.Deserialize(ms);
                OnData(connectionId, channelId, recHostId, msg);
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Have been disconnected");
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
            case NetOP.ChangeScene:
                Debug.Log("NETOP: Change Scene");
                Net_ChangeScene(connId, channelId, recHostId, (Net_ChangeScene)msg);
                break;
            case NetOP.PropogateTroop:
                Debug.Log("NETOP: Propogate troop");
                Net_PropogateTroop(connId, channelId, recHostId, (Net_Propogate)msg);
                break;
        }
    }

    private void Net_PropogateTroop(int connId, int channelId, int recHostId, Net_Propogate msg)
    {
        
        Debug.Log($"Added troop {msg.Prefab}");
        PopulateCharacter popChar = new PopulateCharacter();
        //GameObject tile = (GameObject)Instantiate(Resources.Load(msg.Prefab));
        float varx = (float)msg.AbsoluteXPos;
        float varz = (float)msg.AbsoluteZPos;
        popChar.DuplicateObjects(msg.Prefab, varx, varz, 1, msg.Health, msg.AtkBonus, 0, 0, 0, msg.DefenseMod, 0, 6, 0);
        //tile.transform.position = new Vector3(varx, 0, varz);
    }

    private void Net_ChangeScene(int connId, int channelId, int recHostId, Net_ChangeScene msg)
    {
        SceneManager.LoadScene(msg.SceneName);
    }

    #endregion

    #region Send
    public void SendToServer(NetMsg msg)
    {
        // hold data to send
        byte[] buffer = new byte[BYTE_SIZE];

        // serialize to byte[]
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        NetworkTransport.Send(hostId,
            connectionId, 
            reliableChannel, 
            buffer, 
            BYTE_SIZE, 
            out error);
    }

    public void AddTroopRequest(string troop, string weapon, string armor, int xPos, int yPos)
    {
        Net_AddTroop at = new Net_AddTroop();
        at.TroopType = troop;
        at.WeaponType = weapon;
        at.ArmorType = armor;
        at.XPosRelative = xPos;
        at.ZPosRelative = yPos;

        SendToServer(at);
    }
    #endregion

    public void TESTFUNCTIONCREATEACCOUNT()
    {
        Net_CreateAccount ca = new Net_CreateAccount();
        ca.Username = "megaSwagXXX";
        ca.Password = "hunter2";
        ca.Email = "gmail@netscape.com";

        SendToServer(ca);
    }
}
