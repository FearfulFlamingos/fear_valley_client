using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.CharacterClass;

namespace Scripts.Actions
{
    public class PlayerAttack : MonoBehaviour
    {
        public LayerMask whatCanBeClickedOn;
        private GameObject attackObject;
        private bool canAttack;
        private BattleUIControl uiController;
        private GameLoop gameLoop;
        public IInputManager InputManager { set; get; }

        // Start is called before the first frame update
        void Start()
        {
            uiController = GameObject.FindGameObjectWithTag("scripts").GetComponent<BattleUIControl>();
            gameLoop = GameObject.FindGameObjectWithTag("scripts").GetComponent<GameLoop>();
            if (InputManager == null)
                InputManager = GameObject.Find("SceneController").GetComponent<InputManager>();
        }


        // Update is called once per frame
        /// <summary>
        /// This function is called once per frame while attack is active. It is similar to player spotlight
        /// in that it is constantly checking to see if someone has been clicked on and updating the UI.
        /// </summary>
        void Update()
        {
            if (InputManager.GetAttackButtonDown() && Client.Instance.HasControl())
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask: 1 << 11))
                {

                    if (hit.transform != null)
                    {
                        attackObject = hit.transform.gameObject;

                        CharacterFeatures referenceScript = attackObject.GetComponent<CharacterFeatures>();

                        if (referenceScript.Team != gameObject.GetComponent<CharacterFeatures>().Team)
                        {
                            if (Vector3.Distance(gameObject.transform.position, attackObject.transform.position) < gameObject.GetComponent<CharacterFeatures>().AttackRange)
                            {
                                string text = $"Name: Roman\nHealth: {referenceScript.Health}\nClass: {referenceScript.Charclass}\nDefense: {referenceScript.DamageBonus}\nWithin Range: Yes";
                                uiController.SetAttackPanelEnemyInfo(text);
                                canAttack = true;
                            }
                            else
                            {
                                string text = $"Name: Roman\nHealth: {referenceScript.Health}\nClass: {referenceScript.Charclass}\nDefense: {referenceScript.DamageBonus}\nWithin Range: No";
                                uiController.SetAttackPanelEnemyInfo(text);
                                canAttack = false;
                            }

                        }
                        else
                        {
                            string text = $"You can not attack\nyour own team.";
                            uiController.SetAttackPanelEnemyInfo(text);
                        }

                    }
                }
            }

        }

        /// <summary>
        /// This is the base funciton for all attacks. The trigger for this function to be used is a button on the UI canvas.
        /// Once the button is triggered the player must have already selected a figure to attack or it will get an error.
        /// <para><paramref name="fudgeHit"/> is used for testing-- add or subtract a large number from the roll to ensure it's success or failure.</para>
        /// <para><paramref name="fudgeDamage"/> is like fudgeHit, but for the damage "roll".</para>
        /// </summary>
        /// <remarks>
        ///  This function will take away any health if need be or it can even trigger the destruction of an object. 
        /// </remarks>
        /// <param name="fudgeHit">Amount to add to the die rolls.</param>
        public void Attack()
        {
            ICharacterFeatures defendingCharacter = attackObject.GetComponent<Character>().Features;
            defendingCharacter.IsAttacking = true;
            ICharacterFeatures attackingCharacter = gameObject.GetComponent<Character>().Features;
            if (canAttack)
            {

                if (attackingCharacter.GetAttackRoll() >= defendingCharacter.ArmorBonus)
                {
                    int damageTaken = attackingCharacter.GetDamageRoll();
                    defendingCharacter.DamageCharacter(damageTaken);
                    if (defendingCharacter.Health == 0)
                    {
                        string text = $"You have dealt fatal damage\nto the player named Roman ";
                        uiController.SetAttackPanelEnemyInfo(text);
                        //timeToDistroy = true; // This actually destroys the attacked object


                        gameLoop.PlayerRemoval(attackObject.GetComponent<CharacterFeatures>().TroopId, 2);
                        Destroy(attackObject);
                        Debug.Log("Deleting slain enemy");
                        Client.Instance.SendRetreatData(defendingCharacter.TroopId, 1);
                    }
                    else
                    {
                        defendingCharacter.Health = System.Convert.ToInt32(defendingCharacter.Health - damageTaken);
                        string text = $"You attack was a success \nand you have dealt {damageTaken} damage\nto the player named Roman ";
                        uiController.SetAttackPanelEnemyInfo(text);
                        Client.Instance.SendAttackData(defendingCharacter.TroopId, damageTaken);
                    }

                }
                else
                {
                    string text = $"You could not get passed their armor\nyour attack has failed";
                    uiController.SetAttackPanelEnemyInfo(text);
                }
                CancelOrEndAttack();
                gameLoop.EndAttack();
            }
            else
            {
                string text = $"You can not attack this target\nthey are not in range. Select \nanother fighter to attack.";
                uiController.SetAttackPanelEnemyInfo(text);
            }
        }

        /// <summary>
        /// This function is used after an attack and deactivates this script.
        /// The function will also destroy any object that has been destroyed.
        /// </summary>
        public void CancelOrEndAttack()
        {
            CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
            referenceScript.IsAttacking = false;
            string text = $"Name: Health: \nClass: \nDefense: \nWithin Range: ";
            uiController.SetAttackPanelEnemyInfo(text);
        }
    }

}