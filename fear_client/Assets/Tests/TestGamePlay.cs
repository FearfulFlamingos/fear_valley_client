using NUnit.Framework;
using Scripts.Controller;
using Scripts.Networking;
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

            Client client = new Client();
            SceneManager.LoadScene("Battlefield");
        }

        [UnityTest]
        public IEnumerator TestSpawnLocation()
        {
            // Arrange
            PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            var NewGameObject = CreateFigure.DuplicateObjects(1,"Magic User",1,1,1,6,4,0,0,0,2,6,24,2,0);
            var expected = new Vector3(1, 0.2f, 1);
            yield return null;
            // Act
            //var MagicPrefab = Resources.Load("Magic User");
            var location = NewGameObject.transform.position;
            Debug.Log(location);
            
            // Assert
            Assert.AreEqual(expected[0], location[0]);
            Assert.AreEqual(expected[1], location[1],0.1f);
            Assert.AreEqual(expected[2], location[2]);
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


        //[UnityTest]
        //public IEnumerator TestPlayerHighlight()
        //{
        //    PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
        //    var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
        //    yield return null;
        //    GameObject sceneController = GameObject.Find("SceneController");
        //    PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
        //    spotScript.SpotlightChar(NewGameObject);
        //    Assert.True(NewGameObject.GetComponent<CharacterFeatures>().isFocused);

        //}
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
    }
}
