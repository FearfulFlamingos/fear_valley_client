using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Actions
{
    /// <summary>
    /// Helper script placed on the explosion selector.
    /// </summary>
    public class BlowUpEnemies : MonoBehaviour
    {
        /// <summary>Hashet that keeps track of all enemies currently within the explosion selectors bounds.</summary>
        public HashSet<GameObject> EnemiesInBlast { private set;  get; }

        #region Monobehaviour
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
        #endregion
    }
}