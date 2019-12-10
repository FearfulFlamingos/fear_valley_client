[System.Serializable]
public class Net_Propogate : NetMsg
{
    public Net_Propogate()
    {
        OperationCode = NetOP.PropogateTroop;
    }

    public string Prefab { set; get; }
    public int TroopID { set; get; }
    public int Health { set; get; }
    public int AttackMod { set; get; }
    public int MaxAttackVal { set; get; }
    public int DefenseMod { set; get; }
    public float AbsoluteXPos { set; get; }
    public float AbsoluteZPos { set; get; }
}