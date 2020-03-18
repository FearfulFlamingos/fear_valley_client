namespace FearValleyNetwork
{
    [System.Serializable]
    public class Net_ATTACK : NetMsg
    {
        public Net_ATTACK()
        {
            OperationCode = NetOP.ATTACK;
        }

        public int TroopID { set; get; }
        public int NewHealth { set; get; }

    }
}