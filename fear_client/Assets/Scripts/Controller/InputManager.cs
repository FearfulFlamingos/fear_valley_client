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
        internal static bool GetLeftMouseClick() => Input.GetMouseButtonDown(0);
        internal static bool GetRightMouseClick() => Input.GetMouseButtonDown(1);
        internal static bool GetMoveButtonDown() => Input.GetKeyDown(KeyCode.Q);
        internal static bool GetAttackButtonDown() => Input.GetKeyDown(KeyCode.W);
        internal static bool GetMagicButtonDown() => Input.GetKeyDown(KeyCode.E);
        internal static bool GetRetreatButtonDown() => Input.GetKeyDown(KeyCode.R);
    }
}