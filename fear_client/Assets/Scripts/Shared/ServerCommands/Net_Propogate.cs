[System.Serializable]
public class Net_Propogate : NetMsg
{
    public Net_Propogate()
    {
        OperationCode = NetOP.PropogateTroop;
    }

    public string Prefab { set; get; }
    public int TroopID { set; get; }
    public int ComingFrom { set; get; }
    public int TeamNum { set; get; }
    public int Health { set; get; }
    public int AtkBonus { set; get; }
    public int AtkRange { set; get; }
    public int MaxAttackVal { set; get; }
    public int DefenseMod { set; get; }
    public double AbsoluteXPos { set; get; }
    public double AbsoluteZPos { set; get; }
}