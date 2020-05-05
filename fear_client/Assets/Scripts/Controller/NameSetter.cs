using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controller
{
    public class NameSetter : MonoBehaviour
    {
        public static string SelectedName { set; get; }
        private void Start() { SelectedName = "Anonymous"; DontDestroyOnLoad(gameObject); }
    }
}

