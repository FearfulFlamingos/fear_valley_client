using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Scripts.Actions;
using Scripts.Networking;
using Scripts.CharacterClass;

/// <summary>
/// All scripts related to the battlefield scene.
/// </summary>
namespace Scripts.Controller
{
    /// <summary>
    /// Specifies all the actions that a character can take during thier turn.
    /// </summary>
    public class GameLoop : MonoBehaviour
    {
        private int numAttacks;
        public Dictionary<int, GameObject> p1CharsDict, p2CharsDict;
        
        /// <summary>Static instance of a GameLoop, as there can only be one.</summary>
        public static GameLoop Instance { set; get; }
        
        /// <summary>Player's current amount of action points.</summary>
        public static int ActionPoints { set; get; }
        
        /// <summary>Currently selected friendly character.</summary>
        public static GameObject SelectedCharacter { set; get; }
        
        /// <summary>Player's current amount of magic points.</summary>
        public static int MagicPoints { set; get; }

        #region Monobehavior
        // Start is called before the first frame update
        void Start()
        {
            ActionPoints = 3;
            p1CharsDict = new Dictionary<int, GameObject>();
            p2CharsDict = new Dictionary<int, GameObject>();
            if (Instance == null)
                Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (ActionPoints == 0)
            {
                ActionPoints = 3;
                numAttacks = 0;
                MonoClient.Instance.SendEndTurn();
            }

        }
        #endregion

        /// <summary>
        /// This function is used to create dictionaries for each player. The dicitionaries are used to track
        /// the number of active figures each player currently has and it tracks a reference to the gameobjects.
        /// </summary>
        /// <param name="team"></param>
        /// <param name="troopid">Incremental Id goes up by 1 for each new troop</param>
        /// <param name="reference">Reference to the GameObject</param>
        public void AddtoDict(int team, int troopid, GameObject reference)
        {
            if (team == 1)
            {
                p1CharsDict.Add(troopid, reference);
            }
            else
            {
                p2CharsDict.Add(troopid, reference);
            }
        }
        /// <summary>
        /// This is a networking function that is called by the client script eachtime the other player moves.
        /// Once this message is received the gameobject is retrieved from the dictionary and then the move
        /// function is triggered.
        /// </summary>
        /// <param name="troopid">ID of the troop to move.</param>
        /// <param name="newX">New X position for the troop to move to.</param>
        /// <param name="newZ">New Z position for the troop to move to.</param>
        public void MoveOther(int troopid, float newX, float newZ)
        {
            GameObject changing = p2CharsDict[troopid];
            //Debug.Log(newX);
            //Debug.Log(newZ);
            //float adjX = 4 + (4 - newX);
            //float adjZ = 4 + (4 - newZ);
            Vector3 newVect = new Vector3(newX, 0.2f, newZ);
            Debug.Log(newVect);
            changing.GetComponent<PlayerMovement>().Move(newVect);
        }
        /// <summary>
        /// This is another networking function and it currently is used to remove health from a specified
        /// troop after another player succesfully attacks them.
        /// </summary>
        /// <param name="troopid">Troop to be attacked.</param>
        /// <param name="damage">Amount of damage to take.</param>
        public void IveBeenAttacked(float troopid, int damage)
        {
            GameObject changing = p1CharsDict[Convert.ToInt32(troopid)];
            ICharacterFeatures reference = changing.GetComponent<Character>().Features;
            reference.DamageCharacter(damage);
        }

        #region Player Actions

        /// <summary>
        /// This function is used after the move buttion is clicked and it activates the movement script
        /// </summary>
        public void Move()
        {
            PlayerSpotlight.Instance.DisableCharacterSelect();
            SelectedCharacter.GetComponent<Character>().CurrentState = Character.State.Moving;
        }

        /// <summary>
        /// This is called after an attack is triggered and sends the message to the player attack script.
        /// </summary>
        public void Attack()
        {
            PlayerSpotlight.Instance.DisableCharacterSelect();

            string text = $"You are attacking with: {SelectedCharacter.GetComponent<Character>().Features.Charclass}";
            BattleUIControl.Instance.SetAttackPanelAttackerInfo(text);

            SelectedCharacter.GetComponent<Character>().CurrentState = Character.State.Attacking;
        }

