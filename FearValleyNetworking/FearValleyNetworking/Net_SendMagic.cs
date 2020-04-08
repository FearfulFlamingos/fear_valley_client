namespace FearValleyNetwork
{
    /// <summary>
    /// <inheritdoc/>Inform client how many spells they have.
    /// See: <see cref="Net_Propogate"/> to send troop data.
    /// </summary>
    [System.Serializable]
    public class Net_SendMagic : NetMsg
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        public Net_SendMagic()
        {
            OperationCode = (byte) NetOP.Operation.SendMagic;
        }
        /// <summary>The number of spells purchased.</summary>
        public int MagicAmount { set; get; }
    }
}