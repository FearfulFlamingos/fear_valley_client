using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_USER = 10;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    
    private byte reliableChannel;
    private int hostId;
    private int webHostId;
    private bool isStarted = false;

    #region Monobehavior
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    #endregion

    public void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        // Server only code
        hostId = NetworkTransport.AddHost(topo, PORT, null);
        webHostId = NetworkTransport.AddHost(topo, WEB_PORT, null);

        isStarted = true;
        Debug.Log($"Started server on port {PORT} and web port {WEB_PORT}");
    }                                         
    public void Shutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }
}
