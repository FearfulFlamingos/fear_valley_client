using UnityEngine;

namespace Scripts.Networking
{
    /// <summary>
    /// Wrapper class for the Client implementation. See <see cref="Client"/> for actual implementation.
    /// </summary>
    public class MonoClient : MonoBehaviour
    {
        /// <summary>IClient instance. Can be replaced for testing.</summary>
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