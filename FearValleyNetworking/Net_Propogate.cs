namespace FearValleyNetwork
{
    /// <summary>
    /// <inheritdoc/>Send troop data to the client.
    /// See <see cref="Net_SendMagic"/> to send magic.
    /// </summary>
    [System.Serializable]
    public class Net_Propogate : NetMsg
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        public Net_Propogate()
        {
            OperationCode = (byte) NetOP.Operation.PropogateTroop;
        }
        /// <summary>Troop prefab to spawn.</summary>
        public string Prefab { set; get; }
        /// <summary>Troop ID number.</summary>
        public int TroopID { set; get; }
        /// <summary>Client connection number.</summary>
        /// <remarks>This will either be 1 or 255, since all clients think they are connection #1.</remarks>
        public int ComingFrom { set; get; }
        /// <summary>Troop team number.</summary>
        public int TeamNum { set; get; }
        /// <summary>Amount of health.</summary>
        public int Health { set; get; }
        /// <summary>Distance the troop can move on a turn.</summary>
        public int Movement {set; get; }
        /// <summary>Bonus to attack rolls.</summary>
        public int AtkBonus { set; get; }
        /// <summary>Range of attacks.</summary>
        public int AtkRange { set; get; }
        /// <summary>Size of the damage die.</summary>
        public int MaxAttackVal { set; get; }
        /// <summary>Armor value of the troop.</summary>
        public int DefenseMod { set; get; }
        /// <summary>True X position on the board.</summary>
        public double AbsoluteXPos { set; get; }
        /// <summary>True Z position on the board.</summary>
        public double AbsoluteZPos { set; get; }
    }
}