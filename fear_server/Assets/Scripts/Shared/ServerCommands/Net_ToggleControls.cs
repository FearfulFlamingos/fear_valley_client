[System.Serializable]
public class Net_ToggleControls : NetMsg
{
    public Net_ToggleControls()
    {
        OperationCode = NetOP.ToggleControls;
    }

}