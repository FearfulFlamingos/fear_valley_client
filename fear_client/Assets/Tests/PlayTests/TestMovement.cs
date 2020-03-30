using NUnit.Framework;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.CharacterClass;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NSubstitute;
using Scripts.Actions;

namespace PlayTests
{
    /// <summary>
    /// What do we know about movement?
    /// 
    /// We know that a character should be able to select a new position if they are in the movement state
    /// 
    /// We know that if the character is an enemy, their movement should be mirrored to the actual position.
    /// 
    /// </summary>
    public class TestMovement : MonoBehaviour
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Time.timeScale = 20f;
            GameObject serverPref = new GameObject();
            serverPref.gameObject.name = "ServerJoinPrefs";
            serverPref.AddComponent<ServerPreferences>();
            serverPref.GetComponent<ServerPreferences>().SetValues("127.0.0.1", 50000);

            GameObject client = new GameObject();
            client.AddComponent<Client>();
            Client.Instance = Substitute.For<IClient>();
            Client.Instance.HasControl().Returns(true);
            SceneManager.LoadScene("Battlefield");
        }

        [UnityTest]
        public IEnumerator TestCheckForInput()
        {
            // Arrange
            IInputManager fakeInput = Substitute.For<IInputManager>();
            fakeInput.GetLeftMouseClick().Returns(true);
            GameObject character = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures(), 1, 1);
            character.GetComponent<PlayerMovement>().InputManager = fakeInput;

            // Act
            bool expected = true;
            bool actual = character.GetComponent<PlayerMovement>().InputDetected();

            // Assert
            Assert.AreEqual(expected, actual);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestMoveToNewLocation()
        {
            // Arrange
            GameObject character = GameObject.Find("SceneController")
              .GetComponent<PopulateCharacter>()
              .DuplicateObjects(new CharacterFeatures() { Charclass = "Peasant" }, 1, 1);


            // Act
            Vector3 position = new Vector3(5, 0.2f, 5);
            character.GetComponent<PlayerMovement>().Move(position);
            Vector3 expected = new Vector3(5, 0.2f, 5);
            yield return new WaitForSeconds(4);

            // Wait for movement to occur before checking new position
            Vector3 actual = character.transform.position;

            // Assert
            Assert.AreEqual(expected.x, actual.x);
            Assert.AreEqual(expected.y, actual.y, 0.1f);
            Assert.AreEqual(expected.z, actual.z);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestMoveEnemyToNewLocation()
        {
            // Arrange
            GameObject enemy = GameObject.Find("SceneController")
              .GetComponent<PopulateCharacter>()
              .DuplicateObjects(new CharacterFeatures() { Team = 2, Charclass = "Peasant" }, 1, 1);


            // Act
            Vector3 position = new Vector3(5, 0.2f, 5);
            enemy.GetComponent<PlayerMovement>().Move(position);
            Vector3 expected = new Vector3(3, 0.2f, 3);
            yield return new WaitForSeconds(4);

            // Wait for movement to occur before checking new position
            Vector3 actual = enemy.transform.position;

            // Assert
            Assert.AreEqual(expected.x, actual.x);
            Assert.AreEqual(expected.y, actual.y, 0.1f);
            Assert.AreEqual(expected.z, actual.z);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestAllMovement()
        {
            // Arrange
            GameObject character = GameObject.Find("SceneController")
              .GetComponent<PopulateCharacter>()
              .DuplicateObjects(new CharacterFeatures() { Charclass = "Peasant" }, 2, 2);
            IInputManager input = Substitute.For<IInputManager>();
            character.GetComponent<PlayerMovement>().InputManager = input;
            input.MousePosition().Returns(Camera.main.WorldToScreenPoint(new Vector3(1,0.2f,1)));
            input.GetLeftMouseClick().Returns(true);
            GameLoop.SelectedCharacter = character;

            // Act
            character.GetComponent<PlayerMovement>().ActivateMovement();
            yield return new WaitForSeconds(5);

            Vector3 expected = new Vector3(1, 0.2f, 1);
            Vector3 actual = character.transform.position;
            // Assert
            Assert.AreEqual(expected.x, actual.x, 0.1f);
            Assert.AreEqual(expected.y, actual.y, 0.1f);
            Assert.AreEqual(expected.z, actual.z, 0.1f);

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