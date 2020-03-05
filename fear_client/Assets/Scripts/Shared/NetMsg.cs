public static class NetOP
{
    public const int None = 0;
    // Client Commands are cmds sent FROM client
    public const int AddTroop = 1;
    public const int FinishBuild = 3;
    public const int MOVE = 20;
    public const int ATTACK = 30;
    public const int RETREAT = 40;
    public const int EndTurn = 50;

    // Server Commands are cmds sent FROM server
    public const int ChangeScene = 70;
    public const int PropogateTroop = 4;
    public const int ToggleControls = 60;
    public const int SendMagic = 5;
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
