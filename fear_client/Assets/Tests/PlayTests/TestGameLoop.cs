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
    public class TestGameLoop
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
            Time.timeScale = 20f;
        }


        [UnityTest]
        public IEnumerator TestMove()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            GameObject character = GameObject.Find("SceneController").GetComponent<PopulateCharacter>().DuplicateObjects(new CharacterFeatures() { Charclass = "Peasant" }, 1, 1);
            GameLoop.SelectedCharacter = character;
            GameLoop.Instance.Move();

            Assert.True(character.GetComponent<Character>().CurrentState == Character.State.Moving);


            yield return null;
        }

        [UnityTest]
        public IEnumerator TestAttack()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            GameObject character = GameObject.Find("SceneController").GetComponent<PopulateCharacter>().DuplicateObjects(new CharacterFeatures() { Charclass = "Peasant" }, 1, 1);
            GameLoop.SelectedCharacter = character;
            GameLoop.Instance.Attack();

            Assert.True(character.GetComponent<Character>().CurrentState == Character.State.Attacking);
            GameObject enemy = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 2,
                    Charclass = "Peasant",
                    AttackRange = 3
                }, 1, 1);
            PlayerAttack characterAttack = character.GetComponent<PlayerAttack>();
            characterAttack.attackObject = enemy;
            GameLoop.Instance.ConfirmAttack();

            Assert.True(GameLoop.ActionPoints == 2);


            yield return null;
        }

        [UnityTest]
        public IEnumerator TestCancelAttack()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            GameObject character = GameObject.Find("SceneController").GetComponent<PopulateCharacter>().DuplicateObjects(new CharacterFeatures() { Charclass = "Peasant" }, 1, 1);
            GameLoop.SelectedCharacter = character;
            Debug.Log(GameLoop.SelectedCharacter);
            GameLoop.Instance.CancelAttack();

            Assert.False(character.GetComponent<Character>().CurrentState == Character.State.Attacking);


            yield return null;
        }

        [UnityTest]
        public IEnumerator TestMagic()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            GameObject character = GameObject.Find("SceneController").GetComponent<PopulateCharacter>().DuplicateObjects(new CharacterFeatures() { Charclass = "MagicUser" }, 1, 1);
            GameLoop.SelectedCharacter = character;
            GameLoop.MagicPoints = 1;
            GameLoop.Instance.CastSpell();


            Assert.True(character.GetComponent<Character>().CurrentState == Character.State.CastingSpell);

            //GameLoop.Instance.ConfirmAttack();

            //Assert.True(GameLoop.ActionPoints == 2);


            yield return null;
        }


        [UnityTest]
        public IEnumerator TestRetreat()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            GameObject character = GameObject.Find("SceneController").GetComponent<PopulateCharacter>().DuplicateObjects(new CharacterFeatures() { Charclass = "MagicUser" }, 1, 1);
            GameLoop.SelectedCharacter = character;
            GameLoop.ActionPoints = 1;
            GameLoop.Instance.Leave();
            GameLoop.ActionPoints = 3;
            GameLoop.Instance.Leave();

            Assert.False(GameLoop.Instance.p1CharsDict.ContainsKey(character.GetComponent<Character>().Features.TroopId));
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestOthersLeave()
        {
            GameObject enemy = GameLoop.Instance.GetComponent<PopulateCharacter>().DuplicateObjects(
                new CharacterFeatures()
                {
                    Charclass = "MagicUser",
                    TroopId = 1,
                    Team = 2
                }
                , 1, 1);

            // Character exists so that victory is acheived
            GameObject character = GameLoop.Instance.gameObject.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    TroopId = 4,
                    Team = 1,
                    Charclass = "Peasant",
                    AttackRange = 3
                }, 1, 1);
            GameLoop.Instance.CharacterRemoval(enemy.GetComponent<Character>().Features.TroopId, 2);

            Assert.AreEqual(0, GameLoop.Instance.p2CharsDict.Count);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestEndgame()
        {
            GameLoop.Instance.EndGame();
            yield return null;
            yield return new WaitForSeconds(1);
            Assert.True(SceneManager.GetSceneByName("MainMenu").isLoaded);

            SceneManager.LoadScene("Battlefield");
            yield return new WaitForSeconds(1);
        }

        // This is a wierd one but bear with me
        [UnityTest]
        public IEnumerator TestEnemyIsKilledAndVictoryPanelActivates()
        {
            // Arrange
            GameObject character = GameLoop.Instance.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 1,
                    Charclass = "Peasant",
                    AttackBonus = 400,
                    DamageBonus = 100,
                    TroopId = 1,
                    Rng = new RandomNumberGenerator()
                }, 4, 4);

            GameObject enemy = GameLoop.Instance.GetComponent<PopulateCharacter>()
                .DuplicateObjects(new CharacterFeatures()
                {
                    Team = 2,
                    Charclass = "Peasant",
                    Health = 1,
                    ArmorBonus = 0,
                    TroopId = 2
                }, 4, 4);
            character.GetComponent<PlayerAttack>().attackObject = enemy;
            character.GetComponent<PlayerAttack>().CanAttack = true;
            GameLoop.SelectedCharacter = character;

            // Act
            character.GetComponent<PlayerAttack>().Attack();
            yield return new WaitForSeconds(2);
            // Assert
            bool actual = BattleUIControl.Instance.TESTGETVICTORYPANELSTATUS();

            Assert.IsTrue(actual);

            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            //GameLoop gameLoop = GameObject.Find("SceneController").GetComponent<GameLoop>();
            foreach (var friendly in GameLoop.Instance.p1CharsDict)
            {
                Object.Destroy(friendly.Value);
            }
            GameLoop.Instance.p1CharsDict.Clear();

            foreach (var enemy in GameLoop.Instance.p2CharsDict)
            {
                Object.Destroy(enemy.Value);
            }
            GameLoop.Instance.p2CharsDict.Clear();
            GameLoop.ActionPoints = 3;
            GameLoop.MagicPoints = 0;
        }
    }
}
