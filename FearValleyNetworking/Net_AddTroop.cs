namespace FearValleyNetwork
{
    [System.Serializable]
    public class Net_AddTroop : NetMsg
    {
        public Net_AddTroop()
        {
            OperationCode = NetOP.AddTroop;
        }

        public string TroopType { set; get; }
        public string WeaponType { set; get; }
        public string ArmorType { set; get; }
        public int XPosRelative { set; get; }
        public int ZPosRelative { set; get; }
    }
}
