using Scripts.Controller;
using UnityEngine;

/// <summary>
/// Scripts related to the actions a character can take.
/// </summary>
namespace Scripts.Actions
{
    /// <summary>
    /// Attaches to all character objects and controls movement.
    /// </summary>
    public interface IPlayerMovement
    {
        /// <summary>
        /// Input Manager that the script should use. See <see cref="Controller.InputManager"/> for implementation.
        /// </summary>
        /// <remarks>This is automatically assigned, but can be overridden for testing.</remarks>
        IInputManager InputManager { get; set; }

        /// <summary>
        /// Creates a movement selector from a prefab and sets its position 
        /// to the character's position.
        /// </summary>
        void ActivateMovement();
        
        /// <summary>Moves the gameobject to <paramref name="newPos"/> with the navmeshagent.</summary>
        /// <param name="newPos">This is the new vector where the object is put</param>
        void Move(Vector3 newPos);

        /// <summary>Checks for player input and ability to move a character</summary>
        /// <returns>Boolean value of input.</returns>
        bool InputDetected();

        /// <summary>
        /// This is a function that triggers movement, sends a client request, and toggles the checks for
        /// this script. It also resets the character state via a call to PlayerSpotlight.
        /// </summary>
        void FinishMovementAndReturn();
    }
}