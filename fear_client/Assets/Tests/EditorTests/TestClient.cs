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

            controller = new GameObject("SceneController");
            controller.tag = "scripts";
            controller.AddComponent<GameLoop>();
            controller.AddComponent<PopulateCharacter>();


        }

        // setup
        [SetUp]
        public void CreateMonoServer()
        {
            clientObj = new GameObject("Client");
            clientObj.AddComponent<MonoClient>();
            MonoClient.Instance = new Client();
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
            Assert.AreEqual(1024,MonoClient.Instance.BYTE_SIZE);
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

        //[Test]
        //public void TestSendToServer()
        //{
        //    Net_ToggleControls ntc = new Net_ToggleControls();

        //    MonoClient.Instance.SendToServer(ntc);
        //}

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

        

        // tests
        /* Public fn() to test:
         *   void Init() ✓
         *   void Shutdown() ✓
         *   void UpdateMessagePump()
         *   void UpdateMessagePump() but it isn't started ✓
         *   void CheckMessageType(int, int, int, byte[], NetworkEventType) ✓
         *   bool HasControl() ✓
         *   void SendToServer()
         *   void SendTroopRequest()
         *   void SendFinishBuild()
         *   void SendMoveData()
         *   void SendAttackData()
         *   void SendRetreatData()
         *   void SendEndTurn()
         * 
         * Private fn() to test:
         *   void OnData() ✓
         *   Net_SendMagic() ✓
         *   Net_ToggleControls() ✓
         *   Net_Move() ==> Play test
         *   Net_Attack() ==> Play test
         *   Net_Retreat() ==> Play test
         *   Net_Propogate() ==> Play test
         *   Net_ChangeScene() ==> Play test
         */

        // teardown
        [TearDown]
        public void Teardown()
        {
            DestroyImmediate(clientObj);
        }

    }
}