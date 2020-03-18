namespace FearValleyNetwork
{
    /// <summary>
    /// <inheritdoc/>Tell the server which troop has retreated.
    /// See <see cref="Net_MOVE"/> for movement.
    /// <seealso cref="Net_ATTACK"/>
    /// </summary>
    [System.Serializable]
    public class Net_RETREAT : NetMsg
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        public Net_RETREAT()
        {
            OperationCode = (byte) NetOP.Operation.RETREAT;
        }
        /// <summary>Troop to remove.</summary>
        public int TroopID { set; get; }
        /// <summary>Team of the troop to remove.</summary>
        public int TeamNum { set; get; }
    }
}