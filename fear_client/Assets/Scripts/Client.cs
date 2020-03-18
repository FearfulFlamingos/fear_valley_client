using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FearValleyNetwork;

public class Client : MonoBehaviour
{
    public static Client Instance { private set; get; }
    private const int MAX_USER = 2;
    private int PORT;
    private string SERVER_IP;
    private const int BYTE_SIZE = 1024; // standard packet size

    private byte reliableChannel;
    private int connectionId;
    private int hostId;
    private bool isStarted = false;
    private byte error;
    public bool hasControl;

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
        SERVER_IP = GameObject.Find("/ServerJoinPrefs").GetComponent<ServerPreferences>().GetIP();
        PORT = GameObject.Find("/ServerJoinPrefs").GetComponent<ServerPreferences>().GetPort();
        
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        // Client only code
        
        if (SERVER_IP == "127.0.0.1")
        {
            Debug.Log("Running local instances");
            hostId = NetworkTransport.AddHost(topo, 0);
        }
        else
        {
            hostId = NetworkTransport.AddHost(topo, PORT);
        }

        //Debug.Log(hostId);
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
        //NetworkTransport.Connect(1, SERVER_IP, PORT, 0, out error);
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
            case NetOP.SendMagic:
                Debug.Log("NETOP: Magic Recieved");
                Net_SendMagic(connId, channelId, recHostId, (Net_SendMagic)msg);
                break;
            case NetOP.MOVE:
                Debug.Log("NETOP: Move Player");
                Net_Move(connId, channelId, recHostId, (Net_MOVE)msg);
                break;
            case NetOP.ATTACK:
                Debug.Log("NETOP: Attack Player");
                Net_Attack(connId, channelId, recHostId, (Net_ATTACK)msg);
                break;
            case NetOP.RETREAT:
                Debug.Log("NETOP: Attack Player");
                Net_Retreat(connId, channelId, recHostId, (Net_RETREAT)msg);
                break;
            case NetOP.ToggleControls:
                Debug.Log("NETOP: Toggle controls");
                Net_ToggleControls(connId, channelId, recHostId, (Net_ToggleControls)msg);
                break;
        }
    }

    private void Net_SendMagic(int connId, int channelId, int recHostId, Net_SendMagic msg)
    {
        GameObject.FindGameObjectWithTag("scripts").GetComponent<GameLoop>().SetMagic(msg.MagicAmount);
    }

    private void Net_ToggleControls(int connId, int channelId, int recHostId, Net_ToggleControls msg)
    {
        Client.Instance.hasControl = !Client.Instance.hasControl;
        Debug.Log($"Toggling controls to {Client.Instance.hasControl}");
    }

    private void Net_Move(int connId, int channelId, int recHostId, Net_MOVE msg)
    {
        GameObject scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<GameLoop>().MoveOther(msg.TroopID, msg.NewX, msg.NewZ);
    }
    private void Net_Attack(int connId, int channelId, int recHostId, Net_ATTACK msg)
    {
        GameObject scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<GameLoop>().IveBeenAttacked(msg.TroopID,msg.NewHealth);
    }
    private void Net_Retreat(int connId, int channelId, int recHostId, Net_RETREAT msg)
    {
        GameObject scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<GameLoop>().OtherLeaves(msg.TroopID,msg.TeamNum);
    }
    /// <summary>
    /// Propogates the troops to the client. Note that every client thinks they are connection #1.
    /// Normally this helps with keeping track of channels, etc. But for our purposes, the server
    /// makes sure that P1 has troops chosen by P1 and the rest go to P1's enemy.
    /// </summary>
    /// <param name="connId"></param>
    /// <param name="channelId"></param>
    /// <param name="recHostId"></param>
    /// <param name="msg"></param>
    private void Net_PropogateTroop(int connId, int channelId, int recHostId, Net_Propogate msg)
    {
        
        Debug.Log($"Added troop {msg.TroopID}:{msg.Prefab}");
        PopulateCharacter popChar = new PopulateCharacter();
        //PopulateCharacter popChar2 = new PopulateCharacter();
        //GameObject tile = (GameObject)Instantiate(Resources.Load(msg.Prefab));
        float varx = (float)msg.AbsoluteXPos;
        float varz = (float)msg.AbsoluteZPos;
        Debug.Log($"playerNum:{msg.ComingFrom},teamNum:{msg.TeamNum}");
        if (msg.TeamNum == msg.ComingFrom)
        {
            popChar.DuplicateObjects(msg.TroopID,msg.Prefab, varx, varz, 1, msg.Health, msg.AtkBonus, msg.AtkRange, 0, msg.Movement, 0, msg.DefenseMod, 0, msg.MaxAttackVal, 0);
            //popChar.DuplicateObjects((msg.TroopID+1),msg.Prefab, varx, varz, 2, msg.Health, msg.AtkBonus, 0, 0, 0, msg.DefenseMod, 0, 6, 0);
        }
        else
        {
            popChar.DuplicateObjects(msg.TroopID,msg.Prefab, varx, varz, 2, msg.Health, msg.AtkBonus, msg.AtkRange, 0, msg.Movement, 0, msg.DefenseMod, 0, msg.MaxAttackVal, 0);
        }

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

    public void SendTroopRequest(string troop, string weapon, string armor, int xPos, int yPos)
    {
        Net_AddTroop at = new Net_AddTroop();
        at.TroopType = troop;
        at.WeaponType = weapon;
        at.ArmorType = armor;
        at.XPosRelative = xPos;
        at.ZPosRelative = yPos;

        SendToServer(at);
    }

    public void SendFinishBuild(int magicAmount)
    {
        Net_FinishBuild fb = new Net_FinishBuild();
        fb.MagicBought = magicAmount;
        SendToServer(fb);
    }

    public void SendMoveData(int TroopID, float newX, float newZ)
    {
        Net_MOVE mv = new Net_MOVE();
        mv.TroopID = TroopID;
        mv.NewX = newX;
        mv.NewZ = newZ;

        SendToServer(mv);
    }

    public void SendAttackData(int troopId, int health)
    {
        Net_ATTACK atk = new Net_ATTACK();
        atk.TroopID = troopId;
        atk.NewHealth = health;

        SendToServer(atk);
    }

    public void SendRetreatData(int troopId, int TeamNum)
    {
        Net_RETREAT ret = new Net_RETREAT();
        ret.TroopID = troopId;
        ret.TeamNum = TeamNum;

        SendToServer(ret);
    }

    public void SendEndTurn()
    {
        //Client.Instance.hasControl = false;
        Net_EndTurn et = new Net_EndTurn();
        SendToServer(et);
    }

#endregion

}
