using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System;

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

            NewGameObject.GetComponent<PlayerMovement>().DeactivateMovement();

            Assert.False(NewGameObject.GetComponent<PlayerMovement>().enabled);

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
            NewGameObject.GetComponent<PlayerAttack>().ActivateObjects();
            NewGameObject.GetComponent<PlayerAttack>().DeactivateAttack();
            GameLoop loopScript = sceneController.GetComponent<GameLoop>();
            loopScript.CancelAttack();

            Assert.False(NewGameObject.GetComponent<PlayerAttack>().enabled);

        }

		[UnityTest]
		public IEnumerator TestRetreat()
		{
			SceneManager.LoadScene("Battlefield");
			yield return 3;
			PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
			var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
			NewGameObject.name = "testwiz";
			yield return null;
			GameObject sceneController = GameObject.Find("SceneController");
			PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
			spotScript.SpotlightChar(NewGameObject);
			yield return null;
			ClickButton("RetreatButton");
			yield return 1;

			Assert.Null(GameObject.Find("testwiz"));

		}
        [UnityTest]
        public IEnumerator TestMovement(){
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
            PlayerMovement testmove = NewGameObject.GetComponent<PlayerMovement>();
            Vector3 newPos = new Vector3(4,0,4);
            testmove.MoveMe(newPos);
            yield return 4;
            Debug.Log(NewGameObject.transform.position);
            Assert.True(NewGameObject.transform.position == newPos);
        }
        [UnityTest]
        public IEnumerator TestAttack()
        {
            SceneManager.LoadScene("Battlefield");
            yield return 3;
            PopulateCharacter CreateFigure = new GameObject().AddComponent<PopulateCharacter>();
            var NewGameObject = CreateFigure.DuplicateObjects(1, "Magic User", 1, 1, 1, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
            var NewGameObject2 = CreateFigure.DuplicateObjects(2, "Peasant", 2, 2, 2, 6, 4, 0, 0, 0, 2, 6, 24, 2, 0);
            yield return null;
            GameObject sceneController = GameObject.Find("SceneController");
            PlayerSpotlight spotScript = sceneController.GetComponent<PlayerSpotlight>();
            spotScript.SpotlightChar(NewGameObject);
            yield return null;
            ClickButton("AttackButton");

            UnityEngine.Random.seed = 42;
            NewGameObject.GetComponent<PlayerAttack>().attackObject = NewGameObject2;
            NewGameObject.GetComponent<PlayerAttack>().canAttack = true;
            NewGameObject.GetComponent<PlayerAttack>().ActivateObjects();
            NewGameObject.GetComponent<PlayerAttack>().Attack();
            Debug.Log(NewGameObject2.GetComponent<CharacterFeatures>().health);

            Assert.True(NewGameObject2.GetComponent<CharacterFeatures>().health == 6);

        }
        // [UnityTest]
        // public IEnumerator TestPopulateGrid(){

        // }
        
	}

    //     /Test movement
    //     /Test ui text population
    //     /test player is active
    //     /test activate attack/ movement
    //     /Look into testing server and client
    //     /
    // }
}
