[System.Serializable]
public class Net_ChangeScene : NetMsg
{
    public Net_ChangeScene()
    {
        OperationCode = NetOP.ChangeScene;
    }

    public string SceneName { set; get; }
}