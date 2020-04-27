using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Networking;
using Scripts.Controller;
using FearValleyNetwork;
using NUnit.Framework;
using NSubstitute;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.TestTools;

namespace EditorTests
{
    public class TestClient : MonoBehaviour
    {
        GameObject clientObj;
        GameObject controller;
        // one time setup
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            GameObject serverPref = new GameObject("ServerJoinPrefs");
            serverPref.AddComponent<ServerPreferences>();
            ServerPreferences pref = serverPref.GetComponent<ServerPreferences>();
            pref.SetValues("127.0.0.1", 50000);

            //controller = new GameObject("SceneController");
            //controller.tag = "scripts";
            //controller.AddComponent<GameLoop>();
            //controller.AddComponent<PopulateCharacter>();


        }

        // setup
        [SetUp]
        public void CreateMonoServer()
        {
            clientObj = new GameObject("Client");
            clientObj.AddComponent<MonoClient>();
            MonoClient.Instance = new Client();
            MonoClient.Instance.Init();
        }

        [Test]
        public void TestClientInit()
        {
            // Arrange
            // Act
            MonoClient.Instance.Init();

            // Assert
            Assert.AreEqual(2,MonoClient.Instance.MAX_USER);
            Assert.AreEqual(50000,MonoClient.Instance.PORT);
            Assert.AreEqual("127.0.0.1",MonoClient.Instance.SERVER_IP);
            Assert.IsTrue(MonoClient.Instance.IsStarted);
        }

        [Test]
        public void TestClientInitWhereServerIsntLocal()
        {
            // Arrange
            GameObject serverPrefs = GameObject.Find("ServerJoinPrefs");
            serverPrefs.GetComponent<ServerPreferences>().SetValues("10.28.149.175", 50000);
            MonoClient.Instance.Shutdown();
            MonoClient.Instance = new Client();

            // Act
            MonoClient.Instance.Init();

            // Assert
            Assert.AreEqual(2, MonoClient.Instance.MAX_USER);
            Assert.AreEqual(50000, MonoClient.Instance.PORT);
            Assert.AreEqual("10.28.149.175", MonoClient.Instance.SERVER_IP);
            Assert.IsTrue(MonoClient.Instance.IsStarted);
        }

        [Test]
        public void TestClientShutdown()
        {
            MonoClient.Instance.Shutdown();

            Assert.IsFalse(MonoClient.Instance.IsStarted);
        }

        [Test]
        public void TestUpdateMessagePumpWhenServerIsntStarted()
        {
            MonoClient.Instance.IsStarted = false;
            MonoClient.Instance.UpdateMessagePump();

            Assert.IsFalse(MonoClient.Instance.IsStarted);
        }

        [Test]
        public void TestUpdateMessagePump()
        {
            //MonoClient.Instance.CheckMessageType(0,0,0,new byte[1],NetworkEventType.
        }

        [Test]
        public void TestIfClientHasControl()
        {
            bool actual = MonoClient.Instance.HasControl();
            Assert.IsFalse(actual);
        }

        [Test]
        public void TestMessageTypeOfNothing() 
        {
            MonoClient.Instance.CheckMessageType(0, 0, 0, new byte[1], NetworkEventType.Nothing);
            Assert.AreEqual(NetworkEventType.Nothing, MonoClient.Instance.LastEvent);
        }
        
        [Test]
        public void TestMessageTypeOfConnection() 
        {
            MonoClient.Instance.CheckMessageType(0, 0, 0, new byte[1], NetworkEventType.ConnectEvent);
            Assert.AreEqual(NetworkEventType.ConnectEvent,MonoClient.Instance.LastEvent);
        }
        
        [Test]
        public void TestMessageTypeOfBroadcast() 
        {
            MonoClient.Instance.CheckMessageType(0, 0, 0, new byte[1], NetworkEventType.BroadcastEvent);
            Assert.AreEqual(NetworkEventType.BroadcastEvent,MonoClient.Instance.LastEvent);
        }
        [Test]
        public void TestMessageTypeOfDisconnect()
        {
            MonoClient.Instance.CheckMessageType(0, 0, 0, new byte[1], NetworkEventType.DisconnectEvent);
            Assert.AreEqual(NetworkEventType.DisconnectEvent,MonoClient.Instance.LastEvent);
        }

