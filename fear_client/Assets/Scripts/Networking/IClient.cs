using FearValleyNetwork;
using UnityEngine.Networking;

namespace Scripts.Networking
{
    public interface IClient
    {
        int ConnectionId { get; set; }
        int HostId { get; set; }
        bool IsStarted { get; set; }
        int MAX_USER { get; set; }
        int PORT { get; set; }
        byte ReliableChannel { get; set; }
        string SERVER_IP { get; set; }
        NetworkEventType LastEvent { get; set; }
        NetMsg LastSent { get; set; }

        void CheckMessageType(int recHostId, int connectionId, int channelId, byte[] recievedBuffer, NetworkEventType type);
        bool HasControl();
        void Init();
        void SendAttackData(int troopId, int health);
        void SendEndTurn();
        void SendFinishBuild(int magicAmount);
        void SendMoveData(int TroopID, float newX, float newZ);
        void SendRetreatData(int troopId, int TeamNum);
        void SendToServer(NetMsg msg);
        void SendTroopRequest(string troop, string weapon, string armor, float xPos, float yPos);
        void Shutdown();
        void UpdateMessagePump();
    }
}