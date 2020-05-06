using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using FearValleyNetwork;
using Scripts.Controller;
using Scripts.CharacterClass;

namespace Scripts.Networking
{
    /// <inheritdoc cref="IClient"/>
    public class Client : IClient
    {
        //public static IClient Instance { set; get; }
        // These can all be private, but for testing I set them public.
        private const int PORT = 50000;
        private byte error;
        public const int BYTE_SIZE = 1024; // standard packet size
        public bool hasControl;

        /// <inheritdoc cref="IClient.MAX_USER"/>
        public int MAX_USER { set; get; }
        /// <inheritdoc cref="IClient.SERVER_IP"/>
        public string SERVER_IP { set; get; }
        /// <inheritdoc cref="IClient.ReliableChannel"/>
        public byte ReliableChannel { set; get; }
        /// <inheritdoc cref="IClient.ConnectionId"/>
        public int ConnectionId { set; get; }
        /// <inheritdoc cref="IClient.HostId"/>
        public int HostId { set; get; }
        /// <inheritdoc cref="IClient.IsStarted"/>
        public bool IsStarted { set; get; }
        /// <inheritdoc cref="IClient.LastEvent"/>
        public NetworkEventType LastEvent { set; get; }
        /// <inheritdoc cref="IClient.LastSent"/>
        public NetMsg LastSent { set; get; }

        /// <inheritdoc cref="IClient.Init"/>
        public void Init()
        {
            MAX_USER = 2;
            SERVER_IP = GameObject.Find("/ServerJoinPrefs").GetComponent<ServerPreferences>().GetIP();

#pragma warning disable CS0618 // Type or member is obsolete
            NetworkTransport.Init();
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
            ConnectionConfig cc = new ConnectionConfig();
#pragma warning restore CS0618 // Type or member is obsolete
            ReliableChannel = cc.AddChannel(QosType.Reliable);

#pragma warning disable CS0618 // Type or member is obsolete
            HostTopology topo = new HostTopology(cc, MAX_USER);
#pragma warning restore CS0618 // Type or member is obsolete

            // Client only code

            if (SERVER_IP == "127.0.0.1")
            {
                Debug.Log("Running local instances");
#pragma warning disable CS0618 // Type or member is obsolete
                HostId = NetworkTransport.AddHost(topo, 0);
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
#pragma warning disable CS0618 // Type or member is obsolete
                HostId = NetworkTransport.AddHost(topo, PORT);
#pragma warning restore CS0618 // Type or member is obsolete
            }

            //Debug.Log(hostId);
#pragma warning disable CS0618 // Type or member is obsolete
            ConnectionId = NetworkTransport.Connect(HostId, SERVER_IP, PORT, 0, out error);
#pragma warning restore CS0618 // Type or member is obsolete
            //NetworkTransport.Connect(1, SERVER_IP, PORT, 0, out error);
            Debug.Log("Connecting from standalone");

            IsStarted = true;
            Debug.Log($"Attempting to connnect on {SERVER_IP}...");
        }

