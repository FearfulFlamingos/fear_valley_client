namespace FearValleyNetwork
{
    /// <summary>
    /// <inheritdoc/>Tell the server to move a troop to a new location.
    /// See <see cref="Net_ATTACK"/> to send attack data.
    /// <seealso cref="Net_RETREAT"/>
    /// </summary>
    [System.Serializable]
    public class Net_MOVE : NetMsg
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        public Net_MOVE()
        {
            OperationCode = (byte) NetOP.Operation.MOVE;
        }
        /// <summary>Specific troop to move.</summary>
        public int TroopID { set; get; }
        /// <summary>New X position, relative to client.</summary>
        public float NewX { set; get; }
        /// <summary>New Z position, relative to client.</summary>
        public float NewZ { set; get; }

    }
}