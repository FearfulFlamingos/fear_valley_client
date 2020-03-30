using UnityEngine;

namespace Scripts.Controller
{
    public interface IInputManager
    {
        bool GetAttackButtonDown();
        bool GetCancelButtonDown();
        bool GetLeftMouseClick();
        bool GetMagicButtonDown();
        bool GetMoveButtonDown();
        bool GetRetreatButtonDown();
        bool GetRightMouseClick();
        bool GetSpaceKeyDown();
        Vector3 MousePosition();
    }
}