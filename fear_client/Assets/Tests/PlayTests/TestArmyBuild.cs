using NUnit.Framework;
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
    public class TestArmyBuild: MonoBehaviour
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
            IInputManager fakeInput = Substitute.For<IInputManager>();
            holder.InputManager = fakeInput;
            fakeInput.MousePosition().Returns(Camera.main.WorldToScreenPoint(new Vector3(4, 0.2f, 4)));
            fakeInput.GetLeftMouseClick().Returns(true);

            yield return new WaitForSeconds(4);
            Vector3 expected = new Vector3(4, 0.2f, 4);
            //Debug.Break();
            Vector3 actual = holder.lastclicked.transform.position;
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
            fakeInput.GetCancelButtonDown().Returns(true);
            yield return new WaitForSeconds(3);
            fakeInput.GetCancelButtonDown().Returns(false);
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
            // Arrange
            // Make a character
            GameObject character = (GameObject)Instantiate(Resources.Load("PlaceablePeasant"));
            character.transform.position = new Vector3(4, 0.2f, 4);
            character.name = "TEST CHARACTER";
            FeaturesHolder featuresHolder = character.GetComponent<FeaturesHolder>();
            featuresHolder.cost = 40;

            // Set the budget, "place" the character
            PopulateGrid holder = GameObject.Find("UIController").GetComponent<PopulateGrid>();
            holder.activetroops.Add(character);
            holder.budget = 260;

            Debug.Log("Active troops: " + holder.activetroops.Count);

            // move the mouse over the character and start right clicking
            IInputManager input = Substitute.For<IInputManager>();
            input.MousePosition().Returns(Camera.main.WorldToScreenPoint(character.transform.position));
            input.GetRightMouseClick().Returns(true);
            holder.InputManager = input;
            yield return new WaitForSeconds(3);
            // Act
            //holder.RemovePlacedObject();

            // Assert
            Assert.AreEqual(0, holder.activetroops.Count);
            Assert.AreEqual(300, holder.budget);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestOverBudget()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.budget = 0;
            holder.additem("Peasant");
            holder.GetArmor("Light mundane armor");
            holder.SetSelection("PlaceablePeasant");
            holder.GetWeapon("Polearm");
            yield return null;
            IInputManager fakeInput = Substitute.For<IInputManager>();
            holder.InputManager = fakeInput;
            fakeInput.MousePosition().Returns(Camera.main.WorldToScreenPoint(new Vector3(4, 0.2f, 4)));
            fakeInput.GetLeftMouseClick().Returns(true);
            yield return new WaitForSeconds(3);
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
