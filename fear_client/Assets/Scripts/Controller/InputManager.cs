using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controller
{
    /// <summary>
    /// Moves all input into one place, making testing much easier.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public static bool GetLeftMouseClick() => Input.GetMouseButtonDown(0);
        public static bool GetRightMouseClick() => Input.GetMouseButtonDown(1);
        public static Vector3 mousePosition() => Input.mousePosition;

        public static bool GetMoveButtonDown() => Input.GetKeyDown(KeyCode.Q);
        public static bool GetAttackButtonDown() => Input.GetKeyDown(KeyCode.W);
        public static bool GetMagicButtonDown() => Input.GetKeyDown(KeyCode.E);
        public static bool GetRetreatButtonDown() => Input.GetKeyDown(KeyCode.R);
        public static bool GetCancelButtonDown() => Input.GetKeyDown(KeyCode.Escape);
        public static bool GetSpaceKeyDown() => Input.GetKeyDown(KeyCode.Space);
    }
}