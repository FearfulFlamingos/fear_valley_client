public static class NetOP
{
    public const int None = 0;
    public const int AddTroop = 1;
    public const int CreateAccount = 100;
}

[System.Serializable]
public abstract class NetMsg
{
    public byte OperationCode { set; get; }

    public NetMsg()
    {
        OperationCode = NetOP.None;
    }
}
