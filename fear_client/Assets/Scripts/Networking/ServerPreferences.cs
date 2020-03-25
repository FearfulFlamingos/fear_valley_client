﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Scripts.Networking
{
    public class ServerPreferences : MonoBehaviour
    {
        [SerializeField]
        public TMP_InputField portNum;
        [SerializeField]
        public TMP_InputField serverNum;

        [SerializeField]
        private static string IP_ADDR;
        [SerializeField]
        private static int PORT_NUM;
        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Get the IP Address of the desired server.
        /// </summary>
        /// <returns>String IP Address.</returns>
        public string GetIP()
        {
            return IP_ADDR;
        }

        /// <summary>
        /// Get the port number of the desired server.
        /// </summary>
        /// <returns>Integer port number.</returns>
        public int GetPort()
        {
            return PORT_NUM;
        }

        /// <summary>
        /// Pull the connection values from the scene.
        /// </summary>
        public void SetValues()
        {
            IP_ADDR = serverNum.text;
            PORT_NUM = System.Int32.Parse(portNum.text);
            Debug.Log($"Read {IP_ADDR}:{PORT_NUM}");
            SceneManager.LoadScene(2);
        }

        /// <summary>
        /// Manually set the connection info.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void SetValues(string ip, int port)
        {
            IP_ADDR = ip;
            PORT_NUM = port;
        }

        /// <summary>
        /// Quickly connect to a localhost server.
        /// </summary>
        public void TestLocalhost()
        {
            IP_ADDR = "127.0.0.1";
            PORT_NUM = 50000;
            Debug.Log($"Quick Connect to Localhost");
            SceneManager.LoadScene(2);
        }
    }
}