        [Test]
        public void TestMessageTypeOfData()
        {
            // Arrange
            Net_ToggleControls ntc = new Net_ToggleControls();
            byte[] buffer = new byte[1024];
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, ntc);

            // Act
            MonoClient.Instance.CheckMessageType(0, 0, 0, buffer, NetworkEventType.DataEvent);
            bool actual = MonoClient.Instance.HasControl();

            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void TestSendToServer()
        {
            Net_ToggleControls ntc = new Net_ToggleControls();

            MonoClient.Instance.SendToServer(ntc);
            Assert.AreEqual(ntc,(Net_ToggleControls) MonoClient.Instance.LastSent);
        }

        [Test]
        public void TestSendTroopRequest()
        {
            // Arrange
            string troop = "Peasant";
            string weapon = "Unarmed";
            string armor = "Unarmored";
            float xPos = 1f;
            float yPos = 1f;
            Net_AddTroop expected = new Net_AddTroop()
            {
                TroopType = troop,
                WeaponType = weapon,
                ArmorType = armor,
                XPosRelative = xPos,
                ZPosRelative = yPos
            };

            // Act
            MonoClient.Instance.SendTroopRequest(troop, weapon, armor, xPos, yPos);

            // Assert
            Net_AddTroop actual = (Net_AddTroop)MonoClient.Instance.LastSent;
            Assert.AreEqual(expected.TroopType, actual.TroopType);
            Assert.AreEqual(expected.WeaponType, actual.WeaponType);
            Assert.AreEqual(expected.ArmorType, actual.ArmorType);
            Assert.AreEqual(expected.XPosRelative, actual.XPosRelative);
            Assert.AreEqual(expected.ZPosRelative, actual.ZPosRelative);
        }

        [Test]
        public void TestSendFinishBuild()
        {
            // Arrange
            int magic = 4;
            Net_FinishBuild expected = new Net_FinishBuild() { MagicBought = 4 };

            // Act
            MonoClient.Instance.SendFinishBuild(magic);

            // Assert
            Net_FinishBuild actual = (Net_FinishBuild) MonoClient.Instance.LastSent;
            Assert.AreEqual(expected.MagicBought, actual.MagicBought);
        }

        [Test]
        public void TestSendMoveData()
        {
            // Arrange
            int troop = 1;
            float x = 0;
            float z = 0;
            Net_MOVE expected = new Net_MOVE() { TroopID = troop, NewX = x, NewZ = z };

            // Act
            MonoClient.Instance.SendMoveData(troop, x, z);
            Net_MOVE actual = (Net_MOVE)MonoClient.Instance.LastSent;
            
            // Assert
            Assert.AreEqual(expected.TroopID,actual.TroopID);
            Assert.AreEqual(expected.NewX,actual.NewX);
            Assert.AreEqual(expected.NewZ, actual.NewZ);
        }

        [Test]
        public void TestSendAttackData()
        {
            // Arrange
            int troop = 1;
            int health = 4;
            Net_ATTACK expected = new Net_ATTACK() { TroopID = troop, DamageTaken = health };

            // Act
            MonoClient.Instance.SendAttackData(troop, health);
            Net_ATTACK actual = (Net_ATTACK)MonoClient.Instance.LastSent;

            // Assert
            Assert.AreEqual(expected.TroopID, actual.TroopID);
            Assert.AreEqual(expected.DamageTaken, actual.DamageTaken);
        }

        [Test]
        public void TestSendRetreatData()
        {
            // Arrange
            int troop = 1;
            int teamNum = 4;
            Net_RETREAT expected = new Net_RETREAT() { TroopID = troop, TeamNum = teamNum };

            // Act
            MonoClient.Instance.SendRetreatData(troop, teamNum, false);
            Net_RETREAT actual = (Net_RETREAT) MonoClient.Instance.LastSent;

            // Assert
            Assert.AreEqual(expected.TroopID, actual.TroopID);
            Assert.AreEqual(expected.TeamNum, actual.TeamNum);
        }

