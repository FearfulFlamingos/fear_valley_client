[System.Serializable]
public class Net_FinishBuild : NetMsg
{
    public Net_FinishBuild()
    {
        OperationCode = NetOP.FinishBuild;
    }

}