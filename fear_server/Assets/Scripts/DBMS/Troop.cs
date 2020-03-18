﻿public class Troop
{
    private int troopID;
    private int teamNum;
    private string troopType;
    private int armor;
    private int troopAtkBonus;
    private int weaponRange;
    private int weaponDamage;
    private int health;
    private int movement;
    private bool leader;
    private double xPos;
    private double zPos;

    public Troop(int troopID, int teamNum, string troopType, int armor, int weapMod, int weapRange, int weapDmg, int health, int movement, bool leader, double xPos, double zPos)
    {
        this.TroopID = troopID;
        this.TeamNum = teamNum;
        this.TroopType = troopType;
        this.Armor = armor;
        this.TroopAtkBonus = weapMod;
        this.WeaponRange = weapRange;
        this.WeaponDamage = weapDmg;
        this.Health = health;
        this.Movement = movement;
        this.Leader = leader;
        this.XPos = xPos;
        this.ZPos = zPos;
    }

    public string PRINT()
    {
        return $"\n<Troop>\nTeam={this.TeamNum}\nClass={this.TroopType}" +
            $"\nArmor={this.Armor}\nWMod={this.TroopAtkBonus}" +
            $"\nWDmg={this.WeaponDamage}\nHealth={this.Health}" +
            $"\nleader={this.Leader}\nXPos={this.XPos}\nZPos={this.ZPos}";
    }


    #region Getters and Setters
    // call with:
    //    Troop instance = new Troop(...);
    //    int team = instance.TeamNum;
    //    instance.TeamNum = 8;
    public int TeamNum { get => teamNum; set => teamNum = value; }
    public string TroopType { get => troopType; set => troopType = value; }
    public int Armor { get => armor; set => armor = value; }
    public int TroopAtkBonus { get => troopAtkBonus; set => troopAtkBonus = value; }
    public int WeaponRange { get => weaponRange; set => weaponRange = value; }
    public int WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
    public int Health { get => health; set => health = value; }
    public int Movement { get => movement; set => movement = value; }
    public bool Leader { get => leader; set => leader = value; }
    public double XPos { get => xPos; set => xPos = value; }
    public double ZPos { get => zPos; set => zPos = value; }
    public int TroopID { get => troopID; set => troopID = value; }
    #endregion
}
