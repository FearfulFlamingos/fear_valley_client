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
        public GameObject attackObject;
        public bool CanAttack { set; get; } 
        public IInputManager InputManager { set; get; }
        private bool attacking;


        void Start()
        {
            if (InputManager == null)
                InputManager = GameObject.Find("SceneController").GetComponent<InputManager>();
        }


        /// <summary>
        /// This function is called once per frame while attack is active. It is similar to player spotlight
        /// in that it is constantly checking to see if someone has been clicked on and updating the UI.
        /// </summary>
        void Update()
        {
            if (Attacking() && InputManager.GetLeftMouseClick())
            {
                CheckIfEnemyCanBeAttacked();
            }
        }

        // 
        private void CheckIfEnemyCanBeAttacked()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition());
            // Layer 11 has enemies
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, layerMask: 1 << 11))
            {
                attackObject = hit.transform.gameObject;
                Debug.Log(attackObject);

                ICharacterFeatures referenceScript = attackObject.GetComponent<Character>().Features;

                if (Vector3.Distance(gameObject.transform.position, attackObject.transform.position) < gameObject.GetComponent<Character>().Features.AttackRange)
                {
                    string text = $"Name: Roman\nHealth: {referenceScript.Health}\nClass: {referenceScript.Charclass}\nDefense: {referenceScript.DamageBonus}\nWithin Range: Yes";
                    BattleUIControl.Instance.SetAttackPanelEnemyInfo(text);
                    CanAttack = true;
                }
                else
                {
                    string text = $"Name: Roman\nHealth: {referenceScript.Health}\nClass: {referenceScript.Charclass}\nDefense: {referenceScript.DamageBonus}\nWithin Range: No";
                    BattleUIControl.Instance.SetAttackPanelEnemyInfo(text);
                    CanAttack = false;
                }
            }
        }

        /// <summary>
        /// The big if() block in the update loop hinges on this. The only way it's going to be called is if 
        /// MonoClient.Instance has control, so we don't necessarily need to check that explicitly. 
        /// </summary>
        public bool Attacking() => attacking && MonoClient.Instance.HasControl();
        
        /// <summary>
        /// Allows the PlayerAttack Update loop to start running.
        /// </summary>
        public void ActivateAttack()
        {
            attacking = true;
        }

        /// <summary>
        /// This is the base funciton for all attacks. The trigger for this function to be used is a button on the UI canvas.
        /// Once the button is triggered the player must have already selected a figure to attack or it will get an error.
        /// </summary>
        /// <remarks>
        ///  This function will take away any health if need be or it can even trigger the destruction of an object. 
        /// </remarks>
        public void Attack()
        {
            ICharacterFeatures defendingCharacter = attackObject.GetComponent<Character>().Features;
            ICharacterFeatures attackingCharacter = gameObject.GetComponent<Character>().Features;
            Debug.Log(attackObject);
            if (CanAttack)
            {
                Debug.Log($"Can Attack {attackObject}");
                if (attackingCharacter.GetAttackRoll() >= defendingCharacter.ArmorBonus)
                {
                    Debug.Log($"Successfully attacked {attackObject}");
                    int damageTaken = attackingCharacter.GetDamageRoll();
                    defendingCharacter.DamageCharacter(damageTaken);
                    if (defendingCharacter.Health == 0)
                    {
                        Debug.Log($"Killed {attackObject}");
                        string text = $"You have dealt fatal damage\nto the player named Roman ";
                        BattleUIControl.Instance.SetAttackPanelEnemyInfo(text);
                        GameLoop.Instance.PlayerRemoval(defendingCharacter.TroopId, 2);
                        MonoClient.Instance.SendRetreatData(defendingCharacter.TroopId, 2);
                    }
                    else
                    {
                        string text = $"You attack was a success \nand you have dealt {damageTaken} damage\nto the player named Roman ";
                        BattleUIControl.Instance.SetAttackPanelEnemyInfo(text);
                        MonoClient.Instance.SendAttackData(defendingCharacter.TroopId, damageTaken);
                    }

                }
                else
                {
                    string text = $"You could not get passed their armor\nyour attack has failed";
                    BattleUIControl.Instance.SetAttackPanelEnemyInfo(text);
                }
                CancelOrEndAttack();
                GameLoop.Instance.EndAttack();
            }
            else
            {
                string text = $"You can not attack this target\nthey are not in range. Select \nanother fighter to attack.";
                BattleUIControl.Instance.SetAttackPanelEnemyInfo(text);
            }
        }

        /// <summary>
        /// This function is used after an attack and deactivates this script.
        /// The function will also destroy any object that has been destroyed.
        /// </summary>
        public void CancelOrEndAttack()
        {
            ICharacterFeatures referenceScript = gameObject.GetComponent<Character>().Features;
            referenceScript.IsAttacking = false;
            string text = $"Name: Health: \nClass: \nDefense: \nWithin Range: ";
            BattleUIControl.Instance.SetAttackPanelEnemyInfo(text);
        }
    }

}