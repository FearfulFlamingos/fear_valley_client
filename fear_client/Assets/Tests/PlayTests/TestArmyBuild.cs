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


namespace PlayTests
{
    public class TestArmyBuild: MonoBehaviour
    {
        [OneTimeSetUp]
        public void Init()
        {
            Time.timeScale = 20;
            GameObject serverPref = new GameObject();
            serverPref.gameObject.name = "ServerJoinPrefs";
            serverPref.AddComponent<ServerPreferences>();
            serverPref.GetComponent<ServerPreferences>().SetValues("127.0.0.1", 50000);

            GameObject client = new GameObject();
            client.AddComponent<MonoClient>();
            SceneManager.LoadScene("ArmyBuild-Visual");
            Time.timeScale = 20f;
        }

        [UnityTest]
        public IEnumerator TestButtonActivation()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.ToggleClassButtons(false);
            yield return null;
            Assert.False(GameObject.Find("PeasantButton").GetComponent<Button>().interactable);
            holder.ToggleClassButtons(true);
            Assert.True(GameObject.Find("PeasantButton").GetComponent<Button>().interactable);
        }

        [UnityTest]
        public IEnumerator TestCreateCharacter()
        {
            // Arrange
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int)PopulateGrid.Troop.Peasant);
            holder.SetArmor((int)PopulateGrid.Armor.LightMundaneArmor);
            holder.SetWeapon((int)PopulateGrid.Weapon.Polearm);
            
            // Act
            holder.InstantiateTroop("PlaceablePeasant");
            GameObject NewObject = PopulateGrid.LastClicked;
            yield return null;
            
            // Assert
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenArmor, 
                PopulateGrid.Armor.LightMundaneArmor);
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenWeapon, 
                PopulateGrid.Weapon.Polearm);
            Assert.AreEqual(40, NewObject.GetComponent<FeaturesHolder>().Cost);
            yield return null;
        }


        [UnityTest]
        public IEnumerator TestCreateBlankCharacter()
        {
            // Arrange
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int)PopulateGrid.Troop.None);
            holder.SetArmor((int)PopulateGrid.Armor.Unarmored);
            holder.SetWeapon((int)PopulateGrid.Weapon.Unarmed);

            // Act
            holder.InstantiateTroop("PlaceablePeasant");
            GameObject NewObject = PopulateGrid.LastClicked;
            yield return null;

            // Assert
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenArmor,
                PopulateGrid.Armor.Unarmored);
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenWeapon,
                PopulateGrid.Weapon.Unarmed);
            Assert.AreEqual(0, NewObject.GetComponent<FeaturesHolder>().Cost);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestCreatePeasantWithHeavyMundaneAndTwoHanded()
        {
            // Arrange
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int)PopulateGrid.Troop.Peasant);
            holder.SetArmor((int)PopulateGrid.Armor.HeavyMundaneArmor);
            holder.SetWeapon((int)PopulateGrid.Weapon.TwoHandedWeapon);

            // Act
            holder.InstantiateTroop("PlaceablePeasant");
            GameObject NewObject = PopulateGrid.LastClicked;
            yield return null;

            // Assert
            Assert.AreEqual(PopulateGrid.Armor.HeavyMundaneArmor,
                NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenArmor);
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenWeapon,
                PopulateGrid.Weapon.TwoHandedWeapon);
            Assert.AreEqual(70, NewObject.GetComponent<FeaturesHolder>().Cost);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestCreateWarriorWithLightMagicAndOneHanded()
        {
            // Arrange
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int)PopulateGrid.Troop.TrainedWarrior);
            holder.SetArmor((int)PopulateGrid.Armor.LightMagicalArmor);
            holder.SetWeapon((int)PopulateGrid.Weapon.OneHandedWeapon);

            // Act
            holder.InstantiateTroop("PlaceablePeasant");
            GameObject NewObject = PopulateGrid.LastClicked;
            yield return null;

            // Assert
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenArmor,
                PopulateGrid.Armor.LightMagicalArmor);
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenWeapon,
                PopulateGrid.Weapon.OneHandedWeapon);
            Assert.AreEqual(95, NewObject.GetComponent<FeaturesHolder>().Cost);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestCreateWizardWithHeavyMagicalAndRanged()
        {
            // Arrange
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int)PopulateGrid.Troop.MagicUser);
            holder.SetArmor((int)PopulateGrid.Armor.HeavyMagicalArmor);
            holder.SetWeapon((int)PopulateGrid.Weapon.RangedAttack);

            // Act
            holder.InstantiateTroop("PlaceablePeasant");
            GameObject NewObject = PopulateGrid.LastClicked;
            yield return null;

            // Assert
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenArmor,
                PopulateGrid.Armor.HeavyMagicalArmor);
            Assert.AreEqual(NewObject.GetComponent<FeaturesHolder>().TroopInfo.ChosenWeapon,
                PopulateGrid.Weapon.RangedAttack);
            Assert.AreEqual(175, NewObject.GetComponent<FeaturesHolder>().Cost);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlaceCharacter()
        {
            // Arrange
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int)PopulateGrid.Troop.Peasant);
            holder.SetArmor((int)PopulateGrid.Armor.LightMundaneArmor);
            holder.SetWeapon((int)PopulateGrid.Weapon.Polearm);

            // Act
            holder.InstantiateTroop("PlaceablePeasant");
            GameObject NewObject = PopulateGrid.LastClicked;
            yield return null;

            IInputManager fakeInput = Substitute.For<IInputManager>();
            holder.InputManager = fakeInput;
            fakeInput.MousePosition().Returns(Camera.main.WorldToScreenPoint(new Vector3(4, 0.2f, 4)));
            fakeInput.GetLeftMouseClick().Returns(true);

            yield return new WaitForSeconds(4);
            Vector3 expected = new Vector3(4, 0.2f, 4);
            //Debug.Break();
            Vector3 actual = PopulateGrid.LastClicked.transform.position;
            // Assert
            Assert.AreEqual(expected.x, actual.x, 0.1f);
            Assert.AreEqual(expected.y, actual.y, 0.1f);
            Assert.AreEqual(expected.z, actual.z, 0.1f);
        }

        [UnityTest]
        public IEnumerator TestCancelCharacter()
        {
            // Arrange
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int)PopulateGrid.Troop.Peasant);
            holder.SetArmor((int)PopulateGrid.Armor.LightMundaneArmor);
            holder.SetWeapon((int)PopulateGrid.Weapon.Polearm);

            // Act
            holder.InstantiateTroop("PlaceablePeasant");
            yield return null;

            IInputManager fakeInput = Substitute.For<IInputManager>();
            fakeInput.GetCancelButtonDown().Returns(true);
            holder.InputManager = fakeInput;
            yield return null;
            // Assert
            Assert.AreEqual(0.0, holder.RollingBudget, 0.1f);


        }

        [UnityTest]
        public IEnumerator TestAddExplosion()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            yield return null;
            holder.AddExplosion();
            Assert.AreEqual(1, PopulateGrid.Explosions.Count);
            holder.AddExplosion();
            holder.AddExplosion();
            Assert.AreEqual(3, PopulateGrid.Explosions.Count);
        }

        [UnityTest]
        public IEnumerator TestRemoveExplosion()
        {
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            yield return null;
            PopulateGrid.Explosions.Clear();
            holder.AddExplosion();
            Assert.AreEqual(1, PopulateGrid.Explosions.Count);
            holder.RemoveExplosion();
            Assert.AreEqual(0, PopulateGrid.Explosions.Count);
        }



        [UnityTest]
        public IEnumerator TestRemoveObjectBeforeItIsPlaced()
        {
            // Arrange
            // Make a character
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int)PopulateGrid.Troop.Peasant);
            holder.SetArmor((int)PopulateGrid.Armor.LightMundaneArmor);
            holder.SetWeapon((int)PopulateGrid.Weapon.Polearm);
            holder.InstantiateTroop("PlaceablePeasant");

            Debug.Log("Active troops: " + holder.ActiveTroops.Count);
            Debug.Log($"Budget: {holder.Budget}");
            // move the mouse over the character and start right clicking
            IInputManager input = Substitute.For<IInputManager>();
            input.MousePosition().Returns(
                Camera.main.WorldToScreenPoint(PopulateGrid.LastClicked.transform.position));
            input.GetRightMouseClick().Returns(true);
            holder.InputManager = input;
            yield return null;

            // Assert
            Assert.AreEqual(0, holder.ActiveTroops.Count);
            Assert.AreEqual(0, holder.RollingBudget);
            Assert.AreEqual(300, holder.Budget);
            yield return null;
        }


        [UnityTest]
        public IEnumerator TestRemoveObjectAfterItIsPlaced()
        {
            // Arrange
            // Make a character
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int)PopulateGrid.Troop.Peasant);
            holder.SetArmor((int)PopulateGrid.Armor.LightMundaneArmor);
            holder.SetWeapon((int)PopulateGrid.Weapon.Polearm);
            holder.InstantiateTroop("PlaceablePeasant");

            Debug.Log("Active troops: " + holder.ActiveTroops.Count);
            Debug.Log($"Budget: {holder.Budget}");
            // move the mouse over the character and start right clicking
            IInputManager input = Substitute.For<IInputManager>();
            input.MousePosition().Returns(
                Camera.main.WorldToScreenPoint(PopulateGrid.LastClicked.transform.position));
            input.GetLeftMouseClick().Returns(true);
            holder.InputManager = input;
            yield return null;
            input.GetLeftMouseClick().Returns(false);
            yield return null;
            input.GetRightMouseClick().Returns(true);
            yield return null;

            // Assert
            Assert.AreEqual(0, holder.ActiveTroops.Count);
            Assert.AreEqual(0, holder.RollingBudget);
            Assert.AreEqual(300, holder.Budget);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestOverBudget()
        {
            // Arrange
            GameObject UIController = GameObject.Find("UIController");
            PopulateGrid holder = UIController.GetComponent<PopulateGrid>();
            holder.SetTroop((int) PopulateGrid.Troop.Peasant);
            holder.SetArmor((int) PopulateGrid.Armor.LightMundaneArmor);
            holder.SetWeapon((int) PopulateGrid.Weapon.Polearm);
            holder.Budget = 0;

            holder.InstantiateTroop("PlaceablePeasant");
            yield return null;

            // Act
            holder.ExecutePurchase();
            yield return null;
            
            Assert.AreEqual(0, holder.ActiveTroops.Count);
        }

        [TearDown]
        public void TearDown()
        { 
            foreach (var friendly in GameObject.FindGameObjectsWithTag("Friendlies"))
            {
                Object.Destroy(friendly);
            }
            GameObject.Find("UIController").GetComponent<PopulateGrid>().ActiveTroops.Clear();
            GameObject.Find("UIController").GetComponent<PopulateGrid>().Budget = 300;
            GameObject.Find("UIController").GetComponent<PopulateGrid>().RollingBudget = 0;
            GameObject.Find("UIController").GetComponent<PopulateGrid>().InputManager =
                GameObject.Find("scripts").GetComponent<InputManager>();
        }
    }
}
