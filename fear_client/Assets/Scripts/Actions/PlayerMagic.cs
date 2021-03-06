﻿using System.Collections.Generic;
using UnityEngine;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.CharacterClass;

namespace Scripts.Actions
{
    /// <summary>
    /// Controls magic for characters, including placement.
    /// </summary>
    public class PlayerMagic : MonoBehaviour
    {

        private bool placingExplosion = false;
        private Vector3 startingPosition;
        public GameObject selection;
        public ParticleSystem explosionEffect;
        
        /// <inheritdoc cref="IPlayerMovement.InputManager"/>
        public IInputManager InputManager { set; get; }

        #region Monobehaviour
        private void Start()
        {
            if (InputManager == null)
            {
                InputManager = GameObject.FindGameObjectWithTag("scripts").GetComponent<InputManager>();
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (placingExplosion)
            {    
                MoveExposionMarker();
                CheckPlayerInput();
            }
        }
        #endregion

        // Wrapper function that checks if LMB or RMB have been clicked. Will perform different actions based on input.
        private void CheckPlayerInput()
        {
            if (InputManager.GetLeftMouseClick())
            {
                CreateExplosion();
            }

            else if (InputManager.GetRightMouseClick())
            {
                CancelExplosion();
            }
        }

        // Moves the explosion marker around the placeable area.
        private void MoveExposionMarker()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition());
            // Only checks on ground layer.
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, 1 << 9) && Vector3.Distance(hit.point, startingPosition) < 5) 
            {
                selection.transform.position = hit.point;
            }
        }

        // Activates the explosion particle system and cleans up the gameboard. 
        private void CreateExplosion()
        {
            placingExplosion = false;
            ParticleSystem ex = Instantiate(explosionEffect, selection.transform.position, Quaternion.identity);
            ex.Play();

            HashSet<GameObject> enemies = selection.GetComponent<BlowUpEnemies>().EnemiesInBlast;
            foreach (GameObject enemy in enemies)
            {
                MagicAttack(enemy);
            }

            Destroy(selection);

            PlayerSpotlight.Instance.DeactivateCurrentFocus();
            
            GameLoop.MagicPoints--;
            GameLoop.ActionPoints -= 3;
        }

        // Cancels the explosion, retaining magic and action points.
        private void CancelExplosion()
        {
            placingExplosion = false;
            Destroy(selection);
            BattleUIControl.Instance.CancelMagicExplosion();
        }

        /// <summary>
        /// Creates the placeable explosion marker and starts the Update() loop running.
        /// </summary>
        public void StartExplosionSelector()
        {
            selection = Instantiate(Resources.Load("MagicSpell/MagicAttackArea")) as GameObject;
            startingPosition = transform.position;
            selection.transform.position = startingPosition;
            placingExplosion = true;
        }

        /// <summary>
        /// Checks attack vs armor and deals damage, if needed.
        /// </summary>
        /// <param name="enemy">The enemy character caught in the blast.</param>
        public void MagicAttack(GameObject enemy)
        {
            ICharacterFeatures defendingCharacter = enemy.GetComponent<Character>().Features;
            ICharacterFeatures attackingCharacter = gameObject.GetComponent<Character>().Features;

            if (attackingCharacter.GetMagicAttackRoll() >= defendingCharacter.ArmorBonus)
            {
                int damage = attackingCharacter.GetMagicDamageRoll();
                defendingCharacter.DamageCharacter(damage);
                if (defendingCharacter.Health == 0)
                {
                    Debug.Log("Killed enemy");
                    GameLoop.Instance.CharacterRemoval(defendingCharacter.TroopId, 2);
                    MonoClient.Instance.SendRetreatData(defendingCharacter.TroopId, 2, true);
                }
                else
                {
                    MonoClient.Instance.SendAttackData(defendingCharacter.TroopId, damage);
                    Debug.Log($"Dealt {damage} damage");
                }
            }
            else
            {
                Debug.Log("Missed attack");
            }

        }

    }
}