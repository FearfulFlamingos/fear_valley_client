using NUnit.Framework;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.Character;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestGamePlay
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
            Vector3 expectedEnemyPos = new Vector3(6, 0.2f, 6);
            
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


        [UnityTest]
        public IEnumerator TestPlayerHighlight()
        {
            GameObject newGameObject = GameObject
                .FindGameObjectWithTag("scripts")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures(),1,1);


            GameObject sceneController = GameObject.Find("SceneController");
            PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            spotScript.ChangeSelection(newGameObject);
            Assert.True(newGameObject.GetComponent<Character>().CurrentState == Character.State.Selected);
            //Assert.Equals(newGameObject.name, sceneController.GetComponent<GameLoop>().lastClicked.name);
            yield return null;
        }
            //[UnityTest]
            //public IEnumerator TestActivateCanvas()
            //{
            //    GameObject sceneController = GameObject.Find("SceneController");
            //    PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            //    spotScript.SpotlightChar(magicUser);
            //    var spotlightCanvas = GameObject.Find("InformationPanel");
            //    GameObject magicPanel = GameObject.Find("MagicPanel");
            //    GameObject stdPanel = GameObject.Find("Canvas/ActionsUIHolder/StandardPanel");

            //    Assert.True(spotlightCanvas.activeSelf);
            //    Assert.True(magicPanel.activeSelf);
            //    Assert.False(stdPanel.activeSelf);

            //    spotScript.DeactivateCurrentFocus();

            //    spotScript.SpotlightChar(trainedWarrior);
            //    Assert.True(spotlightCanvas.activeSelf);
            //    Assert.True(stdPanel.activeSelf);
            //    Assert.False(magicPanel.activeSelf);

            //}
            //public static void ClickButton(string name)
            //{
            //    // Find button Game Object
            //    var go = GameObject.Find(name);
            //    Assert.IsNotNull(go, "Missing button " + name);

            //    // Set it selected for the Event System
            //    EventSystem.current.SetSelectedGameObject(go);

            //    // Invoke click
            //    go.GetComponent<Button>().onClick.Invoke();
            //}

            //[UnityTest]
            //public IEnumerator TestActivateMovement()
            //{
            //    PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            //    var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
            //    yield return null;
            //    GameObject sceneController = GameObject.Find("SceneController");
            //    PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            //    spotScript.SpotlightChar(NewGameObject);
            //    yield return null;
            //    ClickButton("MoveButton");

            //    Assert.True(NewGameObject.GetComponent<PlayerMovement>().enabled);

            //}

            ///// The player "clicks" the attack button.
            ///// The info panel remains active, and the Attack panel is activated.
            ///// All other panels are inactive.
            //[UnityTest]
            //public IEnumerator TestAttack()
            //{
            //    PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            //    var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
            //    yield return null;

            //    GameObject sceneController = GameObject.Find("SceneController");
            //    GameObject attackPanel = GameObject.Find("/Canvas/ActionsUIHolder/AttackPanel");
            //    GameObject stdActionPanel = GameObject.Find("/Canvas/ActionsUIHolder/StandardPanel");
            //    GameObject magicActionPanel = GameObject.Find("/Canvas/ActionsUIHolder/MagicPanel");
            //    GameObject magicExplosionPanel = GameObject.Find("/Canvas/ActionsUIHolder/MagicExplosion");

            //    PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();

            //    spotScript.testing = true;
            //    spotScript.SpotlightChar(NewGameObject);
            //    yield return null;
            //    ClickButton("AttackButton");

            //    Assert.True(NewGameObject.GetComponent<PlayerAttack>().enabled);
            //    Assert.True(attackPanel.activeSelf);
            //    Assert.False(stdActionPanel.activeSelf);
            //    Assert.False(magicActionPanel.activeSelf);
            //    Assert.False(magicExplosionPanel.activeSelf);

            //    // Attack
            //}

            //[UnityTest]
            //public IEnumerator TestRetreat()
            //{
            //    SceneManager.LoadScene("Battlefield");
            //    yield return 3;
            //    PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            //    var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 0, 6, 0);
            //    yield return null;
            //    GameObject sceneController = GameObject.Find("SceneController");
            //    PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            //    spotScript.SpotlightChar(NewGameObject);
            //    yield return null;
            //    ClickButton("RetreatButton");

            //    Assert.Null(NewGameObject);

            //}

            ///Test movement
            ///Test ui text population
            ///test player is active
            ///test activate attack/ movement
            ///Look into testing server and client
            ///

        // Teardown runs after every test
        [TearDown]
        public void TearDown()
        {
            GameObject.Find("SceneController").GetComponent<GameLoop>().p1CharsDict.Clear();
            GameObject.Find("SceneController").GetComponent<GameLoop>().p2CharsDict.Clear();
        }
    }
}
