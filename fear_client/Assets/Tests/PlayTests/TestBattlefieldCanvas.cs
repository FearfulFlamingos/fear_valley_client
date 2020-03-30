using NUnit.Framework;
using Scripts.Actions;
using Scripts.CharacterClass;
using Scripts.Controller;
using Scripts.Networking;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace PlayTests
{
    public class TestBattlefieldCanvas : MonoBehaviour
    {
        [OneTimeSetUp]
        public void Init()
        {
            //GameObject serverPref = new GameObject();
            //serverPref.gameObject.name = "ServerJoinPrefs";
            //serverPref.AddComponent<ServerPreferences>();
            //serverPref.GetComponent<ServerPreferences>().SetValues("127.0.0.1", 50000);

            //GameObject client = new GameObject();
            //client.AddComponent<Client>();
            SceneManager.LoadScene("Battlefield");
        }

        [UnityTest]
        public IEnumerator SwitchToAttackPanel()
        {
            // Arrange
            BattleUIControl battleUIControl = GameObject.Find("SceneController").GetComponent<BattleUIControl>();


            GameObject attackPanel = GameObject.Find("/Canvas/ActionsUIHolder/AttackPanel");
            GameObject stdPanel = GameObject.Find("/Canvas/ActionsUIHolder/StandardPanel");
            GameObject magicPanel = GameObject.Find("/Canvas/ActionsUIHolder/MagicPanel");
            // Act
            battleUIControl.SwitchToAttackPanel();

            // Assert
            Assert.True(attackPanel.activeSelf);
            Assert.False(stdPanel.activeSelf);
            Assert.False(magicPanel.activeSelf);

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator SwitchToMagicPanel()
        {
            // Arrange
            BattleUIControl battleUIControl = GameObject.Find("SceneController").GetComponent<BattleUIControl>();
            GameObject magicActionPanel = GameObject.Find("/Canvas/ActionsUIHolder/MagicExplosion");
            GameObject stdPanel = GameObject.Find("/Canvas/ActionsUIHolder/StandardPanel");
            GameObject magicPanel = GameObject.Find("/Canvas/ActionsUIHolder/MagicPanel");
            // Act
            battleUIControl.Magic();

            // Assert
            Assert.True(magicActionPanel.activeSelf);
            Assert.False(stdPanel.activeSelf);
            Assert.False(magicPanel.activeSelf);

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator CancelAttackPanelFromPeasant()
        {
            // Arrange
            BattleUIControl battleUIControl = GameObject.Find("SceneController").GetComponent<BattleUIControl>();
            GameObject dummy = new GameObject();
            dummy.AddComponent<Character>();
            dummy.GetComponent<Character>().Features = new CharacterFeatures() { Charclass = "Peasant" };
            GameLoop.SelectedCharacter = dummy;

            GameObject attackPanel = GameObject.Find("/Canvas/ActionsUIHolder/AttackPanel");
            GameObject stdPanel = GameObject.Find("/Canvas/ActionsUIHolder/StandardPanel");
            // Act
            battleUIControl.CancelAttackPanel();

            // Assert
            Assert.False(attackPanel.activeSelf);
            Assert.True(stdPanel.activeSelf);

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator CancelAttackPanelFromMagicUser()
        {
            // Arrange
            BattleUIControl battleUIControl = GameObject.Find("SceneController").GetComponent<BattleUIControl>();
            GameObject dummy = new GameObject();
            dummy.AddComponent<Character>();
            dummy.GetComponent<Character>().Features = new CharacterFeatures() { Charclass = "Magic User" };
            GameLoop.SelectedCharacter = dummy;

            GameObject attackPanel = GameObject.Find("/Canvas/ActionsUIHolder/AttackPanel");
            GameObject magicPanel = GameObject.Find("/Canvas/ActionsUIHolder/MagicPanel");
            // Act
            battleUIControl.CancelAttackPanel();

            // Assert
            Assert.False(attackPanel.activeSelf);
            Assert.True(magicPanel.activeSelf);

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator CancelActionPanel()
        {
            // Arrange
            BattleUIControl battleUIControl = GameObject.Find("SceneController").GetComponent<BattleUIControl>();
            GameObject dummy = new GameObject();
            dummy.AddComponent<Character>();
            dummy.AddComponent<MeshRenderer>();
            GameLoop.SelectedCharacter = dummy;

            //GameObject infoPanel = GameObject.Find("InformationPanel");
            GameObject stdPanel = GameObject.Find("/Canvas/ActionsUIHolder/StandardPanel");
            GameObject magicPanel = GameObject.Find("/Canvas/ActionsUIHolder/MagicPanel");
            // Act
            battleUIControl.Cancel();

            // Assert
            //Assert.False(infoPanel.activeSelf);
            Assert.False(stdPanel.activeSelf);
            Assert.False(magicPanel.activeSelf);

            yield return null;
        }
    }
}