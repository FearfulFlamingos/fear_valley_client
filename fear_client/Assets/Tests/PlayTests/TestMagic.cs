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
    /// Tests both the PlayerMagic and BlowUpEnemies scripts.
    /// </summary>
    public class TestMagic : MonoBehaviour
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
        public IEnumerator TestMakeSuccessfulMagicAttack()
        {
            // Arrange 
            ICharacterFeatures friendlyCharacter = Substitute.For<ICharacterFeatures>();
            friendlyCharacter.GetMagicAttackRoll().Returns(20);
            friendlyCharacter.GetMagicDamageRoll().Returns(1);
            friendlyCharacter.Team.Returns(1);
            friendlyCharacter.Charclass.Returns("Magic User");

            GameObject character = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(friendlyCharacter, 1, 1);
            
            ICharacterFeatures enemyCharacter = Substitute.For<ICharacterFeatures>();
            enemyCharacter.Health.Returns(10);
            enemyCharacter.ArmorBonus.Returns(1);
            enemyCharacter.Team.Returns(2);
            enemyCharacter.TroopId.Returns(1);
            enemyCharacter.Charclass.Returns("Magic User");

            GameObject enemy = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(enemyCharacter, 1, 1);


            // Act
            character.GetComponent<PlayerMagic>().MagicAttack(enemy);

            // We already know that health / damage works from the editor tests

            // Assert
            Assert.AreEqual(10, friendlyCharacter.Health);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestFailMagicAttack()
        {
            // Arrange 
            ICharacterFeatures friendlyCharacter = Substitute.For<ICharacterFeatures>();
            friendlyCharacter.GetMagicAttackRoll().Returns(1);
            friendlyCharacter.Team.Returns(1);
            friendlyCharacter.Charclass.Returns("Magic User");

            GameObject character = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(friendlyCharacter, 1, 1);

            ICharacterFeatures enemyCharacter = Substitute.For<ICharacterFeatures>();
            enemyCharacter.Health.Returns(10);
            enemyCharacter.ArmorBonus.Returns(20);
            enemyCharacter.Team.Returns(2);
            enemyCharacter.TroopId.Returns(1);
            enemyCharacter.Charclass.Returns("Magic User");

            GameObject enemy = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(enemyCharacter, 1, 1);


            // Act
            character.GetComponent<PlayerMagic>().MagicAttack(enemy);

            // We already know that health / damage works from the editor tests

            // Assert
            Assert.AreEqual(10, friendlyCharacter.Health);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestDeadlyMagicAttack()
        {
            // Arrange 
            ICharacterFeatures friendlyCharacter = Substitute.For<ICharacterFeatures>();
            friendlyCharacter.GetMagicAttackRoll().Returns(20);
            friendlyCharacter.GetMagicDamageRoll().Returns(1);
            friendlyCharacter.Team.Returns(1);
            friendlyCharacter.Charclass.Returns("Magic User");

            GameObject character = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(friendlyCharacter, 1, 1);

            ICharacterFeatures enemyCharacter = Substitute.For<ICharacterFeatures>();
            enemyCharacter.Health.Returns(0);
            enemyCharacter.ArmorBonus.Returns(1);
            enemyCharacter.Team.Returns(2);
            enemyCharacter.TroopId.Returns(1);
            enemyCharacter.Charclass.Returns("Magic User");

            GameObject enemy = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(enemyCharacter, 1, 1);


            // Act
            character.GetComponent<PlayerMagic>().MagicAttack(enemy);
            GameLoop gameLoop = GameObject.Find("SceneController").GetComponent<GameLoop>();
            // We already know that health / damage works from the editor tests

            // Assert
            Assert.AreEqual(0, friendlyCharacter.Health);
            Assert.AreEqual(0, gameLoop.p2CharsDict.Count);

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