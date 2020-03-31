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
            Assert.AreEqual(10, enemyCharacter.Health);

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
            Assert.AreEqual(10, enemyCharacter.Health);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestDeadlyMagicAttack()
        {
            Debug.Log("Starting TestDeadlyMagic");
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
            enemyCharacter.Charclass.Returns("Magic user");

            GameObject enemy = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(enemyCharacter, 1, 1);
            
            // We need two enemies in the scene so that the victory panel doesn't get triggered.
            // It's tested separately.
            GameObject enemy2 = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures() { Team = 2 }, 2, 2);

            // Act
            character.GetComponent<PlayerMagic>().MagicAttack(enemy);
            // We already know that health / damage works from the editor tests

            // Assert
            Assert.AreEqual(0, friendlyCharacter.Health);
            Assert.AreEqual(1, GameLoop.Instance.p2CharsDict.Count);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestCancelExplosion()
        {
            // Arrange
            GameObject character = GameLoop
                .Instance
                .gameObject
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(
                    new CharacterFeatures()
                    {
                        Team = 1,
                        Charclass = "Magic User"
                    },
                    1,
                    1);

            IInputManager input = Substitute.For<IInputManager>();
            input.GetRightMouseClick().Returns(true);
            character.GetComponent<PlayerMagic>().InputManager = input;

            Vector3 target = Camera.main.WorldToScreenPoint(new Vector3(5, 0.2f, 5));
            input.MousePosition().Returns(target);
            BattleUIControl.Instance.ToggleMagicInstructions(true);
            // Act
            character.GetComponent<PlayerMagic>().PlaceExplosion();
            
            yield return new WaitForSeconds(5);
            bool actual = GameObject.Find("/Canvas/ActionsUIHolder/MagicExplosion").activeSelf;

            // Assert
            Assert.IsFalse(actual);


            yield return null;
        }

        [UnityTest]
        public IEnumerator TestExplosion()
        {
            // Arrange
            GameObject character = GameLoop
                .Instance
                .gameObject
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(
                    new CharacterFeatures()
                    {
                        Team = 1,
                        Charclass = "Magic User"
                    },
                    1,
                    1);

            IInputManager input = Substitute.For<IInputManager>();
            input.GetLeftMouseClick().Returns(true);
            character.GetComponent<PlayerMagic>().InputManager = input;

            Vector3 target = Camera.main.WorldToScreenPoint(new Vector3(5, 0.2f, 5));
            input.MousePosition().Returns(target);
            BattleUIControl.Instance.ToggleMagicInstructions(true);
            GameLoop.SelectedCharacter = character;
            // Act
            character.GetComponent<PlayerMagic>().PlaceExplosion();
            
            yield return new WaitForSeconds(5);
            bool actual = GameObject.Find("/Canvas/ActionsUIHolder/MagicExplosion").activeSelf;

            // Assert
            Assert.IsFalse(actual);


            yield return null;
        }

        [UnityTest]
        public IEnumerator TestExplosionMarkerCollisionTriggerEnter()
        {
            // Arrange
            
            GameObject character = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 1,
                    Charclass = "Magic User"
                }, 1, 1);
            GameObject enemy = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 2,
                    Charclass = "Peasant"
                }, 4, 4);

            IInputManager input = Substitute.For<IInputManager>();
            input.MousePosition().Returns(Camera.main.WorldToScreenPoint(new Vector3(4.9f, 0.2f, 4.9f)));
            character.GetComponent<PlayerMagic>().InputManager = input;
            yield return null;

            // Act
            character.GetComponent<PlayerMagic>().PlaceExplosion();
            yield return null;
            int expected = 1;
            int actual = character.GetComponent<PlayerMagic>().selection
                .GetComponent<BlowUpEnemies>().EnemiesInBlast.Count;

            // Assert
            Assert.AreEqual(expected, actual);

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