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
    public class TestSpotlight : MonoBehaviour
    {
        [OneTimeSetUp]
        public void Init()
        {
            GameObject serverPref = new GameObject();
            serverPref.gameObject.name = "ServerJoinPrefs";
            serverPref.AddComponent<ServerPreferences>();
            serverPref.GetComponent<ServerPreferences>().SetValues("127.0.0.1", 50000);

            GameObject client = new GameObject();
            client.AddComponent<Client>();
            SceneManager.LoadScene("Battlefield");
        }

        [UnityTest]
        public IEnumerator TestPlayerHighlight()
        {
            GameObject newGameObject = GameObject
                .FindGameObjectWithTag("scripts")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures(), 1, 1);


            GameObject sceneController = GameObject.Find("SceneController");
            PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            spotScript.ChangeSelection(newGameObject);
            Assert.True(newGameObject.GetComponent<Character>().CurrentState == Character.State.Selected);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestHighlightAndSwitchSelection()
        {
            GameObject newGameObject = GameObject
                .FindGameObjectWithTag("scripts")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures(), 1, 1);
            GameObject secondPlayer = GameObject
                .FindGameObjectWithTag("scripts")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures() { TroopId = 2 }, 1, 1);


            GameObject sceneController = GameObject.Find("SceneController");
            PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            spotScript.ChangeSelection(newGameObject);
            spotScript.ChangeSelection(secondPlayer);
            Assert.False(newGameObject.GetComponent<Character>().CurrentState == Character.State.Selected);
            Assert.True(secondPlayer.GetComponent<Character>().CurrentState == Character.State.Selected);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestHighlightUnselectByClickingSameGameObjectAgain()
        {
            // Arrange
            GameObject newGameObject = GameObject
                .FindGameObjectWithTag("scripts")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures(), 1, 1);
            PlayerSpotlight spotlight = GameObject.Find("SceneController").GetComponent<PlayerSpotlight>();

            // Act
            // first selection
            spotlight.ChangeSelection(newGameObject);
            spotlight.ChangeSelection(newGameObject);

            // Assert
            Assert.False(newGameObject.GetComponent<Character>().CurrentState == Character.State.None);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestSelectionColorChange()
        {
            GameObject newGameObject = GameObject
                .FindGameObjectWithTag("scripts")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures(), 1, 1);
            PlayerSpotlight spotlight = GameObject.Find("SceneController").GetComponent<PlayerSpotlight>();

            // Act
            spotlight.ChangeSelection(newGameObject);
            Color expected = Color.red;
            Color actual = newGameObject.GetComponent<Renderer>().material.color;

            // Assert
            Assert.AreEqual(expected, actual);
            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Find("SceneController").GetComponent<GameLoop>().p1CharsDict.Clear();
            GameObject.Find("SceneController").GetComponent<GameLoop>().p2CharsDict.Clear();
        }

    }
}