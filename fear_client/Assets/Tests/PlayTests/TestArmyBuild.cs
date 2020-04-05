using NUnit.Framework;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.ArmyBuilder;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

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
            GameObject panel = GameObject.FindGameObjectWithTag("TestPanel");
            Debug.Log(panel);
            holder.GetCurrentPanel(panel);
            GameObject NewObject = holder.lastclicked;
            yield return null;
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().armor, "Light mundane armor");
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().weapon, "Polearm");

        }
    }
}
