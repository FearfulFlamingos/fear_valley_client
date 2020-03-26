using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Scripts.Actions;
using Scripts.Networking;
using Scripts.Character;

namespace Scripts.Controller
{
    public class GameLoop : MonoBehaviour
    {
        public int actionPoints;
        public int magicPoints;
        public GameObject lastClicked;
        public Dictionary<int, GameObject> p1CharsDict, p2CharsDict;
        private int numAttacks;
        private PlayerSpotlight spotlight;
        private BattleUIControl uiController;

        #region Monobehavior
        // Start is called before the first frame update
        void Start()
        {
            actionPoints = 3;
            spotlight = gameObject.GetComponent<PlayerSpotlight>();
            p1CharsDict = new Dictionary<int, GameObject>();
            p2CharsDict = new Dictionary<int, GameObject>();
            uiController = gameObject.GetComponent<BattleUIControl>();
        }

        // Update is called once per frame
        /// <summary>
        /// This function is constantly checking if action points have dipped below 0 at which point the next turn is triggered
        /// </summary>
        void Update()
        {
            if (actionPoints == 0)
            {
                actionPoints = 3;
                numAttacks = 0;
                Client.Instance.SendEndTurn();
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
        /// <param name="troopid"></param>
        /// <param name="newX"></param>
        /// <param name="newZ"></param>
        public void MoveOther(int troopid, float newX, float newZ)
        {
            GameObject changing = p2CharsDict[troopid];
            //Debug.Log(newX);
            //Debug.Log(newZ);
            //float adjX = 4 + (4 - newX);
            //float adjZ = 4 + (4 - newZ);
            Vector3 newVect = new Vector3(newX, 0, newZ);
            Debug.Log(newVect);
            changing.GetComponent<PlayerMovement>().MoveMe(newVect);
        }
        /// <summary>
        /// This is another networking function and it currently is used to remove health from a specified
        /// troop after another player succesfully attacks them.
        /// </summary>
        /// <param name="troopid"></param>
        /// <param name="damage"></param>
        public void IveBeenAttacked(float troopid, float damage)
        {
            GameObject changing = p1CharsDict[Convert.ToInt32(troopid)];
            CharacterFeatures reference = changing.GetComponent<CharacterFeatures>();
            reference.Health = Convert.ToInt32(reference.Health - damage);
        }

        #region Build Game

        internal void SetMagic(int magicAmount)
        {
            magicPoints = magicAmount;
        }

        #endregion

        #region Player Actions

        /// <summary>
        /// This function is used after the move buttion is clicked and it activates the movement script
        /// </summary>
        public void Move()
        {
            spotlight.DisableCharacterSelect();
            lastClicked.GetComponent<Character.Character>().CurrentState = Character.Character.State.Moving;
        }

        /// <summary>
        /// This is called after an attack is triggered and sends the message to the player attack script.
        /// </summary>
        public void Attack()
        {
            spotlight.DisableCharacterSelect();

            string text = $"You are attacking with: {lastClicked.GetComponent<CharacterFeatures>().Charclass}";
            uiController.SetAttackPanelAttackerInfo(text);

            lastClicked.GetComponent<PlayerAttack>().enabled = true;
            //lastClicked.GetComponent<PlayerAttack>().Attack();
        }

        /// <summary>
        /// Actually attacks
        /// </summary>
        public void ConfirmAttack()
        {
            actionPoints--;
            numAttacks++;
            lastClicked.GetComponent<PlayerAttack>().Attack();
        }

        /// <summary>
        /// This function is called after the cancel attack button is pressed and it deactivates attack mode.
        /// </summary>
        public void CancelAttack()
        {
            spotlight.DeactivateCurrentFocus();
            lastClicked.GetComponent<PlayerAttack>().DeactivateAttack();
            EndAttack();
        }
        /// <summary>
        /// This function is used after the end of an attack to deactivate attack mode and track the number
        /// of attacks.
        /// </summary>
        public void EndAttack()
        {
            lastClicked.GetComponent<PlayerAttack>().enabled = false;
            spotlight.DeactivateCurrentFocus();
        }

        /// <summary>
        /// Casts a spell if the player has enough magic points and action points.
        /// </summary>
        public void CastSpell()
        {
            if (actionPoints < 3 || magicPoints < 1)
            {
                Debug.Log("You do not have enough action points or magic to cast a spell!");
            }
            else
            {
                magicPoints--;
                actionPoints -= 3;
                spotlight.DisableCharacterSelect();
                spotlight.DeactivateCurrentFocus();

                lastClicked.GetComponent<PlayerMagic>().PlaceExplosion();
            }

        }

        /// <summary>
        /// Cancels the spell and returns the action points and action points.
        /// </summary>
        public void CancelSpell()
        {
            magicPoints++;
            actionPoints += 3;
            uiController.CancelMagicExplosion();
        }

        /// <summary>
        /// This function is called when the local player calls a retreat on their figure.
        /// See <see cref="OtherLeaves(int, int)"/>
        /// </summary>
        public void Leave()
        {
            if (actionPoints >= 3)
            {
                PlayerRemoval(lastClicked.GetComponent<CharacterFeatures>().TroopId, 1, true);
                Destroy(lastClicked);
                Client.Instance.SendRetreatData(lastClicked.GetComponent<CharacterFeatures>().TroopId, 2);
                actionPoints = 0;
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
            SceneManager.LoadScene("ArmyBuilder");
        }
        #endregion

        #region Enemy Actions
        /// <summary>
        /// This is a networking function used when other players call a retreat in the game.
        /// </summary>
        /// <param name="troopId">Troop retreating</param>
        /// <param name="TeamNum">The team that is leaving</param>
        /// See <see cref="Leave"/>
        public void OtherLeaves(int troopId, int TeamNum)
        {
            Debug.Log($"Retreat message received with {TeamNum} and {troopId}");
            if (TeamNum == 1)
            {
                GameObject destroy = p1CharsDict[troopId];
                PlayerRemoval(troopId, TeamNum);
                Destroy(destroy);
            }

            else
            {
                GameObject destroy = p2CharsDict[troopId];
                PlayerRemoval(troopId, TeamNum, true);
                Destroy(destroy);
            }
        }


        #endregion

        /// <summary>
        /// This function is checked each time a character dies. It removes them from the internal team dictionaries, then checks if the endgame has been triggered.
        /// </summary>
        /// <remarks>
        /// This used to have a separate set of checks for if the enemy/player retreated thier last troop,
        /// but I think it's unnecessary. 
        /// </remarks>
        /// <param name="troopId"></param>
        /// <param name="team"></param>
        /// <param name="retreat"></param>
        public void PlayerRemoval(int troopId, int team, bool retreat = false)
        {
            switch (team)
            {
                case 1:
                    p1CharsDict.Remove(troopId);
                    break;
                case 2:
                    p2CharsDict.Remove(troopId);
                    break;
            }
            CheckVictoryState(team);
        }

        /// <summary>
        /// Check if either team is empty. 
        /// </summary>
        /// <param name="team"></param>
        private void CheckVictoryState(int team)
        {
            // Since we know the team that the person died on, we only need to check if that dictionary is empty.
            switch (team)
            {
                case 1:
                    // You have lost
                    if (p1CharsDict.Count == 0) 
                    {
                        uiController.ToggleVictoryPanel(true);
                        uiController.ToggleInfoPanel(false);
                        string text = $"Victory has been acheived for \nplayer 2 after defeating player 1 ";
                        uiController.SetVictoryPanelText(text);
                    }
                    break;
                case 2:
                    // You have won!
                    if (p2CharsDict.Count == 0)
                    {
                        uiController.ToggleVictoryPanel(true);
                        uiController.ToggleInfoPanel(false);
                        string text = $"Victory has been acheived for \nplayer 1 after defeating player 2 ";
                        uiController.SetVictoryPanelText(text);
                    }
                    break;
            }
        }

    }
}