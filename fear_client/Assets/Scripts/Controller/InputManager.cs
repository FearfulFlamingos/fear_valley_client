using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controller
{
    /// <summary>
    /// Moves all input into one place, making testing much easier.
    /// </summary>
    public class InputManager : MonoBehaviour, IInputManager
    {
        public bool GetLeftMouseClick() => Input.GetMouseButtonDown(0);
        public bool GetRightMouseClick() => Input.GetMouseButtonDown(1);
        public Vector3 MousePosition() => Input.mousePosition;
        public bool GetMoveButtonDown() => Input.GetKeyDown(KeyCode.Q);
        public bool GetAttackButtonDown() => Input.GetKeyDown(KeyCode.W);
        public bool GetMagicButtonDown() => Input.GetKeyDown(KeyCode.E);
        public bool GetRetreatButtonDown() => Input.GetKeyDown(KeyCode.R);
        public bool GetCancelButtonDown() => Input.GetKeyDown(KeyCode.Escape);
        public bool GetSpaceKeyDown() => Input.GetKeyDown(KeyCode.Space);
    }
}