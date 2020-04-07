﻿using NUnit.Framework;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.ArmyBuilder;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NSubstitute;

namespace Tests
{
    public class TestArmyBuild
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
            SceneManager.LoadScene("ArmyBuild-Visual");
        }

        [UnityTest]
        public IEnumerator TestButtonActivation()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.DeactivateButtons();
            yield return null;
            Assert.False(GameObject.Find("PeasantButton").GetComponent<Button>().interactable);
            holder.ActivateButtons();
            Assert.True(GameObject.Find("PeasantButton").GetComponent<Button>().interactable);


        }

        [UnityTest]
        public IEnumerator TestCreateCharacter()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.additem("Peasant");
            holder.GetArmor("Light mundane armor");
            holder.SetSelection("PlaceablePeasant");
            holder.GetWeapon("Polearm");
            GameObject NewObject = holder.lastclicked;
            yield return null;
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().armor, "Light mundane armor");
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().weapon, "Polearm");


        }
        [UnityTest]
        public IEnumerator TestPlaceCharacter()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.additem("Peasant");
            holder.GetArmor("Light mundane armor");
            holder.SetSelection("PlaceablePeasant");
            holder.GetWeapon("Polearm");
            yield return null;
            Debug.Log("here");
            Debug.Log(holder.selection);
            Debug.Log(holder.lastclicked);
            IInputManager fakeInput = Substitute.For<IInputManager>();
            holder.InputManager = fakeInput;
            fakeInput.MousePosition().Returns(Camera.main.WorldToScreenPoint(new Vector3(4, 0.2f, 4)));
            fakeInput.GetLeftMouseClick().Returns(true);

            yield return new WaitForSeconds(4);
            Vector3 expected = new Vector3(4, 0.2f, 4);
            Debug.Log(holder.lastclicked);
            Vector3 actual = holder.lastclicked.transform.position;
            Debug.Log(actual);
            // Assert
            Assert.AreEqual(expected.x, actual.x, 0.1f);
            Assert.AreEqual(expected.y, actual.y, 0.1f);
            Assert.AreEqual(expected.z, actual.z, 0.1f);
        }

        [UnityTest]
        public IEnumerator TestCancelCharacter()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.additem("Peasant");
            holder.GetArmor("Light mundane armor");
            holder.SetSelection("PlaceablePeasant");
            holder.GetWeapon("Polearm");
            yield return null;
            IInputManager fakeInput = Substitute.For<IInputManager>();
            holder.InputManager = fakeInput;
            fakeInput.GetEscapeKeyDown().Returns(true);
            yield return new WaitForSeconds(1);
            // Assert
            Assert.AreEqual(0.0, holder.rollingbudget, 0.1f);

        }

        [UnityTest]
        public IEnumerator TestAddExplosion()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            yield return null;
            holder.AddExplosion();
            Assert.AreEqual(1, holder.explosions.Count);
            holder.AddExplosion();
            holder.AddExplosion();
            Assert.AreEqual(3, holder.explosions.Count);
        }

        [UnityTest]
        public IEnumerator TestRemoveExplosion()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            yield return null;
            holder.explosions.Clear();
            holder.AddExplosion();
            Assert.AreEqual(1, holder.explosions.Count);
            holder.RemoveExplosion();
            Assert.AreEqual(0, holder.explosions.Count);
        }



        [UnityTest]
        public IEnumerator TestRemoveObject()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.additem("Peasant");
            holder.GetArmor("Light mundane armor");
            holder.SetSelection("PlaceablePeasant");
            holder.GetWeapon("Polearm");
            GameObject NewObject = holder.lastclicked;
            yield return null;
            IInputManager fakeInput2 = Substitute.For<IInputManager>();
            holder.InputManager = fakeInput2;
            fakeInput2.MousePosition().Returns(Camera.main.WorldToScreenPoint(new Vector3(4, 0.2f, 4)));
            fakeInput2.GetLeftMouseClick().Returns(true);
            Debug.Log(holder.activetroops.Count);
            yield return new WaitForSeconds(1);
            Debug.Log(holder.lastclicked);
            fakeInput2.MousePosition().Returns(Camera.main.WorldToScreenPoint(new Vector3(4, 0.2f, 4)));
            Debug.Log(holder.lastclicked);
            fakeInput2.GetRightMouseClick().Returns(true);

            Assert.AreEqual(0, holder.activetroops.Count);
        }


        [TearDown]
        public void TearDown()
        { 
            foreach (var friendly in GameObject.FindGameObjectsWithTag("Friendlies"))
            {
                Object.Destroy(friendly);
            }
            GameObject.Find("UIController").GetComponent<PopulateGrid>().activetroops.Clear();
        }
    }
}