        /// <inheritdoc cref="IClient.Shutdown"/>
        public void Shutdown()
        {
            IsStarted = false;
#pragma warning disable CS0618 // Type or member is obsolete
            NetworkTransport.Shutdown();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <inheritdoc cref="IClient.UpdateMessagePump"/>
        public void UpdateMessagePump()
        {
            if (!IsStarted)
                return;

            int recHostId; // From web or standalone?
            int connectionId; // Which user?
            int channelId; // Which "lane"  is message coming through?

            byte[] recievedBuffer = new byte[BYTE_SIZE];
            int dataSize; // length of byte[] that data fills

#pragma warning disable CS0618 // Type or member is obsolete
            NetworkEventType type = NetworkTransport.Receive(out recHostId,
#pragma warning restore CS0618 // Type or member is obsolete
            out connectionId,
                out channelId,
                recievedBuffer,
                BYTE_SIZE,
                out dataSize,
                out error);

            CheckMessageType(recHostId, connectionId, channelId, recievedBuffer, type);
        }

        /// <inheritdoc cref="IClient.CheckMessageType(int, int, int, byte[], NetworkEventType)"/>
        public void CheckMessageType(int recHostId, int connectionId, int channelId, byte[] recievedBuffer, NetworkEventType type)
        {
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
            LastEvent = type;
        }

        /// <inheritdoc cref="IClient.HasControl"/>
        public bool HasControl() => hasControl;

        #region OnData
        // Blanket handler for all network data events.
        private void OnData(int connId, int channelId, int recHostId, NetMsg msg)
        {
            Debug.Log($"Recieved message of type {msg.OperationCode}");
            switch (msg.OperationCode)
            {
                case (byte)NetOP.Operation.None:
                    Debug.Log("Unexpected NETOP code");
                    break;
                case (byte)NetOP.Operation.ChangeScene:
                    Debug.Log("NETOP: Change Scene");
                    Net_ChangeScene(connId, channelId, recHostId, (Net_ChangeScene)msg);
                    break;
                case (byte)NetOP.Operation.PropogateTroop:
                    Debug.Log("NETOP: Propogate troop");
                    Net_PropogateTroop(connId, channelId, recHostId, (Net_Propogate)msg);
                    break;
                case (byte)NetOP.Operation.SendMagic:
                    Debug.Log("NETOP: Magic Recieved");
                    Net_SendMagic(connId, channelId, recHostId, (Net_SendMagic)msg);
                    break;
                case (byte)NetOP.Operation.MOVE:
                    Debug.Log("NETOP: Move Player");
                    Net_Move(connId, channelId, recHostId, (Net_MOVE)msg);
                    break;
                case (byte)NetOP.Operation.ATTACK:
                    Debug.Log("NETOP: Attack Player");
                    Net_Attack(connId, channelId, recHostId, (Net_ATTACK)msg);
                    break;
                case (byte)NetOP.Operation.RETREAT:
                    Debug.Log("NETOP: Retreat");
                    Net_Retreat(connId, channelId, recHostId, (Net_RETREAT)msg);
                    break;
                case (byte)NetOP.Operation.ToggleControls:
                    Debug.Log("NETOP: Toggle controls");
                    Net_ToggleControls(connId, channelId, recHostId, (Net_ToggleControls)msg);
                    break;
                case (byte)NetOP.Operation.UpdateEnemyName:
                    Debug.Log("NETOP: Update Enemy Name");
                    Net_UpdateEnemyName(connId, channelId, recHostId, (Net_UpdateEnemyName)msg);
                    break;
            }
        }

        // Updates the enemy's name in the battlefield scene.
        private void Net_UpdateEnemyName(int connId, int channelId, int recHostId, Net_UpdateEnemyName msg)
        {
            Debug.Log(msg.Name);
            BattleUIControl.Instance.UpdateEnemyName(msg.Name);
        }

        // Sets the number of spells that the player purchased.
        private void Net_SendMagic(int connId, int channelId, int recHostId, Net_SendMagic msg)
        {
            GameLoop.MagicPoints = msg.MagicAmount;
        }

        // Toggles the player's UI controls.
        private void Net_ToggleControls(int connId, int channelId, int recHostId, Net_ToggleControls msg)
        {
            hasControl = !hasControl;
            Debug.Log($"Toggling controls to {HasControl()}");
        }

        // Moves an enemy character to a new position.
        private void Net_Move(int connId, int channelId, int recHostId, Net_MOVE msg)
        {
            GameObject scripts = GameObject.FindGameObjectWithTag("scripts");
            GameLoop.Instance.MoveOther(msg.TroopID, msg.NewX, msg.NewZ);
        }

        // Updates a friendly character's health after it is attacked.
        private void Net_Attack(int connId, int channelId, int recHostId, Net_ATTACK msg)
        {
            GameObject scripts = GameObject.FindGameObjectWithTag("scripts");
            GameLoop.Instance.IveBeenAttacked(msg.TroopID, msg.DamageTaken);
        }

        // Causes a character to retreat.
        private void Net_Retreat(int connId, int channelId, int recHostId, Net_RETREAT msg)
        {
            GameObject scripts = GameObject.FindGameObjectWithTag("scripts");
            GameLoop.Instance.CharacterRemoval(msg.TroopID, msg.TeamNum);
        }

        // Propogates the troops to the client. Note that every client thinks they are connection #1.
        // Normally this helps with keeping track of channels, etc. But for our purposes, the server
        // makes sure that P1 has troops chosen by P1 and the rest go to P1's enemy.
        // APPARENTLY, some update from unity fixed the "clients think they're #1" issue. But I don't want to fix it,
        // so we're just going to replicate it.
        private void Net_PropogateTroop(int connId, int channelId, int recHostId, Net_Propogate msg)
        {

            Debug.Log($"Added troop {msg.TroopID}:{msg.Prefab}");
            GameObject sceneController = GameObject.FindGameObjectWithTag("scripts");
            PopulateCharacter popChar = sceneController.GetComponent<PopulateCharacter>();

            //PopulateCharacter popChar2 = new PopulateCharacter();
            //GameObject tile = (GameObject)Instantiate(Resources.Load(msg.Prefab));
            Debug.Log($"playerNum:{msg.ComingFrom},teamNum:{msg.TeamNum}");

            // This way of contstructing means that any values not set are left as default
            CharacterFeatures features = new CharacterFeatures()
            {
                Team = 1,
                Health = msg.Health,
                TroopId = msg.TroopID,
                Charclass = msg.Prefab,
                AttackBonus = msg.AtkBonus,
                DamageBonus = msg.DamageBonus,
                Movement = msg.Movement,
                ArmorBonus = msg.DefenseMod,
                AttackRange = msg.AtkRange,
                MaxAttackVal = msg.MaxAttackVal,
                Rng = new RandomNumberGenerator()
            };

            if (msg.TeamNum != msg.ComingFrom)
            {
                features.Team = 2;
            }
            popChar.DuplicateObjects(features, msg.AbsoluteXPos, msg.AbsoluteZPos);

            //tile.transform.position = new Vector3(varx, 0, varz);
        }

        // Changes the player's scene.
        private void Net_ChangeScene(int connId, int channelId, int recHostId, Net_ChangeScene msg)
        {
            SceneManager.LoadScene(msg.SceneName);
            if (msg.SceneName.Contains("Battlefield"))
                SendUpdatedName();
        }

        #endregion

        #region Send
        /// <inheritdoc cref="IClient.SendToServer(NetMsg)"/>
        public void SendToServer(NetMsg msg)
        {
            LastSent = msg;
            // hold data to send
            byte[] buffer = new byte[BYTE_SIZE];

            // serialize to byte[]
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, msg);

#pragma warning disable CS0618 // Type or member is obsolete
            NetworkTransport.Send(HostId,
#pragma warning restore CS0618 // Type or member is obsolete
            ConnectionId,
                ReliableChannel,
                buffer,
                BYTE_SIZE,
                out error);
        }

