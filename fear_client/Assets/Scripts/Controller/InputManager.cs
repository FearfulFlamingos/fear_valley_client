using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controller
{
    ///<inheritdoc/>
    public class InputManager : MonoBehaviour, IInputManager
    {
        ///<inheritdoc cref="IInputManager.GetLeftMouseClick"/>
        public bool GetLeftMouseClick() => Input.GetMouseButtonDown(0);

        ///<inheritdoc cref="IInputManager.GetRightMouseClick"/>
        public bool GetRightMouseClick() => Input.GetMouseButtonDown(1);

        ///<inheritdoc cref="IInputManager.MousePosition"/>
        public Vector3 MousePosition() => Input.mousePosition;

        ///<inheritdoc cref="IInputManager.GetMoveButtonDown"/>
        public bool GetMoveButtonDown() => Input.GetKeyDown(KeyCode.Q);

        ///<inheritdoc cref="IInputManager.GetAttackButtonDown"/>
        public bool GetAttackButtonDown() => Input.GetKeyDown(KeyCode.W);

        ///<inheritdoc cref="IInputManager.GetMagicButtonDown"/>
        public bool GetMagicButtonDown() => Input.GetKeyDown(KeyCode.E);

        ///<inheritdoc cref="IInputManager.GetRetreatButtonDown"/>
        public bool GetRetreatButtonDown() => Input.GetKeyDown(KeyCode.R);

        ///<inheritdoc cref="IInputManager.GetCancelButtonDown"/>
        public bool GetCancelButtonDown() => Input.GetKeyDown(KeyCode.Escape);

        ///<inheritdoc cref="IInputManager.GetSpaceKeyDown"/>
        public bool GetSpaceKeyDown() => Input.GetKeyDown(KeyCode.Space);
    }
}