namespace FearValleyNetwork
{
    /// <summary>
    /// Defines the network operation codes used by all network commands.
    /// </summary>
    /// <remarks>
    /// The Operation codes are roughly in the order that a typical game will play out.
    /// <para>Codes 1-10 are used to initialize the game pieces.</para>
    /// <para>Codes 20-50 are reserved for player actions.</para>
    /// <para>Codes 6X are reserved for manipulating controls and UI.</para>
    /// <para>Codes 7X are reserved for scene transitions.</para>
    /// </remarks>
    public static class NetOP
    {
        /// <summary>
        /// All possible network commands.
        /// </summary>
        public enum Operation {
            /// <summary>Default command.</summary>
            None = 0,
            /// <summary>Client command to add a troop to server DB.</summary>
            AddTroop = 1,
            /// <summary>Client command to finish building an army.</summary>
            FinishBuild = 3,
            /// <summary>Client command to move a troop.</summary>
            MOVE = 20,
            /// <summary>Client command to attack another troop.</summary>
            ATTACK = 30,
            /// <summary>Client command to retreat a toop.</summary>
            RETREAT = 40,
            /// <summary>Client command to end a turn.</summary>
            EndTurn = 50,
            /// <summary>Server command to change client scene.</summary>
            ChangeScene = 70,
            /// <summary>Server command to send troop data.</summary>
            PropogateTroop = 4,
            /// <summary>Server command to send amount of spells.</summary>
            SendMagic = 5,
            /// <summary>Server command to allow/disallow all controls.</summary>
            ToggleControls = 60
        }
    }

    /// <summary>
    /// Network message class.
    /// </summary>
    [System.Serializable]
    public abstract class NetMsg
    {
        /// <summary>
        /// Inherited method to determine the type of message.
        /// </summary>
        public byte OperationCode { set; get; }

        /// <summary>
        /// Constructor method.
        /// </summary>
        public NetMsg()
        {
            OperationCode = (byte) NetOP.Operation.None;
        }
    }
}
