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

namespace PlayTests
{
    public class TestClient : MonoBehaviour
    {

        GameObject clientObj;
        
        // one time setup
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            GameObject serverPref = new GameObject("ServerJoinPrefs");
            serverPref.AddComponent<ServerPreferences>();
            ServerPreferences pref = serverPref.GetComponent<ServerPreferences>();
            pref.SetValues("127.0.0.1", 50000);

            SceneManager.LoadScene("Battlefield");
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
        public void TestNet_Propogate()
        {
            // Arrange
            Net_Propogate message = new Net_Propogate()
            {
                TroopID = 1,
                TeamNum = 1,
                Health = 1,
                Prefab = "Peasant",
                AtkBonus = 1,
                Movement = 2,
                DefenseMod = 10,
                AtkRange = 3,
                MaxAttackVal = 6,
                ComingFrom = 1,
                AbsoluteXPos = 1,
                AbsoluteZPos = 1
            };
            byte[] buffer = new byte[1024];
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, message);

            // Act
            MonoClient.Instance.CheckMessageType(1, 0, 0, buffer, NetworkEventType.DataEvent);

            // Assert
            Assert.AreEqual(1, GameLoop.Instance.p1CharsDict.Count);

        }

        [Test]




    }
}