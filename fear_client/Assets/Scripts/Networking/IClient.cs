using FearValleyNetwork;

namespace Scripts.Networking
{
    public interface IClient
    {
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