        /// <inheritdoc cref="IClient.SendTroopRequest(string, string, string, float, float)"/>
        public void SendTroopRequest(string troop, string weapon, string armor, float xPos, float yPos)
        {
            Net_AddTroop at = new Net_AddTroop
            {
                TroopType = troop,
                WeaponType = weapon,
                ArmorType = armor,
                XPosRelative = xPos,
                ZPosRelative = yPos
            };

            SendToServer(at);
        }

        /// <inheritdoc cref="IClient.SendFinishBuild(int)"/>
        public void SendFinishBuild(int magicAmount)
        {
            Net_FinishBuild fb = new Net_FinishBuild
            {
                MagicBought = magicAmount
            };
            //SendUpdatedName();
            SendToServer(fb);
        }

        /// <inheritdoc cref="IClient.SendMoveData(int, float, float)"/>
        public void SendMoveData(int TroopID, float newX, float newZ)
        {
            Net_MOVE mv = new Net_MOVE
            {
                TroopID = TroopID,
                NewX = newX,
                NewZ = newZ
            };
            SendToServer(mv);
        }
        
        /// <inheritdoc cref="IClient.SendAttackData(int, int)"/>
        public void SendAttackData(int troopId, int health)
        {
            Net_ATTACK atk = new Net_ATTACK
            {
                TroopID = troopId,
                DamageTaken = health
            };

            SendToServer(atk);
        }

        /// <inheritdoc cref="IClient.SendRetreatData(int, int, bool)"/>
        public void SendRetreatData(int troopId, int TeamNum, bool characterShouldDie)
        {
            Net_RETREAT ret = new Net_RETREAT
            {
                TroopID = troopId,
                TeamNum = TeamNum,
                ForceEnemyToRetreat = characterShouldDie
            };

            SendToServer(ret);
        }

        /// <inheritdoc cref="IClient.SendEndTurn"/>
        public void SendEndTurn()
        {
            //Client.Instance.hasControl = false;
            Net_EndTurn et = new Net_EndTurn();
            SendToServer(et);
        }

        /// <inheritdoc cref="IClient.SendUpdatedName"/>
        public void SendUpdatedName()
        {
            Net_UpdateEnemyName uen = new Net_UpdateEnemyName()
            {
                Name = NameSetter.SelectedName
            };

            SendToServer(uen);
        }
        #endregion

    }
}