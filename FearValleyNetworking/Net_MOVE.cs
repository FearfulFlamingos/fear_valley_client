namespace FearValleyNetwork
{
    [System.Serializable]
    public class Net_MOVE : NetMsg
    {
        public Net_MOVE()
        {
            OperationCode = NetOP.MOVE;
        }

        public int TroopID { set; get; }
        public float NewX { set; get; }
        public float NewZ { set; get; }

    }
}