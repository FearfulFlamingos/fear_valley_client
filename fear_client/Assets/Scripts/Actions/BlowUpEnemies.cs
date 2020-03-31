using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.CharacterClass;

namespace Scripts.Actions
{
    public class BlowUpEnemies : MonoBehaviour
    {
        public HashSet<GameObject> EnemiesInBlast { private set;  get; }

        private void Awake()
        {
            EnemiesInBlast = new HashSet<GameObject>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 11)
            {
                EnemiesInBlast.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 11)
            {
                EnemiesInBlast.Remove(other.gameObject);
            }
        }
    }
}