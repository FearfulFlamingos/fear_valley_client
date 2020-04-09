using NUnit.Framework;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.CharacterClass;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace PlayTests
{
    public class TestPropogate
    {

        [OneTimeSetUp]
        public void Init()
        {
            GameObject serverPref = new GameObject();
            serverPref.gameObject.name = "ServerJoinPrefs";
            serverPref.AddComponent<ServerPreferences>();
            serverPref.GetComponent<ServerPreferences>().SetValues("127.0.0.1", 50000);

            GameObject client = new GameObject();
            client.AddComponent<MonoClient>();
            SceneManager.LoadScene("Battlefield");
        }

        [UnityTest]
        public IEnumerator TestSpawnLocation()
        {
            // Arrange
            
            GameObject newGameObject = GameObject
                .FindGameObjectWithTag("scripts")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures(), 1, 1);
            GameObject enemyGameObject = GameObject
                .FindGameObjectWithTag("scripts")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures() { Team = 2 }, 1, 1);

            Vector3 expectedAllyPos = new Vector3(1, 0.2f, 1);
            Vector3 expectedEnemyPos = new Vector3(7, 0.2f, 7);
            
            // Act
            Vector3 actualAllyPos = newGameObject.transform.position;
            Vector3 actualEnemyPos = enemyGameObject.transform.position;
            Debug.Log(actualAllyPos);
            
            // Assert
            Assert.AreEqual(expectedAllyPos.x, actualAllyPos.x);
            Assert.AreEqual(expectedAllyPos.y, actualAllyPos.y,0.1f);
            Assert.AreEqual(expectedAllyPos.z, actualAllyPos.z);
            Assert.AreEqual(10, newGameObject.layer);
            Assert.AreEqual(expectedEnemyPos.x, actualEnemyPos.x);
            Assert.AreEqual(expectedEnemyPos.y, actualEnemyPos.y,0.1f);
            Assert.AreEqual(expectedEnemyPos.z, actualEnemyPos.z);
            Assert.AreEqual(11, enemyGameObject.layer);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestActivateSpotlightandLoop()
        {
            GameObject sceneController = GameObject.Find("SceneController");
            PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            GameLoop loopScript = sceneController.GetComponent<GameLoop>();
            yield return null;
            Assert.True(loopScript.enabled);
            Assert.True(spotScript.enabled);
        }

    }
}
