using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Character;

namespace Scripts.Actions
{
    public class BlowUpEnemies : MonoBehaviour
    {
        public HashSet<GameObject> EnemiesInBlast { private set;  get; }
        public int countEnemiesInBlast = 0;

        private void Awake()
        {
            EnemiesInBlast = new HashSet<GameObject>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 10 && other.gameObject.GetComponent<CharacterFeatures>().Team == 2)
            {
                Debug.Log("Object entered");
                Debug.Log($"{other.gameObject.layer}");
                EnemiesInBlast.Add(other.gameObject);
                countEnemiesInBlast++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 10 && other.gameObject.GetComponent<CharacterFeatures>().Team == 2)
            {
                Debug.Log("Object exited");
                EnemiesInBlast.Remove(other.gameObject);
                countEnemiesInBlast--;
            }
        }
    }
}