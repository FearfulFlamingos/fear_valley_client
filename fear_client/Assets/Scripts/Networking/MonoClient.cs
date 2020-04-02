using UnityEngine;
using System.Collections;

namespace Scripts.Networking
{
    public class MonoClient : MonoBehaviour
    {
        public static IClient Instance { set; get; }
        
        void Start()
        {
            if (Instance == null)
                Instance = new Client();
            DontDestroyOnLoad(gameObject);
            Instance.Init();
        }

        void Update()
        {
            Instance.UpdateMessagePump();
        }
    }
}