        /// <summary>
        /// Actually attacks
        /// </summary>
        public void ConfirmAttack()
        {
            ActionPoints--;
            numAttacks++;
            SelectedCharacter.GetComponent<PlayerAttack>().Attack();
        }

        /// <summary>
        /// This function is called after the cancel attack button is pressed and it deactivates attack mode.
        /// </summary>
        public void CancelAttack()
        {
            SelectedCharacter.GetComponent<PlayerAttack>().CancelOrEndAttack();
            EndAttack();
        }
        /// <summary>
        /// This function is used after the end of an attack to deactivate attack mode and track the number
        /// of attacks.
        /// </summary>
        public void EndAttack()
        {
            PlayerSpotlight.Instance.DeactivateCurrentFocus();
        }

        /// <summary>
        /// Casts a spell if the player has enough magic points and action points.
        /// </summary>
        public void CastSpell()
        {
            if (ActionPoints < 3 || MagicPoints < 1)
            {
                Debug.Log("You do not have enough action points or magic to cast a spell!");
            }
            else
            {
                PlayerSpotlight.Instance.DisableCharacterSelect();
                SelectedCharacter.GetComponent<Character>().CurrentState = Character.State.CastingSpell;
            }

        }

        /// <summary>
        /// This function is called when the local player calls a retreat on their figure.
        /// See <see cref="CharacterRemoval(int, int)"/>.
        /// </summary>
        public void Leave()
        {
            if (ActionPoints >= 3)
            {
                // remove the character from this client's screen
                CharacterRemoval(SelectedCharacter.GetComponent<Character>().Features.TroopId, 1);
                // remove the character from the enemy's client
                MonoClient.Instance.SendRetreatData(SelectedCharacter.GetComponent<Character>().Features.TroopId, 2, false);
                ActionPoints = 0;
            }
            else
            {
                Debug.Log("You don't have enough action points to retreat.");
            }
        }

        /// <summary>
        /// This function is called when the game is finished and the player clicks the x button.
        /// </summary>
        public void EndGame()
        {
            SceneManager.LoadScene("MainMenu");
        }
        #endregion

        /// <summary>
        /// This function is checked each time a character dies or retreats. It removes them from the internal team dictionaries, 
        /// then checks if the endgame has been triggered.
        /// </summary>
        /// <param name="troopId">ID of the troop.</param>
        /// <param name="team">Team that the character was removed from.</param>
        public void CharacterRemoval(int troopId, int team)
        {
            Debug.Log("Called CharacterRemoval()");
            switch (team)
            {
                case 1:
                    Destroy(p1CharsDict[troopId]);
                    p1CharsDict.Remove(troopId);

                    break;
                case 2:
                    Debug.Log($"Deleting Enemy Character {troopId}");
                    Destroy(p2CharsDict[troopId]);
                    p2CharsDict.Remove(troopId);
                    break;
            }
            CheckVictoryState();
        }

        /// <summary>
        /// Check if either team is empty. 
        /// </summary>
        private void CheckVictoryState()
        {
            Debug.Log($"Called CheckVictoryState() {p1CharsDict.Count} friendlies remaining, {p2CharsDict.Count} enemies remaining");
            // You have lost
            if (p1CharsDict.Count == 0)
            {
                BattleUIControl.Instance.ToggleVictoryPanel(true);
                BattleUIControl.Instance.ToggleInfoPanel(false);
                string text = $"You have suffered a terrible defeat!";
                BattleUIControl.Instance.SetVictoryPanelText(text);
            }

            // You have won!
            else if (p2CharsDict.Count == 0)
            {
                Debug.Log("Victory was acheived");
                BattleUIControl.Instance.ToggleVictoryPanel(true);
                BattleUIControl.Instance.ToggleInfoPanel(false);
                string text = $"Victory has been acheived for \n{NameSetter.SelectedName}";
                BattleUIControl.Instance.SetVictoryPanelText(text);
            }
        }

    }
}