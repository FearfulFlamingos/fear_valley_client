using FearValleyNetwork;
using UnityEngine.Networking;

/// <summary>
/// All networking related scripts are here.
/// </summary>
namespace Scripts.Networking
{
    /// <summary>
    /// Attaches to a client object and handles all communication with the server.
    /// </summary>
    /// <remarks>
    /// There are a lot of precompiler options being used. This is to disable the warnings about 
    /// how Unity's netcode is being deprecated. Since the replacement has yet to come out and the 
    /// code is still valid in this version of unity, it's cleaner to disable the warnings.
    /// <para>
    /// The Interface is used with NSubstitute for testing network commands 
    /// in other scripts.
    /// </para>
    /// </remarks>
    public interface IClient
    {
        /// <summary>Maximum number of users on the server.</summary>
        int MAX_USER { get; set; }

        /// <summary>IP address of the server.</summary>
        string SERVER_IP { get; set; }

        /// <summary>Reliable channel #</summary>
        byte ReliableChannel { get; set; }

        /// <summary>Connection # from the server.</summary>
        int ConnectionId { get; set; }

        /// <summary>Port # and Host Topology combined.</summary>
        int HostId { get; set; }

        /// <summary>Whether or not the client connection has been started.</summary>
        bool IsStarted { get; set; }

        /// <summary>Used for Testing.</summary>
        NetworkEventType LastEvent { get; set; }

        /// <summary>Used for Testing.</summary>
        NetMsg LastSent { get; set; }

        /// <summary>
        /// Checks the network event type of the incoming net message and acts appropriately.
        /// </summary>
        /// <remarks>
        /// Public due to testing, could easily have been a part of 
        /// <see cref="UpdateMessagePump"/>
        /// </remarks>
        /// <param name="recHostId">Which server is it coming from</param>
        /// <param name="connectionId">Which connection is being used.</param>
        /// <param name="channelId">Which channel is being used.</param>
        /// <param name="recievedBuffer">Data sent with message (if applicable).</param>
        /// <param name="type">Type of network event.</param>
        void CheckMessageType(int recHostId, int connectionId, int channelId, byte[] recievedBuffer, NetworkEventType type);
        
        /// <summary>
        /// Determines whether or not the player can act.
        /// </summary>
        /// <returns>State of characters turn.</returns>
        bool HasControl();
        
        /// <summary>
        /// Initializes the client and connects to the server.
        /// </summary>
        void Init();


        /// <summary>
        /// Sends an attack to the server, updating the characters health.
        /// </summary>
        /// <param name="troopId">Troop that took damage.</param>
        /// <param name="health">New health of troop.</param>
        void SendAttackData(int troopId, int health);

        /// <summary>
        /// Ends the players turn.
        /// </summary>
        void SendEndTurn();

        /// <summary>
        /// Tell the server that the player is finished building an army.
        /// <para>Updates the number of spells the player bought in the database.</para>
        /// <para>Pulls the player name set in Options, if it is set.</para>
        /// </summary>
        /// <param name="magicAmount">Number of spells the player bought.</param>
        void SendFinishBuild(int magicAmount);

        /// <summary>
        /// Tells the server to move the character to a new position.
        /// </summary>
        /// <param name="TroopID">Troop to be moved.</param>
        /// <param name="newX">New X position, relative to the player.</param>
        /// <param name="newZ">New Z position, relative to the player.</param>
        void SendMoveData(int TroopID, float newX, float newZ);

        /// <summary>
        /// Lets the server know a character has retreated or been slain.
        /// </summary>
        /// <param name="troopId">Specific troop.</param>
        /// <param name="TeamNum">Team number of troop.</param>
        /// <param name="characterShouldDie">Whether the other troop should be counted as dead.</param>
        void SendRetreatData(int troopId, int TeamNum, bool characterShouldDie);

        /// <summary>
        /// Base function to send a command to the server. 
        /// </summary>
        /// <param name="msg">Any net command.</param>
        void SendToServer(NetMsg msg);

        /// <summary>
        /// Sends a troop with information to the server's database.
        /// </summary>
        /// <param name="troop">Type of troop.</param>
        /// <param name="weapon">Weapon used by troop.</param>
        /// <param name="armor">Armor used by troop.</param>
        /// <param name="xPos">X position of troop, relative to the player.</param>
        /// <param name="yPos">Z position of the troop, relative to the player.</param>
        void SendTroopRequest(string troop, string weapon, string armor, float xPos, float yPos);
        
        /// <summary>
        /// Pulls the stored name from PlayerPrefs and updates the server.
        /// </summary>
        void SendUpdatedName();
        
        /// <summary>
        /// Shuts down the server.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Checks every frame for a new network event.
        /// </summary>
        void UpdateMessagePump();
    }
}