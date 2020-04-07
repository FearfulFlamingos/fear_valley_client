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
using Scripts.CharacterClass;
using Scripts.Actions;

namespace PlayTests
{
    public class TestClient : MonoBehaviour
    {

        GameObject clientObj;
        
        // one time setup
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Time.timeScale = 20f;
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
            MonoClient.Instance.Init();
        }

        [UnityTest]
        public IEnumerator TestNet_Propogate()
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
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestNet_PropogateWithEnemy()
        {
            // Arrange
            Net_Propogate message = new Net_Propogate()
            {
                TroopID = 1,
                TeamNum = 2,
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
            Assert.AreEqual(1, GameLoop.Instance.p2CharsDict.Count);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestMoveCharacter()
        {
            // Arrange
            GameObject character = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures() { TroopId = 1, Charclass = "Peasant", Team = 2}, 1, 1);

            Net_MOVE message = new Net_MOVE()
            {
                TroopID = 1,
                NewX = 2,
                NewZ = 2
            };
            byte[] buffer = new byte[1024];
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, message);

            // Act
            MonoClient.Instance.CheckMessageType(0, 1, 1, buffer, NetworkEventType.DataEvent);
            yield return new WaitForSeconds(4);
            Vector3 expected = new Vector3(6, 0.2f, 6);
            Vector3 actual = character.transform.position; 

            // Assert
            Assert.AreEqual(expected.x,actual.x,0.1f);
            Assert.AreEqual(expected.y,actual.y,0.1f);
            Assert.AreEqual(expected.z,actual.z,0.1f);
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestNet_Attack()
        {
            Net_ATTACK message = new Net_ATTACK() { TroopID = 1, DamageTaken = 1 }; 
            GameObject character = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures() { TroopId = 1, Charclass = "Peasant", Team = 1, Health = 10 }, 1, 1);

            byte[] buffer = new byte[1024];
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, message);

            // Act
            MonoClient.Instance.CheckMessageType(0, 1, 1, buffer, NetworkEventType.DataEvent);

            // Assert
            Assert.AreEqual(9, character.GetComponent<Character>().Features.Health);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestNet_Retreat()
        {
            Net_RETREAT message = new Net_RETREAT() { TroopID = 1, TeamNum =  1 };
            GameObject character = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures() { TroopId = 1, Charclass = "Peasant", Team = 1 }, 1, 1);
            
            // These two aren't used but prevent the victory panel from appearing.
            GameObject enemy = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures() { TroopId = 2, Charclass = "Peasant", Team = 2 }, 1, 1);
            GameObject extraFriendly = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures() { TroopId = 3, Charclass = "Peasant", Team = 1 }, 1, 1);

            byte[] buffer = new byte[1024];
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, message);

            // Act
            MonoClient.Instance.CheckMessageType(0, 1, 1, buffer, NetworkEventType.DataEvent);

            // Assert
            Assert.AreEqual(1, GameLoop.Instance.p1CharsDict.Count);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestNet_ChangeScene()
        {
            Net_ChangeScene message = new Net_ChangeScene() { SceneName = "Scenes/ServerConnect" };
            byte[] buffer = new byte[1024];
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            formatter.Serialize(ms, message);

            // Act
            MonoClient.Instance.CheckMessageType(0, 1, 1, buffer, NetworkEventType.DataEvent);
            yield return new WaitForSeconds(5);

            string expected = "ServerConnect";
            string actual = SceneManager.GetActiveScene().name;
            // Assert
            Assert.AreEqual(expected, actual);

            SceneManager.LoadScene("Battlefield");
            yield return new WaitForSeconds(4);

            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            GameLoop gameLoop = GameObject.Find("SceneController").GetComponent<GameLoop>();
            foreach (var friendly in gameLoop.p1CharsDict)
            {
                Destroy(friendly.Value);
            }
            gameLoop.p1CharsDict.Clear();

            foreach (var enemy in gameLoop.p2CharsDict)
            {
                Destroy(enemy.Value);
            }
            gameLoop.p2CharsDict.Clear();
        }

    }
}