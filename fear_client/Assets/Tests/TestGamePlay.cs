using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class TestGamePlay
    {
        [UnityTest]
        public IEnumerator TestLocation()
        {
            SceneManager.LoadScene("Battlefield");
            yield return 3;
            PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            //var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 0, 6, 0);
            var NewGameObject = CreateFigure.DuplicateObjects(1,"Magic User",1,1,1,6,4,0,0,0,2,6,24,2,0);
            //var NewGameObject = Resources.Load("Magic User");
            //Debug.Log(NewGameObjectID);
            Debug.Log(NewGameObject);
            var location = NewGameObject.transform.position;
            var expected = new Vector3(1, 0.149999976F, 1);
            yield return null;

            var MagicPrefab = Resources.Load("Magic User");
            Debug.Log(location);
            var prefabOfSpawnedTroop = PrefabUtility.GetCorrespondingObjectFromSource(NewGameObject);
            Assert.AreEqual(expected[0], location[0]);
            Assert.AreEqual(expected[1], location[1]);
            Assert.AreEqual(expected[2], location[2]);
        }

        [UnityTest]
        public IEnumerator TestActivateSpotlightandLoop()
        {
            SceneManager.LoadScene("Battlefield");
            yield return 3;
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
            SceneManager.LoadScene("Battlefield");
            yield return 3;
            PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
            yield return null;
            GameObject sceneController = GameObject.Find("SceneController");
            PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            spotScript.SpotlightChar(NewGameObject);
            Assert.True(NewGameObject.GetComponent<CharacterFeatures>().isFocused);

        }
        [UnityTest]
        public IEnumerator TestActivateCanvas()
        {
            SceneManager.LoadScene("Battlefield");
            yield return 3;
            PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
            yield return null;
            GameObject sceneController = GameObject.Find("SceneController");
            PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            spotScript.SpotlightChar(NewGameObject);
            var spotlightCanvas = GameObject.Find("ActionsUI");
            Assert.True(spotlightCanvas.active);

        }
        public static void ClickButton(string name)
        {
            // Find button Game Object
            var go = GameObject.Find(name);
            Assert.IsNotNull(go, "Missing button " + name);

            // Set it selected for the Event System
            EventSystem.current.SetSelectedGameObject(go);

            // Invoke click
            go.GetComponent<Button>().onClick.Invoke();
        }

        [UnityTest]
        public IEnumerator TestActivateMovement()
        {
            SceneManager.LoadScene("Battlefield");
            yield return 3;
            PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
            yield return null;
            GameObject sceneController = GameObject.Find("SceneController");
            PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            spotScript.SpotlightChar(NewGameObject);
            yield return null;
            ClickButton("MoveButton");

            Assert.True(NewGameObject.GetComponent<PlayerMovement>().enabled);

        }

        [UnityTest]
        public IEnumerator TestActivateAttack()
        {
            SceneManager.LoadScene("Battlefield");
            yield return 3;
            PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
            yield return null;
            GameObject sceneController = GameObject.Find("SceneController");
            PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            spotScript.SpotlightChar(NewGameObject);
            yield return null;
            ClickButton("AttackButton");

            Assert.True(NewGameObject.GetComponent<PlayerAttack>().enabled);

        }

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
