using UnityEngine;

namespace Scripts.Actions
{
    public interface IPlayerMovement
    {
        void ActivateMovement();
        void Move(Vector3 newPos);
        bool InputDetected();
    }
}