using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.CharacterClass;

namespace Scripts.Actions
{
    public class PlayerMagic : MonoBehaviour
    {

        private bool placingExplosion = false;
        private Vector3 startingPosition;
        public GameObject selection;

        public ParticleSystem explosionEffect;
        public IInputManager InputManager { set; get; }

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

        private void MoveExposionMarker()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition());
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, 1 << 9) && Vector3.Distance(hit.point, startingPosition) < 5) //Only look on ground layer
            {
                selection.transform.position = hit.point;
            }
        }

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

        private void CancelExplosion()
        {
            placingExplosion = false;
            Destroy(selection);
            BattleUIControl.Instance.CancelMagicExplosion();
        }

        public void PlaceExplosion()
        {
            selection = Instantiate(Resources.Load("MagicAttackAreaTemp")) as GameObject;
            startingPosition = transform.position;
            selection.transform.position = startingPosition;
            placingExplosion = true;
        }

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
                    //attackChar.text = $"You have dealt fatal damage\nto the player named Roman ";
                    Debug.Log("Killed enemy");
                    GameLoop.Instance.CharacterRemoval(defendingCharacter.TroopId, 2);
                    MonoClient.Instance.SendRetreatData(defendingCharacter.TroopId, 2, true);
                }
                else
                {
                    MonoClient.Instance.SendAttackData(defendingCharacter.TroopId, damage);
                    //attackChar.text = $"You attack was a success \nand you have dealt {damageTaken} damage\nto the player named Roman ";
                    Debug.Log($"Dealt {damage} damage");
                }

            }
            else
            {
                //attackChar.text = $"You could not get passed their armor\nyour attack has failed";
                Debug.Log("Missed attack");
            }

        }

    }
}