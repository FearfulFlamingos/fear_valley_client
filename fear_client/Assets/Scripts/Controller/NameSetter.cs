using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controller
{
    /// <summary>
    /// Attaches to a non-destructable object and stores the name preference of the player.
    /// </summary>
    public class NameSetter : MonoBehaviour
    {
        /// <summary>Name to store.</summary>
        public static string SelectedName { set; get; }

        // Monobehaviour start.
        private void Start() { SelectedName = "Anonymous"; DontDestroyOnLoad(gameObject); }
    }
}

