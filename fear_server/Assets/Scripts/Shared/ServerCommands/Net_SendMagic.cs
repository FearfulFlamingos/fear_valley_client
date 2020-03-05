[System.Serializable]
public class Net_SendMagic: NetMsg
{

    public Net_SendMagic()
    {
        OperationCode = NetOP.SendMagic;
    }

    public int MagicAmount { set; get; }
}