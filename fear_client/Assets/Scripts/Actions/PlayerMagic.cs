using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.Character;

namespace Scripts.Actions
{
    public class PlayerMagic : MonoBehaviour
    {

        private bool placingExplosion = false;
        private Vector3 startingPosition;
        private GameObject selection;
        private GameObject scripts;

        public ParticleSystem explosionEffect;

        private void Start()
        {
            scripts = GameObject.FindGameObjectWithTag("scripts");
        }

        // Update is called once per frame
        private void Update()
        {
            if (placingExplosion)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, 1 << 9)) //Only look on ground layer
                {
                    if (Vector3.Distance(hit.point, startingPosition) < 5)
                    {
                        selection.transform.position = hit.point;
                    }

                }

                if (Input.GetMouseButtonDown(0))
                {
                    CreateExplosion();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    CancelExplosion();
                }
            }
        }

        private void CreateExplosion()
        {
            placingExplosion = false;
            ParticleSystem ex = Instantiate(explosionEffect, selection.transform.position, Quaternion.identity);
            ex.Play();

            HashSet<GameObject> enemies = selection.GetComponent<BlowUpEnemies>().GetEnemiesInBlast();
            foreach (GameObject enemy in enemies)
            {
                MagicAttack(enemy);
            }

            Destroy(selection);

        }

        private void CancelExplosion()
        {
            placingExplosion = false;
            Destroy(selection);
            scripts.GetComponent<GameLoop>().CancelSpell();
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
            System.Random random = new System.Random();
            CharacterFeatures referenceScript = enemy.GetComponent<Scripts.Character.Character>().Features;
            GameLoop gamevars = scripts.GetComponent<GameLoop>();

            if (random.Next(0, 20) + 5 >= referenceScript.ArmorBonus)
            {
                int damageTaken = random.Next(1, 12) + 2;
                if ((referenceScript.Health - damageTaken) <= 0)
                {
                    //attackChar.text = $"You have dealt fatal damage\nto the player named Roman ";
                    Debug.Log("Killed enemy");

                    gamevars.PlayerRemoval(enemy.GetComponent<CharacterFeatures>().TroopId, 2, false);
                    Client.Instance.SendRetreatData(referenceScript.TroopId, 1);
                }
                else
                {
                    referenceScript.Health = System.Convert.ToInt32(referenceScript.Health - damageTaken);
                    Client.Instance.SendAttackData(referenceScript.TroopId, damageTaken);
                    //attackChar.text = $"You attack was a success \nand you have dealt {damageTaken} damage\nto the player named Roman ";
                    Debug.Log($"Dealt {damageTaken} damage");
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