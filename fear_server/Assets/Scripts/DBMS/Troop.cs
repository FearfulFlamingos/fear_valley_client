namespace Scripts.DBMS
{
    public class Troop
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
        private float xPos;
        private float zPos;

        public Troop(int troopID, int teamNum, string troopType, int armor, int weapMod, int weapRange, int weapDmg, int health, int movement, bool leader, float xPos, float zPos)
        {
            TroopID = troopID;
            TeamNum = teamNum;
            TroopType = troopType;
            Armor = armor;
            TroopAtkBonus = weapMod;
            WeaponRange = weapRange;
            WeaponDamage = weapDmg;
            Health = health;
            Movement = movement;
            Leader = leader;
            XPos = xPos;
            ZPos = zPos;
        }

        public string PRINT()
        {
            return $"\n<Troop>\nTeam={TeamNum}\nClass={TroopType}" +
                $"\nArmor={Armor}\nWMod={TroopAtkBonus}" +
                $"\nWDmg={WeaponDamage}\nHealth={Health}" +
                $"\nleader={Leader}\nXPos={XPos}\nZPos={ZPos}";
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
        public float XPos { get => xPos; set => xPos = value; }
        public float ZPos { get => zPos; set => zPos = value; }
        public int TroopID { get => troopID; set => troopID = value; }
        #endregion
    }
}