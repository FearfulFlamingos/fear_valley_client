namespace FearValleyNetwork
{
    /// <summary>
    /// <inheritdoc/>Tell the server the client's turn is over. No information is passed along.
    /// </summary>
    [System.Serializable]
    public class Net_EndTurn : NetMsg
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        public Net_EndTurn()
        {
            OperationCode = (byte) NetOP.Operation.EndTurn;
        }

    }
}