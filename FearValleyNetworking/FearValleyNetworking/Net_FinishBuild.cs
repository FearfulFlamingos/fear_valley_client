namespace FearValleyNetwork
{
    /// <summary>
    /// <inheritdoc/>Tell the server that the client is finished building an army.
    /// See <see cref="Net_AddTroop"/> to send troop data.
    /// </summary>
    [System.Serializable]
    public class Net_FinishBuild : NetMsg
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        public Net_FinishBuild()
        {
            OperationCode = (byte) NetOP.Operation.FinishBuild;
        }

        /// <summary>Number of spells the client bought.</summary>
        public int MagicBought { set; get; }
    }
}