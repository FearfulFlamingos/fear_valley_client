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
    public class TestAttack : MonoBehaviour
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
            client.AddComponent<MonoClient>();
            MonoClient.Instance = Substitute.For<IClient>();
            MonoClient.Instance.HasControl().Returns(true);
            SceneManager.LoadScene("Battlefield");
        }

        // Test attacking check
        [UnityTest]
        public IEnumerator TestActivateAttack()
        {
            // Arrange
            GameObject character = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 1,
                    Charclass = "Peasant",
                    AttackRange = 3
                }, 1, 1);

            PlayerAttack characterAttack = character.GetComponent<PlayerAttack>();

            // Act
            characterAttack.ActivateAttack();

            // Assert
            Assert.IsTrue(characterAttack.Attacking());

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestIfEnemyCanBeAttackedReturnsTrue()
        {
            // Arrange
            GameObject character = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 1,
                    Charclass = "Peasant",
                    AttackRange = 20
                }, 1, 1);

            GameObject enemy = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 2,
                    Charclass = "Peasant",
                    AttackRange = 3
                }, 1, 1);
            Debug.Log(enemy.transform.position);

            PlayerAttack characterAttack = character.GetComponent<PlayerAttack>();

            IInputManager input = Substitute.For<IInputManager>();
            input.MousePosition().Returns(Camera.main.WorldToScreenPoint(enemy.transform.position));
            input.GetLeftMouseClick().Returns(true);
            characterAttack.InputManager = input;

            // Act
            characterAttack.ActivateAttack();
            yield return new WaitForSeconds(1);
            // Let the Update() loop run

            // Assert
            Assert.AreEqual(enemy, characterAttack.attackObject);
            Assert.IsTrue(characterAttack.CanAttack);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestIfEnemyCanBeAttackedReturnsFalse()
        {
            // Arrange
            GameObject character = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 1,
                    Charclass = "Peasant",
                    AttackRange = 3
                }, 1, 1);

            GameObject enemy = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 2,
                    Charclass = "Peasant",
                    AttackRange = 3
                }, 1, 1);
            Debug.Log(enemy.transform.position);

            PlayerAttack characterAttack = character.GetComponent<PlayerAttack>();

            IInputManager input = Substitute.For<IInputManager>();
            input.MousePosition().Returns(Camera.main.WorldToScreenPoint(enemy.transform.position));
            input.GetLeftMouseClick().Returns(true);
            characterAttack.InputManager = input;

            // Act
            characterAttack.ActivateAttack();
            yield return new WaitForSeconds(1);
            // Let the Update() loop run

            // Assert
            Assert.AreEqual(enemy, characterAttack.attackObject);
            Assert.IsFalse(characterAttack.CanAttack);

            yield return null;
        }

        // Test successful attack

        [UnityTest]
        public IEnumerator TestSuccessfulAttack()
        {
            // Arrange
            ICharacterFeatures friendlyCharacter = Substitute.For<ICharacterFeatures>();
            friendlyCharacter.GetAttackRoll().Returns(20);
            friendlyCharacter.GetDamageRoll().Returns(1);
            friendlyCharacter.Team.Returns(1);
            friendlyCharacter.Charclass.Returns("MagicUser");

            GameObject character = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(friendlyCharacter, 1, 1);

            ICharacterFeatures enemyCharacter = Substitute.For<ICharacterFeatures>();
            enemyCharacter.Health.Returns(10);
            enemyCharacter.ArmorBonus.Returns(1);
            enemyCharacter.Team.Returns(2);
            enemyCharacter.TroopId.Returns(1);
            enemyCharacter.Charclass.Returns("MagicUser");

            GameObject enemy = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(enemyCharacter, 1, 1);

            PlayerAttack playerAttack = character.GetComponent<PlayerAttack>();
            
            playerAttack.CanAttack = true;
            playerAttack.attackObject = enemy;
            GameLoop.SelectedCharacter = character;

            // Act
            playerAttack.Attack();

            // Assert
            // We already know that health / damage work from the editor tests.
            Assert.AreEqual(10, enemyCharacter.Health);

            yield return null;
        }


        [UnityTest]
        public IEnumerator TestFailedAttack()
        {
            // Arrange
            ICharacterFeatures friendlyCharacter = Substitute.For<ICharacterFeatures>();
            friendlyCharacter.GetAttackRoll().Returns(1);
            friendlyCharacter.GetDamageRoll().Returns(1);
            friendlyCharacter.Team.Returns(1);
            friendlyCharacter.Charclass.Returns("MagicUser");

            GameObject character = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(friendlyCharacter, 1, 1);

            ICharacterFeatures enemyCharacter = Substitute.For<ICharacterFeatures>();
            enemyCharacter.Health.Returns(10);
            enemyCharacter.ArmorBonus.Returns(20);
            enemyCharacter.Team.Returns(2);
            enemyCharacter.TroopId.Returns(1);
            enemyCharacter.Charclass.Returns("MagicUser");

            GameObject enemy = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(enemyCharacter, 1, 1);

            PlayerAttack playerAttack = character.GetComponent<PlayerAttack>();

            playerAttack.CanAttack = true;
            playerAttack.attackObject = enemy;
            GameLoop.SelectedCharacter = character;


            // Act
            playerAttack.Attack();
            yield return new WaitForSeconds(4);

            // Assert
            // We already know that health / damage work from the editor tests.
            Assert.AreEqual(10, enemyCharacter.Health);

            yield return null;
        }


        [UnityTest]
        public IEnumerator TestDeadlyAttack()
        {
            // Arrange 
            ICharacterFeatures friendlyCharacter = Substitute.For<ICharacterFeatures>();
            friendlyCharacter.GetAttackRoll().Returns(20);
            friendlyCharacter.GetDamageRoll().Returns(3);
            friendlyCharacter.Team.Returns(1);
            friendlyCharacter.Charclass.Returns("MagicUser");

            GameObject character = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(friendlyCharacter, 1, 1);

            ICharacterFeatures enemyCharacter = Substitute.For<ICharacterFeatures>();
            enemyCharacter.Health.Returns(0);
            enemyCharacter.ArmorBonus.Returns(1);
            enemyCharacter.Team.Returns(2);
            enemyCharacter.TroopId.Returns(1);
            enemyCharacter.Charclass.Returns("MagicUser");

            GameObject enemy = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(enemyCharacter, 1, 1);

            // We need two enemies in the scene so that the victory panel doesn't get triggered.
            // It's tested separately.
            GameObject enemy2 = GameObject.Find("SceneController")
                .GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures() { Team = 2, TroopId = 2, Charclass = "Peasant" }, 2, 2);

            PlayerAttack playerAttack = character.GetComponent<PlayerAttack>();

            playerAttack.CanAttack = true;
            playerAttack.attackObject = enemy;
            GameLoop.SelectedCharacter = character;

            // Act
            playerAttack.Attack();
            yield return new WaitForSeconds(4);

            // Assert
            foreach (var keyValuePair in GameLoop.Instance.p2CharsDict)
            {
                Debug.Log(keyValuePair.Key + ": " + keyValuePair.Value);
            }

            Assert.AreEqual(0, friendlyCharacter.Health);
            Assert.AreEqual(1, GameLoop.Instance.p2CharsDict.Count);

            yield return null;
        }


        [UnityTest]
        public IEnumerator TestUnableToAttackCharacter()
        {
            // Arrange
            GameObject character = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 1,
                    Charclass = "Peasant",
                    AttackRange = 3
                }, 1, 1);

            GameObject enemy = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 2,
                    Charclass = "Peasant",
                    AttackRange = 3
                }, 1, 1);
            Debug.Log(enemy.transform.position);

            PlayerAttack characterAttack = character.GetComponent<PlayerAttack>();
            characterAttack.CanAttack = false;
            characterAttack.attackObject = enemy;
            // Act
            BattleUIControl.Instance.ToggleInfoPanel(true);
            characterAttack.Attack();
            yield return new WaitForSeconds(4);

            // Assert
            string expected = "You can not attack this target\nthey are not in range. Select \nanother fighter to attack.";
            string actual = BattleUIControl.Instance.TESTGETATTACKPANELENEMYINFO();

            Assert.AreEqual(expected, actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestCancelAttack()
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
                        Charclass = "MagicUser"
                    },
                    1,
                    1);

            // Act
            IInputManager input = Substitute.For<IInputManager>();
            input.GetCancelButtonDown().Returns(true);
            character.GetComponent<PlayerAttack>().InputManager = input;
            GameLoop.SelectedCharacter = character;

            yield return new WaitForSeconds(3);

            // Assert
            Assert.IsFalse(character.GetComponent<Character>().Features.IsAttacking);

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