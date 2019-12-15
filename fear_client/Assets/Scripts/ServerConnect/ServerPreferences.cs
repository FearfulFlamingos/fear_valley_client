using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    public string GetIP()
    {
        return IP_ADDR;
    }

    public int GetPort()
    {
        return PORT_NUM;
    }

    public void SetValues()
    {
        IP_ADDR = serverNum.text;
        PORT_NUM = System.Int32.Parse(portNum.text);
        Debug.Log($"Read {IP_ADDR}:{PORT_NUM}");
        SceneManager.LoadScene("ArmyBuild");
    }


}
