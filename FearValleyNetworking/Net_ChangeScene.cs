namespace FearValleyNetwork
{
    /// <summary>
    /// <inheritdoc/>Change the client's scene.
    /// </summary>
    [System.Serializable]
    public class Net_ChangeScene : NetMsg
    {
        /// <summary>
        /// Constructor method.
        /// </summary>
        public Net_ChangeScene()
        {
            OperationCode = (byte) NetOP.Operation.ChangeScene;
        }
        
        /// <summary>Name of the scene to change to.</summary>
        public string SceneName { set; get; }
    }
}