[System.Serializable]
public class Net_RETREAT : NetMsg
{
    public Net_RETREAT()
    {
        OperationCode = NetOP.RETREAT;
    }

    public int TroopID { set; get; }
}