        [Test]
        public void TestEndTurn()
        {
            // Arrange
            Net_EndTurn expected = new Net_EndTurn();
            // Act
            MonoClient.Instance.SendEndTurn();
            Net_EndTurn actual = (Net_EndTurn)MonoClient.Instance.LastSent;
            // Assert
            Assert.AreEqual(expected.OperationCode, actual.OperationCode);
        }

        [Test]
        public void TestUnknownNetMessage()
        {
            Net_SendMagic message = new Net_SendMagic() { OperationCode = 0 };
            byte[] buffer = new byte[1024];
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, message);

            MonoClient.Instance.CheckMessageType(0, 0, 0, buffer, NetworkEventType.DataEvent);

            Assert.AreEqual(NetworkEventType.DataEvent, MonoClient.Instance.LastEvent);
        }

        [Test]
        public void TestNet_SendMagic()
        {
            // Arrange
            Net_SendMagic message = new Net_SendMagic() { MagicAmount = 1 };
            byte[] buffer = new byte[1024];
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, message);

            // Act
            MonoClient.Instance.CheckMessageType(0, 0, 0, buffer, NetworkEventType.DataEvent);

            // Assert
            Assert.AreEqual(1, GameLoop.MagicPoints);
        }

        [Test]
        public void TestNet_ToggleControls()
        {
            // Arrange
            Net_ToggleControls message = new Net_ToggleControls();
            byte[] buffer = new byte[1024];
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, message);

            // Act
            MonoClient.Instance.CheckMessageType(0, 0, 0, buffer, NetworkEventType.DataEvent);

            // Assert
            Assert.IsTrue(MonoClient.Instance.HasControl());
        }

        [Test]
        public void TestSendANewEmptyName()
        {
            // Arrange
            PlayerPrefs.DeleteKey("PlayerName");
            Net_UpdateEnemyName expected = new Net_UpdateEnemyName();

            // Act
            MonoClient.Instance.SendUpdatedName();
            Net_UpdateEnemyName actual = (Net_UpdateEnemyName)MonoClient.Instance.LastSent;

            // Assert
            Assert.AreEqual(expected.OperationCode, actual.OperationCode);
            Assert.AreEqual("Anonymous", actual.Name);
        }

        [Test]
        public void TestSendName()
        {
            // Arrange
            PlayerPrefs.SetString("PlayerName", "Test Name");
            Net_UpdateEnemyName expected = new Net_UpdateEnemyName();

            // Act
            MonoClient.Instance.SendUpdatedName();
            Net_UpdateEnemyName actual = (Net_UpdateEnemyName)MonoClient.Instance.LastSent;

            // Assert
            Assert.AreEqual(expected.OperationCode, actual.OperationCode);
            Assert.AreEqual("Test Name", actual.Name);
        }

        // tests
        /* Public fn() to test:
         *   void Init() ✓
         *   void Shutdown() ✓
         *   void UpdateMessagePump()
         *   void UpdateMessagePump() but it isn't started ✓
         *   void CheckMessageType(int, int, int, byte[], NetworkEventType) ✓
         *   bool HasControl() ✓
         *   void SendToServer() ✓
         *   void SendTroopRequest() ✓
         *   void SendFinishBuild() ✓
         *   void SendMoveData() ✓
         *   void SendAttackData() ✓
         *   void SendRetreatData() ✓
         *   void SendEndTurn() ✓
         * 
         * Private fn() to test:
         *   void OnData() ✓
         *   Net_SendMagic() ✓
         *   Net_ToggleControls() ✓
         *   Net_Move() ==> Play test ✓
         *   Net_Attack() ==> Play test ✓
         *   Net_Retreat() ==> Play test ✓
         *   Net_Propogate() ==> Play test ✓
         *   Net_ChangeScene() ==> Play test ✓
         */

        // teardown
        [TearDown]
        public void Teardown()
        {
            MonoClient.Instance.Shutdown();
            DestroyImmediate(clientObj);
        }

    }
}