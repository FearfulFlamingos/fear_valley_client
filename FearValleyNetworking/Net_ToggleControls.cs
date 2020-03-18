namespace FearValleyNetwork
{
    /// <summary>
    /// <inheritdoc/>Toggle the clients controls. No information is passed along.
    /// </summary>
    [System.Serializable]
    public class Net_ToggleControls : NetMsg
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        public Net_ToggleControls()
        {
            OperationCode = (byte) NetOP.Operation.ToggleControls;
        }

    }
}