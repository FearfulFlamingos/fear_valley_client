namespace FearValleyNetwork
{
    /// <summary>
    /// <inheritdoc/>Add a troop to the server's database.
    /// See <see cref="Net_FinishBuild"/> to finish build data.
    /// </summary>
    [System.Serializable]
    public class Net_AddTroop : NetMsg
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        public Net_AddTroop()
        {
            OperationCode = (byte) NetOP.Operation.AddTroop;
        }
        /// <summary>Type of troop.</summary>
        public string TroopType { set; get; }
        /// <summary>Weapon carried by troop.</summary>
        public string WeaponType { set; get; }
        /// <summary>Armor carried by troop.</summary>
        public string ArmorType { set; get; }
        /// <summary>X position where they were placed, relative to thier perspective.</summary>
        public float XPosRelative { set; get; }
        /// <summary>Z position where they were placed, relative to their perspective.</summary>
        public float ZPosRelative { set; get; }
    }
}
