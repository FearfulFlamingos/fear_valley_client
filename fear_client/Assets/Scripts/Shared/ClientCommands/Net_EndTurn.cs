[System.Serializable]
public class Net_EndTurn : NetMsg
{
    public Net_EndTurn()
    {
        OperationCode = NetOP.EndTurn;
    }

}