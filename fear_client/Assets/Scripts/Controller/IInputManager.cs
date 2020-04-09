using UnityEngine;

namespace Scripts.Controller
{
    /// <summary>
    /// This is a wrapper class for Input testing.
    /// </summary>
    /// <remarks>
    /// While you don't necessarily need to have this class, you cannot test
    /// input without it.
    /// </remarks>
    public interface IInputManager
    {
        /// <summary>
        /// Checks whether the W key is being pressed down.
        /// </summary>
        /// <returns>Boolean state of the key.</returns>
        bool GetAttackButtonDown();

        /// <summary>
        /// Checks whether the Escape key is being pressed down.
        /// </summary>
        /// <returns>Boolean state of the key.</returns>
        bool GetCancelButtonDown();

        /// <summary>
        /// Checks whether the LMB is being clicked.
        /// </summary>
        /// <returns>Boolean state of the button.</returns>
        bool GetLeftMouseClick();

        /// <summary>
        /// Checks whether the E key is being pressed down.
        /// </summary>
        /// <returns>Boolean state of the key.</returns>
        bool GetMagicButtonDown();

        /// <summary>
        /// Checks whether the Q key is being pressed down.
        /// </summary>
        /// <returns>Boolean state of the key.</returns>
        bool GetMoveButtonDown();

        /// <summary>
        /// Checks whether the R key is being pressed down.
        /// </summary>
        /// <returns>Boolean state of the key.</returns>
        bool GetRetreatButtonDown();

        /// <summary>
        /// Checks whether the RMB is being clicked.
        /// </summary>
        /// <returns>Boolean state of the button.</returns>
        bool GetRightMouseClick();

        /// <summary>
        /// Checks whether the Space key is being pressed down.
        /// </summary>
        /// <returns>Boolean state of the key.</returns>
        bool GetSpaceKeyDown();

        /// <summary>
        /// The current position of the mouse in (x,y) pixel coordinates.
        /// </summary>
        /// <returns>The position as a Vector3 (x,y,0).</returns>
        Vector3 MousePosition();
    }
}